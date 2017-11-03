using System;
using System.Globalization;
using System.Security.Cryptography;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageLibrary
{
	public class Client
	{
		internal static CloudStorageAccount GetAccount(string account, string key)
		{
			string sas = "";
			if (key.StartsWith("?"))
				sas = key;

			Uri blobUri = new Uri($"http://{account}.blob.core.windows.net/{sas}");
			Uri queueUri = new Uri($"http://{account}.queue.core.windows.net/{sas}");
			Uri tableUri = new Uri($"http://{account}.table.core.windows.net/{sas}");

			StorageCredentials credentials;
			CloudStorageAccount storageAccount;

			if (string.IsNullOrEmpty(sas))
			{
				credentials = new StorageCredentials(account, key);
				storageAccount = new CloudStorageAccount(credentials, true);
			}
			else
			{
				credentials = new StorageCredentials(sas);
				storageAccount = new CloudStorageAccount(credentials, account, null, true);
			}

			return storageAccount;
		}

		public static CloudBlobClient GetBlobClient(string account, string key) => GetAccount(account, key).CreateCloudBlobClient();

		public static CloudTableClient GetTableClient(string account, string key) => GetAccount(account, key).CreateCloudTableClient();

		public static CloudQueueClient GetQueueClient(string account, string key) => GetAccount(account, key).CreateCloudQueueClient();

		public static string GetSignedKey(string method, string contentType, string msDate, Uri baseUri, string account, string key)
		{
			// create the canonized string we're going to sign
			string stringToSign = method + "\n" +           // "VERB", aka the http method
				String.Empty + "\n" +                       // "Content-MD5", we not using MD5 on this request
				contentType + "\n" +                        // "Content-Type"
				msDate + "\n" +                             // "Date", this is a legacy value and not really needed
				"/" + account + baseUri.AbsolutePath;       // "CanonicalizedResources", just storage account name and path for this request

			// compute the MACSHA signature
			byte[] KeyAsByteArray = Convert.FromBase64String(key);
			byte[] dataToMAC = System.Text.Encoding.UTF8.GetBytes(stringToSign);
			string computedBase64Signature = string.Empty;
			using (HMACSHA256 hmacsha1 = new HMACSHA256(KeyAsByteArray))
				computedBase64Signature = Convert.ToBase64String(hmacsha1.ComputeHash(dataToMAC));

			// add the signature to the request
			string authorizationHeader = string.Format(CultureInfo.InvariantCulture, "SharedKey {0}:{1}", account, computedBase64Signature);

			return authorizationHeader;
		}
	}
}
