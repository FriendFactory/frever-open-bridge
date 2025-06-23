using System;

namespace Bridge.Authorization.Models
{
    public class UserRegistrationModel
    {
        public ICredentials Credentials;
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }

        public int? Gender { get; set; }
        public bool? AllowDataCollection { get; set; }
        public bool AnalyticsEnabled { get; set; }
        public string Country { get; set; }
        public string DefaultLanguage { get; set; }

        public UserRegistrationModel()
        {
        }

        public UserRegistrationModel(
            ICredentials credentials, string userName,
            DateTime birthDate,
            int? gender = null,
            bool? allowDataCollection = null,
            bool analyticsEnabled = false)
        {
            Credentials = credentials;
            UserName = userName;
            BirthDate = birthDate;
            Gender = gender;
            AllowDataCollection = allowDataCollection;
            AnalyticsEnabled = analyticsEnabled;
        }
    }
}