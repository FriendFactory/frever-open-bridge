using System;
using System.Collections.Generic;
using System.IO;
using Bridge.Models.ClientServer.Assets;
using UnityEngine;

namespace Bridge.Services.AssetService.Caching.Encryption
{
    internal sealed class EncryptionService
    {
        private readonly IEncryptedFileReader _encryptedFileReader;
        private readonly IEncryptedFileWriter _encryptedFileWriter;
        private readonly IPartialEncryptedFileReader _partialEncryptedFileReader;
        private readonly IPartialEncryptedFileWriter _partialEncryptedFileWriter;
        
        private readonly HashSet<Type> _encryptedTypes = new HashSet<Type>()
        {
            typeof(VoiceTrackFullInfo),
            typeof(UserSoundFullInfo),
            typeof(PhotoFullInfo),
        };
        
        private readonly HashSet<Type> _partialEncryptedTypes = new HashSet<Type>()
        {
            typeof(SetLocationBundleInfo),
            typeof(BodyAnimationInfo),
            typeof(CameraFilterVariantInfo),
            typeof(VfxInfo),
            typeof(UmaBundleFullInfo)
        };

        public EncryptionService()
        {
            var cryptoServiceProvider = new CryptoServiceProvider();
            _encryptedFileReader = new EncryptedFileReader(cryptoServiceProvider);
            _encryptedFileWriter = new EncryptedFileWriter(cryptoServiceProvider);
            _partialEncryptedFileReader = new PartialEncryptedFileReader(cryptoServiceProvider);
            _partialEncryptedFileWriter = new PartialEncryptedFileWriter(cryptoServiceProvider);
        }

        public bool EncryptionEnabled
        {
            get
            {
#if UNITY_2022_3_OR_NEWER  
            return false; // off until Oleh fixes issues with seeking
#else
            return Application.platform == RuntimePlatform.Android;
#endif
            }
        }
        
        public string TargetExtension => Constants.ENCRYPTED_FILE_EXTENSION;
        
        public bool ShouldEncrypt(Type targetType) => EncryptionEnabled && (IsPartiallyEncrypted( targetType) || _encryptedTypes.Contains(targetType));
        public bool IsPartiallyEncrypted(Type targetType) => _partialEncryptedTypes.Contains(targetType);

        public bool TryGetEncryptedExtension(string filePath, out string extension)
        {
            extension = string.Empty;
            
            if (string.IsNullOrEmpty(filePath) || !filePath.EndsWith(TargetExtension)) return false;
            
            extension = Path.GetExtension(filePath.Substring(0, filePath.Length - TargetExtension.Length));
            extension = $"{extension}{Constants.ENCRYPTED_FILE_EXTENSION}";
            
            return true;
        }

        public IEncryptedFileWriter GetEncryptedFileWriter() => _encryptedFileWriter;
        public IPartialEncryptedFileWriter GetPartialEncryptedFileWriter() => _partialEncryptedFileWriter;

        public IEncryptedFileReader GetEncryptedFileReader() => _encryptedFileReader;
        public IPartialEncryptedFileReader GetPartialEncryptedFileReader() => _partialEncryptedFileReader;
    }
}