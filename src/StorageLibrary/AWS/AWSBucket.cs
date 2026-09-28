using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

using StorageLibrary.Common;
using StorageLibrary.Interfaces;

namespace StorageLibrary.AWS
{
	internal class AWSBucket : StorageObject, IContainer, IDisposable
	{
		protected AmazonS3Client _s3Client;
		private readonly string _region;
		public AWSBucket(StorageFactoryConfig config): base(config)
		{
			_region = config.AwsRegion;
			var credentials = new BasicAWSCredentials(config.AwsKey, config.AwsSecret);
			_s3Client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(config.AwsRegion));
		}

		public async Task CreateAsync(string bucket, bool publicAccess)
		{
			var putBucketRequest = new PutBucketRequest
			{
				BucketName = bucket,
			};

			await _s3Client.PutBucketAsync(putBucketRequest);

			if (publicAccess)
			{
				await SetBucketPolicyAsync(bucket);
			}
		}

		public async Task SetBucketPolicyAsync(string bucket)
		{
			var bucketPolicy = new
			{
				Version = "2012-10-17",
				Statement = new[]
				{
				new
				{
					Sid = "AddPerm",
					Effect = "Allow",
					Principal = "*",
					Action = "s3:GetObject",
					Resource = $"arn:aws:s3:::{bucket}/*"
				}
			}
			};

			var policyJson = JsonSerializer.Serialize(bucketPolicy);

			var putBucketPolicyRequest = new PutBucketPolicyRequest
			{
				BucketName = bucket,
				Policy = policyJson
			};

			await _s3Client.PutBucketPolicyAsync(putBucketPolicyRequest);
		}

		public async Task CreateBlobAsync(string bucket, string key, Stream fileContent, bool overwrite = false)
		{
			var putRequest = new PutObjectRequest
			{
				BucketName = bucket,
				Key = key,
				InputStream = fileContent
			};

			// S3 conditional write: the request fails with 412 when the key exists.
			if (!overwrite)
				putRequest.IfNoneMatch = "*";

			try
			{
				await _s3Client.PutObjectAsync(putRequest);
			}
			catch (AmazonS3Exception ex) when (!overwrite && ex.StatusCode == HttpStatusCode.PreconditionFailed)
			{
				throw new BlobAlreadyExistsException(bucket, key, ex);
			}
		}

		public async Task DeleteAsync(string bucket)
		{
			var deleteBucketRequest = new DeleteBucketRequest
			{
				BucketName = bucket
			};

			await _s3Client.DeleteBucketAsync(deleteBucketRequest);
		}

		public async Task DeleteBlobAsync(string bucket, string key)
		{
			var deleteObjectRequest = new DeleteObjectRequest
			{
				BucketName = bucket,
				Key = key
			};

			await _s3Client.DeleteObjectAsync(deleteObjectRequest);
		}

		public async Task<string> GetBlobAsync(string bucket, string key)
		{
			string tmpPath = Util.File.GetTempFileName();

			var getRequest = new GetObjectRequest
			{
				BucketName = bucket,
				Key = key
			};

			using (GetObjectResponse response = await _s3Client.GetObjectAsync(getRequest))
			using (Stream responseStream = response.ResponseStream)
			using (FileStream fileStream = File.Create(tmpPath))
			{
				await responseStream.CopyToAsync(fileStream);
			}

			return tmpPath;
		}

		public async Task<List<BlobItemWrapper>> ListBlobsAsync(string bucket, string path)
		{
			var request = new ListObjectsV2Request
			{
				BucketName = bucket,
				Prefix = path,
				Delimiter = Path.AltDirectorySeparatorChar.ToString()
			};


			var blobs = new List<BlobItemWrapper>();
			var uriTemplate = $"https://{bucket}.s3.{_region}.amazonaws.com/";

			// S3 returns at most 1,000 keys for each request.
			ListObjectsV2Response response;
			do
			{
				response = await _s3Client.ListObjectsV2Async(request);

				foreach (S3Object entry in response.S3Objects ?? [])
				{
					// Skip only the folder placeholder object ("folder/"). A prefix that is
					// the full name of a file must still return that file.
					if (entry.Key == path && path.EndsWith("/"))
						continue;

					blobs.Add(new BlobItemWrapper($"{uriTemplate}{entry.Key}", bucket, entry.Key, true, entry.Size ?? 0, CloudProvider.AWS));
				}

				foreach (string commonPrefix in response.CommonPrefixes ?? [])
					blobs.Add(new BlobItemWrapper($"{uriTemplate}{commonPrefix}", bucket, commonPrefix, false, 0, CloudProvider.AWS));

				request.ContinuationToken = response.NextContinuationToken;
			}
			while (response.IsTruncated == true);

			return blobs;
		}

		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			var buckets = new List<CloudBlobContainerWrapper>();

			// S3 returns a continuation token only when MaxBuckets is set. 10,000 is the
			// API maximum, so most accounts need one request.
			var request = new ListBucketsRequest { MaxBuckets = 10000 };

			ListBucketsResponse response;
			do
			{
				response = await _s3Client.ListBucketsAsync(request);

				foreach (S3Bucket bucket in response.Buckets ?? [])
					buckets.Add(new CloudBlobContainerWrapper() { Name = bucket.BucketName });

				request.ContinuationToken = response.ContinuationToken;
			}
			while (!string.IsNullOrEmpty(response.ContinuationToken));

			return buckets;
		}
	
		// Presigned PUT URLs (GetPreSignedURL) are not wired up yet; callers fall back
		// to uploading through the server.
		public Task<string> GetBlobUploadUrlAsync(string bucket, string key, TimeSpan validFor)
		{
			return Task.FromResult<string>(null);
		}

		public void Dispose()
    	{
        	_s3Client?.Dispose();
    	}
	}
}