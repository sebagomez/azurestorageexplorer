using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageHelper;

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
