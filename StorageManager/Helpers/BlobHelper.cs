using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageHelper;
using Microsoft.WindowsAzure.StorageClient;
using System.ComponentModel;

namespace StorageManager.Helpers
{
    public class BlobHelper
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Type { get; set; }

        [Browsable(false)]
        public string BlobType { get; set; }

        public DateTime LastModfied { get; set; }
        public string ETag { get; set; }

        public static List<BlobHelper> GetAll(string container, string account, string key)
        {
            List<BlobHelper> list = new List<BlobHelper>();
            CloudBlobContainer cont = StorageHelper.Container.Get(account,key, container);

            if (cont == null)
                return list;

            BlobRequestOptions opt = new BlobRequestOptions();
            opt.BlobListingDetails = BlobListingDetails.All;
            opt.UseFlatBlobListing = true;

            foreach (IListBlobItem blob in cont.ListBlobs(opt))
            {
                CloudBlockBlob b = blob as CloudBlockBlob;
                if (b == null)
                    continue;

                b.FetchAttributes(opt);

                BlobHelper bh = new BlobHelper();
                bh.Url = b.Uri.ToString();
                bh.Name = b.Uri.Segments[b.Uri.Segments.Length - 1];
                bh.Size = b.Properties.Length;
                bh.Type = b.Properties.ContentType;
                bh.BlobType = b.Properties.BlobType.ToString();
                bh.LastModfied = b.Properties.LastModifiedUtc;
                bh.ETag = b.Properties.ETag;
                list.Add(bh);
            }

            return list;
        }
    
        public static void Delete(string account, string key, string url)
        {
            StorageHelper.Container.DeleteBlob(account, key, url);
        }
    }
}
