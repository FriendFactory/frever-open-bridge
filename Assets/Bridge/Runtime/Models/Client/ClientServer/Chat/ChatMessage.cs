using System;
using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;
using UnityEngine;

namespace Bridge.Models.ClientServer.Chat
{
    public interface IChatMessageModel : IFilesAttached
    {
    }

    public class ChatMessage: IChatMessageModel, IFilesAttachedEntity
    {
        public long Id { get; set; }
        public GroupShortInfo Group { get; set; }
        public string Text { get; set; }
        public long? VideoId { get; set; }
        public long LikeCount { get; set; }
        public DateTime Time { get; set; }
        public long MessageTypeId { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
        public GroupShortInfo[] Mentions { get; set; }
        public List<FileInfo> Files { get; set; }
        public ChatMessage ReplyToMessage { get; set; }
    }

    public static class ChatMessageExtension
    {
        public static void AttachFile(this IChatMessageModel chatMessage, Texture2D image, FileExtension extension)
        {
            var fileInfo = new FileInfo(image, extension, fileType: FileType.MainFile);
            chatMessage.InitializeFileListIfNotYet();
            chatMessage.Files.Add(fileInfo);
        }

        public static void AttachFile(this IChatMessageModel chatMessage, string filePath)
        {
            var fileInfo = new FileInfo(filePath, FileType.MainFile);
            chatMessage.InitializeFileListIfNotYet();
            chatMessage.Files.Add(fileInfo);
        }

        private static void InitializeFileListIfNotYet(this IChatMessageModel chatMessage)
        {
            if (chatMessage.Files == null)
            {
                chatMessage.Files = new List<FileInfo>();
            }
        }
    }
}