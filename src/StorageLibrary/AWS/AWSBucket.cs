using System.Collections.Generic;
using System.IO;
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
	internal class AWSBucket : StorageObject, IContainer
	{
		protected AmazonS3Client _s3Client;
		public AWSBucket(StorageFactoryConfig config): base(config)
		{
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

		public async Task CreateBlobAsync(string bucket, string key, Stream fileContent)
		{
			var putRequest = new PutObjectRequest
			{
				BucketName = bucket,
				Key = key,
				InputStream = fileContent
			};

			await _s3Client.PutObjectAsync(putRequest);
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
			var response = await _s3Client.ListObjectsV2Async(request);

			var uriTemplate = $"https://{bucket}.s3.{RegionEndpoint.USEast1.SystemName}.amazonaws.com/";

			foreach (S3Object entry in response.S3Objects)
			{
				if (entry.Key == path)
					continue;

				blobs.Add(new BlobItemWrapper($"{uriTemplate}{entry.Key}", entry.Size, CloudProvider.AWS));
			}

			foreach (string commonPrefix in response.CommonPrefixes)
				blobs.Add(new BlobItemWrapper($"{uriTemplate}{commonPrefix}", 0, CloudProvider.AWS));

			return blobs;
		}

		public async Task<List<CloudBlobContainerWrapper>> ListContainersAsync()
		{
			var buckets = new List<CloudBlobContainerWrapper>();

			ListBucketsResponse response = await _s3Client.ListBucketsAsync();
			foreach (S3Bucket bucket in response.Buckets)
			{
				buckets.Add(new CloudBlobContainerWrapper() { Name = bucket.BucketName });
			}

			return buckets;
		}
	}
}