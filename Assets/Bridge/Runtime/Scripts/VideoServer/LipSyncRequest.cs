namespace Bridge.VideoServer
{
    public class LipSyncRequest
    {
        public long? SongId { get; set; }
        public long? ExternalSongId { get; set; }
        public long? UserSoundId { get; set; }

        internal LipSyncRequest() { }
    }
    
    public class LipSyncRequestBuilder
    {
        private long? _songId;
        private long? _externalSongId;
        private long? _userSoundId;

        public LipSyncRequestBuilder WithSongId(long? songId)
        {
            _songId = songId;
            
            return this;
        }

        public LipSyncRequestBuilder WithExternalSongId(long? externalSongId)
        {
            _externalSongId = externalSongId;
            
            return this;
        }

        public LipSyncRequestBuilder WithUserSoundId(long? userSoundId)
        {
            _userSoundId = userSoundId;
            
            return this;
        }

        public LipSyncRequest Build()
        {
            return new LipSyncRequest
            {
                SongId = _songId,
                ExternalSongId = _externalSongId,
                UserSoundId = _userSoundId
            };
        }
    }
}