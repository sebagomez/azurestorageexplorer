using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Security.Cryptography;
using System.Globalization;

namespace StorageHelper
{
	public class Client
	{
		static StorageCredentialsAccountAndKey s_credentials = null;
		static CloudStorageAccount s_storageAccount = null;
		static CloudBlobClient s_blobClient = null;
		static CloudTableClient s_tableClient = null;
		static CloudQueueClient s_queueClient = null;

		private static bool Initilized(string account)
		{
			return s_storageAccount != null && s_credentials.AccountName == account; 
		}

		public static void Initialize(string account, string key)
		{
			Uri blobUri = new Uri(string.Format("http://{0}.blob.core.windows.net", account));
			Uri queueUri = new Uri(string.Format("http://{0}.queue.core.windows.net", account));
			Uri tableUri = new Uri(string.Format("http://{0}.table.core.windows.net", account));

			s_credentials = new StorageCredentialsAccountAndKey(account, key);
			s_storageAccount = new CloudStorageAccount(s_credentials, blobUri, queueUri, tableUri);

			s_blobClient = s_storageAccount.CreateCloudBlobClient();
			s_tableClient = s_storageAccount.CreateCloudTableClient();
			s_queueClient = s_storageAccount.CreateCloudQueueClient();
		}

		public static CloudBlobClient GetBlobClient(string account, string key)
		{
			if (!Initilized(account))
				Initialize(account, key);

			return s_blobClient;
		}

		public static CloudTableClient GetTableClient(string account, string key)
		{
			if (!Initilized(account))
				Initialize(account, key);

			return s_tableClient;
		}

		public static CloudQueueClient GetQueueClient(string account, string key)
		{
			if (!Initilized(account))
				Initialize(account, key);

			return s_queueClient;
		}

		public static string GetSignedKey(string method, string contentType, string msDate, Uri baseUri, string account, string key)
		{
			// create the canonized string we're going to sign
			string stringToSign = method + "\n" +			// "VERB", aka the http method
				String.Empty + "\n" +						// "Content-MD5", we not using MD5 on this request
				contentType + "\n" +						// "Content-Type"
				msDate + "\n" +								// "Date", this is a legacy value and not really needed
				"/" + account +  baseUri.AbsolutePath;		// "CanonicalizedResources", just storage account name and path for this request

			// compute the MACSHA signature
			byte[] KeyAsByteArray = Convert.FromBase64String(key);
			byte[] dataToMAC = System.Text.Encoding.UTF8.GetBytes(stringToSign);
			string computedBase64Signature = string.Empty;
			using (HMACSHA256 hmacsha1 = new HMACSHA256(KeyAsByteArray))
				computedBase64Signature = System.Convert.ToBase64String(hmacsha1.ComputeHash(dataToMAC));

			// add the signature to the request
			string authorizationHeader = string.Format(CultureInfo.InvariantCulture, "SharedKey {0}:{1}",account, computedBase64Signature);

			return authorizationHeader;
		}
	}
}
