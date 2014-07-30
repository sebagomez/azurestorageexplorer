using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StorageHelper;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageManager.Helpers
{
    public class ContainerHelper
    {
        public string Name { get; set; }

        public static List<ContainerHelper> GetAll(string account, string key)
        {
            List<ContainerHelper> list = new List<ContainerHelper>();
            foreach (CloudBlobContainer cont in Container.GetAll(account,key))
            {
                ContainerHelper ch = new ContainerHelper() { Name = cont.Name };
                list.Add(ch);
            }

            return list;
        }

    }
}
