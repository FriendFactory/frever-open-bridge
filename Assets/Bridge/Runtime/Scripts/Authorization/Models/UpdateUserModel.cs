using System;

namespace Bridge.Authorization.Models
{
    public sealed class UpdateUserModel
    {
        public string DefaultLanguage { get; set; }
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }
    }
}