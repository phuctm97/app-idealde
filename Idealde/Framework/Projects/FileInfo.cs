namespace Idealde.Framework.Projects
{
    public class FileInfo
    {
        public string Name { set; get; }

        public string VirtualAddress { set; get; }

        public string MemoryAddress { set; get; }

        public FileInfo(string name, string virtualAddress, string memoryAddress)
        {
            Name = name;
            VirtualAddress = virtualAddress;
            MemoryAddress = memoryAddress;
        }
    }
}