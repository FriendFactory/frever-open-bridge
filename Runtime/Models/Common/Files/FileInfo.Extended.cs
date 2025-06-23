using Newtonsoft.Json;
using ProtoBuf;
using UnityEngine;

namespace Bridge.Models.Common.Files
{
    /// <summary>
    /// That part contains properties which server doesn't have
    /// </summary>
    public partial class FileInfo
    {
        [JsonProperty, ProtoIgnore] public string FilePath { get; private set; }

        [JsonProperty, ProtoIgnore] public FileState State { get; private set; } = FileState.SyncedWithServer;
        
        internal FilePlacingType PlacingType => FileRawData != null ? FilePlacingType.InMemory : FilePlacingType.OnDisk;

        [JsonIgnore, ProtoIgnore] internal byte[] FileRawData { get; set; }

        public FileInfo(FileType fileType)
        {
            FileType = fileType;
            TagAsModified();
        }

        public FileInfo(FileType fileType, string fileVersion):this(fileType)
        {
            Version = fileVersion;
        }

        public FileInfo(string filePath, FileType fileType, FileExtension extension = FileExtension.Null):this(fileType)
        {
            FilePath = filePath;
            Extension = extension;
        }
        
        public FileInfo(string filePath, FileType fileType, Platform platform):this(fileType)
        {
            FilePath = filePath;
            Platform = platform;
        }

        public FileInfo(string filePath, FileType fileType, Resolution resolution) : this(filePath, fileType)
        {
            Resolution = resolution;
        }

        public FileInfo(Texture2D texture2D, FileExtension imageExtension, Resolution? resolution = null, FileType fileType = FileType.Thumbnail)
        {
            FileType = fileType;
            Extension = imageExtension;
            Resolution = resolution;
            FileRawData = imageExtension == FileExtension.Png
                ? texture2D.EncodeToPNG()
                : texture2D.EncodeToJPG();
            TagAsModified();
        }
        
        public FileInfo(byte[] textureBytes, FileExtension imageExtension, Resolution? resolution = null, FileType fileType = FileType.Thumbnail)
        {
            FileType = fileType;
            Extension = imageExtension;
            Resolution = resolution;
            FileRawData = textureBytes;
            TagAsModified();
        }

        public FileInfo(IFilesAttachedEntity copyFrom, FileInfo fileToCopy)
        {
            FileType = fileToCopy.FileType;
            Resolution = fileToCopy.Resolution;
            Extension = fileToCopy.Extension;
            TagAsShouldBeCopiedFromSource(copyFrom, fileToCopy);
        }
        
        internal FileInfo(FileType fileType, string fileVersion, FileExtension extension, Resolution? resolution):this(fileType)
        {
            Version = fileVersion;
            Extension = extension;
            Resolution = resolution;
        }
        
        public void TagAsModified()
        {
            State = FileState.ModifiedLocally;
        }

        internal void TasAsInUploadingProcess()
        {
            State = FileState.Uploading;
        }

        internal void TagAsDeployed()
        {
            State = FileState.PreUploaded;
        }

        internal void TagAsSyncedWithServer()
        {
            State = FileState.SyncedWithServer;
        }

        internal void ResetFilePath()
        {
            FilePath = null;
        }

        internal void ReleaseAssetRawDataFromRAM()
        {
            FileRawData = null;
        }

        private void TagAsShouldBeCopiedFromSource(IFilesAttachedEntity sourceModel, FileInfo fileInfo)
        {
            State = FileState.ShouldBeCopiedFromSource;

            Source = new FileSource
            {
                CopyFrom = new AssetFileSourceInfo
                {
                    Id = sourceModel.Id,
                    Version = fileInfo.Version
                }
            };
        }

        public bool Exists()
        {
            return System.IO.File.Exists(FilePath);
        }

        private bool Equals(FileInfo other)
        {
            return FileType == other.FileType && Resolution == other.Resolution 
                                              && Extension == other.Extension
                                              && FilePath == other.FilePath;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FileInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) FileType;
                hashCode = (hashCode * 397) ^ Resolution.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Extension;
                return hashCode;
            }
        }

        public static bool operator ==(FileInfo obj1, FileInfo obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }

            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return obj1.Equals(obj2);
        }

        public static bool operator !=(FileInfo obj1, FileInfo obj2)
        {
            return !(obj1 == obj2);
        }
    }
}