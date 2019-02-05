using System.Linq;

namespace Ajf.NugetCrawler.Wpf
{
    public class NugetReference
    {
        public NugetReference(string path, string id, string version)
        {
            Path = path;
            Id = id;
            Version = version;

            FolderName = Path.Split('\\').Last();
        }

        public string FolderName { get; set; }

        public string Path { get; }
        public string Id { get; }
        public string Version { get; }

        //public int NumberOfOwnedNugets { get; set; }
        public bool IsNewest { get; set; }
    }
}