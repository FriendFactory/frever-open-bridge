using System;
using System.Collections.Generic;
using Bridge.Models.Common.Files;

namespace Bridge.Models.AdminService
{
    public class CharacterBakedViewDto
    {
        public long CharacterId { get; set; }
        public long? OutfitId { get; set; }
        public long ReadinessId { get; set; }
        public float HeelsHeight { get; set; }
        public bool IsValid { get; set; }
        public string BakingMachineAgentName { get; set; }

        public Guid CharacterVersion { get; set; }
        public List<FileInfo> Files { get; set; }
    }
}