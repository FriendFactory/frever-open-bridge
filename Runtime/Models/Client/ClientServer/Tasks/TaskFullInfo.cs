using System;
using System.Collections.Generic;
using Bridge.Models.Common;
using Bridge.Models.Common.Files;

namespace Bridge.Models.ClientServer.Tasks
{
    public class TaskFullInfo: IEntity, ITaggable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long? LevelId { get; set; }

        public long? TemplateId { get; set; }

        public int XpPayout { get; set; }

        public int BonusXp { get; set; }

        public int SoftCurrencyPayout { get; set; }

        public int BonusSoftCurrency { get; set; }

        public int CharacterCount { get; set; }

        public int? TotalTime { get; set; }

        public bool IsDressed { get; set; }

        public bool DeletionAllowed { get; set; }

        public int CreatorsCount { get; set; }

        public TaskType TaskType { get; set; }

        public DateTime Deadline { get; set; }

        public List<FileInfo> Files { get; set; }

        public long[] Tags { get; set; }

        public Page[] Pages { get; set; }
        
        public BattleReward[] BattleRewards { get; set; }
        
        public bool IsAvailableForVoting { get; set; }
        
        public DateTime? BattleResultReadyAt { get; set; }
    }
    
    public class BattleReward
    {
        public long Id { get; set; }
        public int Place { get; set; }
        public int SoftCurrency { get; set; }
    }
    
    public enum TaskType
    {
        Daily = 0,
        Weekly = 1,
        Season = 2,
        Onboarding = 3,
        Voting = 4
    }

    public enum Page
    {
        LevelEditor,
        PostRecordEditor,
        CharacterEditor
    }
}
