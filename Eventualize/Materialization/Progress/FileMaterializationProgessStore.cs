using System.IO;
using System.Linq;
using System.Reflection;

using Eventualize.Infrastructure;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization;
using Eventualize.Persistence;

namespace Eventualize.Materialization.Progress
{
    public class FileMaterializationProgessStore : IMaterializationProgessStore
    {
        private string folder;

        private string filePattern;

        private ISerializer serializer;

        public FileMaterializationProgessStore(ISerializer serializer, string folder = "Progess", string filePattern = "Progess_{0}.txt")
        {
            this.serializer = serializer;
            this.folder = folder;
            this.filePattern = filePattern;

            this.EnsurePaths();
        }

        public T GetProgess<T>(string key)
        {
            var filePath = this.GetFilePath(key);
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            return (T)this.serializer.Deserialize(typeof(T), File.ReadAllBytes(filePath));
        }

        public void SaveProgess<T>(string key, T currentProgess)
        {
            File.WriteAllBytes(this.GetFilePath(key), this.serializer.Serialize(currentProgess));
        }

        private void EnsurePaths()
        {
            var someFile = this.GetFilePath("asd");
            var directory = Path.GetDirectoryName(someFile);
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private string GetFilePath(string key)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), this.folder, string.Format(this.filePattern, key));
        }
    }
}