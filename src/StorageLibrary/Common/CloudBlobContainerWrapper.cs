namespace StorageLibrary.Common
{
    public class CloudBlobContainerWrapper
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
