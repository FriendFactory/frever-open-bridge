using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bridge.Models.ClientServer.Chat;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;
using UnityEngine;
using FileInfo = Bridge.Models.Common.Files.FileInfo;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace ApiTests.ChatTests
{
    internal sealed class PostMessageToChat : AuthorizedUserApiTestBase
    {
        [SerializeField] private long _chatId;
        [SerializeField] private string _message;
        [SerializeField] private bool _addFile;
        [SerializeField] private FilePassingType _filePassing;
        protected override async void RunTestAsync()
        {
            var messageModel = new AddMessageModel
            {
                Text = _message
            };
            if (_addFile)
            {
                AttachFile(messageModel);
            }
            var resp = await Bridge.PostMessage(_chatId, messageModel);
            
            Debug.Log($"Result: {JsonConvert.SerializeObject(resp)}");
            
            if (!_addFile) return;
            var getMessageResp = await Bridge.GetChatMessages(_chatId, null, 1, 1);
            var message = getMessageResp.Models.First();
            var filesResp = await Bridge.GetMessageFiles(message);
            if (filesResp.IsError)
            {
                Debug.LogError($"Failed to download chat message files. Reason: {filesResp.ErrorMessage}");
                return;
            }
            
            Debug.Log($"Success. Image name: {filesResp.Images.First().name}");
        }

        private void AttachFile(IChatMessageModel chatMessage)
        {
            var filePath = GetFilePath(TestFileNames.IMAGE_JPG);
            switch (_filePassing)
            {
                case FilePassingType.FilePath:
                    chatMessage.AttachFile(filePath);
                    return;
                case FilePassingType.Texture2D:
                    var fileBytes = File.ReadAllBytes(filePath);
                    var texture = new Texture2D(2, 2);
                    texture.LoadRawTextureData(fileBytes);
                    texture.Apply();
                    chatMessage.AttachFile(texture, FileExtension.Jpg);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private enum FilePassingType
        {
            FilePath,
            Texture2D
        }
    }
}
