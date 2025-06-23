namespace Bridge.ClientServer.Assets.Vfxs
{
    public class VfxFilterModel
    {
        public long RaceId { get; set; }

        public long? VfxCategoryId { get; set; }

        public string Name { get; set; }

        public long? TaskId { get; set; }

        public long[] TagIds { get; set; }

        public string UnityVersion { get; set; }

        public long? Target { get; set; }

        public int TakePrevious { get; set; }

        public int TakeNext { get; set; } = 20;
        public bool? WithAnimationOnly { get; set; }
    }
}