using System;
using Bridge.Models.AsseManager;

namespace Bridge.AssetManagerServer.ModelCleaning
{
    internal class CharacterCleaner : GenericCleaner<Character>
    {
        protected override Type[] AllowedTypesToCreate { get; } = 
        {
            typeof(CharacterAndUmaRecipe),typeof(UmaRecipe),
            typeof(UmaRecipeAndWardrobe)
        };
    }
}