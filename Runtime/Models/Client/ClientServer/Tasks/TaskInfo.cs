using System;
using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Tasks
{
    public class TaskInfo: IEntity, INamed, IThumbnailOwner
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int XpPayout { get; set; }

        public int SoftCurrencyPayout { get; set; }

        public string Description { get; set; }

        public int SortOrder { get; set; }

        public int CreatorsCount { get; set; }

        public TaskType TaskType { get; set; }

        public DateTime Deadline { get; set; }
        
        public List<FileInfo> Files { get; set; }
        
        public DateTime? BattleResultReadyAt { get; set; }
    }
}
