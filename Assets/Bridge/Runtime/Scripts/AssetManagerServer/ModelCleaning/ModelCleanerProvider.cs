using System.Linq;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal class ModelCleanerProvider
    {
        private readonly SendModelCleaner[] _cleaners = 
        {
            new LevelCleaner(),
            new EventCleaner(),
            new CharacterCleaner(),
            new SetLocationBundleCleaner(),
            new SetLocationCleaner(),
            new WardrobeCleaner()
        };

        public SendModelCleaner GetCleaner<T>() where T: IEntity
        {
            var targetType = typeof(T);
            var cleaner = _cleaners.FirstOrDefault(x => x.TargetType == targetType);
            return cleaner ?? new DefaultCleaner();
        }
    }
}