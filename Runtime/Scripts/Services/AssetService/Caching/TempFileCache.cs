using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching
{
    internal sealed class TempFileCache
    {
        private const string FOLDER = "TempFiles";
        private static readonly string PERSISTENT_DATA_PATH = Application.persistentDataPath;
        private readonly IFileWriter _fileWriter = new FileWriter();
        private readonly string _cacheRootFolder;

        private string RootPath => Path.Combine(PERSISTENT_DATA_PATH, _cacheRootFolder, FOLDER);

        public TempFileCache(string cacheRootFolder)
        {
            _cacheRootFolder = cacheRootFolder;
        }

        public async Task<string> SaveAsync(string fileName, byte[] data)
        {
            CreateRootFolder();
            var filePath = GetFilePath(fileName);
            await _fileWriter.WriteFileBytesAsync(filePath, data);
            return filePath;
        }

        public void Clear()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, true);
            }
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(RootPath, fileName);
        }
        
        private void CreateRootFolder()
        {
            if (!Directory.Exists(RootPath))
            {
                Directory.CreateDirectory(RootPath);
            }
        }
    }
}