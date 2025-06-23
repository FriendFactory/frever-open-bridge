using System;
using System.Collections.Generic;
using System.IO;
using Bridge.Models.Common.Files;
using UnityEngine;

namespace Bridge.AssetManagerServer
{
    internal static class FileExtensionManager
    {
        private static readonly Dictionary<string, FileExtension> ExtDictionary =
            new Dictionary<string, FileExtension>(StringComparer.OrdinalIgnoreCase)
            {
                {"gif", FileExtension.Gif},
                {"mp3", FileExtension.Mp3},
                {"ogg", FileExtension.Ogg},
                {"wav", FileExtension.Wav},
                {"png", FileExtension.Png},
                {"txt", FileExtension.Txt},
                {"", FileExtension.Empty},
                {"mp4", FileExtension.Mp4},
                {"jpg", FileExtension.Jpg},
                {"jpeg", FileExtension.Jpeg},
                {"mov", FileExtension.Mov},
            };

        public static FileExtension GetFileExtension(string filePath)
        {
            var extSting = Path.GetExtension(filePath).Replace(".", string.Empty);
            if (ExtDictionary.TryGetValue(extSting, out var output))
            {
                return output;
            }
            Debug.LogWarning($"Unregistered format in file: {filePath}. Extension: {extSting}");
            return FileExtension.Null;
        }
    }
}