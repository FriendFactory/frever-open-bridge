using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiTests;
using Bridge.Models.AsseManager;
using Bridge.Models.Common.Files;
using Newtonsoft.Json;
using UnityEngine;
using Resolution = Bridge.Models.Common.Files.Resolution;

namespace Git_Ignore.Scripts
{
    public class CreateCharacterTest : EntityApiTest<Character>
    {
        private const string ThumbnailPath1 = @"thumbnail.png";
        private const string AvatarPath2 = @"thumbnail2.png";

        protected override async void RunTestAsync()
        {
            var recipeJson =
                "{\"packedRecipeType\":\"DynamicCharacterAvatar\",\"name\":\"FreverUMADynamicCharacterAvatar(Clone)\",\"race\":\"Female Base\",\"dna\":[{\"dnaType\":\"DynamicUMADna\",\"dnaTypeHash\":965488746,\"packedDna\":\"{\\\"bDnaAsset\\\":{\\\"instanceID\\\":302786},\\\"bDnaAssetName\\\":\\\"LipsDnaAsset\\\",\\\"bDnaSettings\\\":[{\\\"name\\\":\\\"mouthSize\\\",\\\"value\\\":128},{\\\"name\\\":\\\"mouthUpperlipSize\\\",\\\"value\\\":140},{\\\"name\\\":\\\"mouthLowerlipSize\\\",\\\"value\\\":172},{\\\"name\\\":\\\"mouthCorners\\\",\\\"value\\\":0},{\\\"name\\\":\\\"mouthPosition\\\",\\\"value\\\":64},{\\\"name\\\":\\\"mouthCupidbow\\\",\\\"value\\\":242},{\\\"name\\\":\\\"mouthCupidbowAdjust\\\",\\\"value\\\":170},{\\\"name\\\":\\\"mouthTeardrop\\\",\\\"value\\\":90},{\\\"name\\\":\\\"mouthTeardropAdjust\\\",\\\"value\\\":147},{\\\"name\\\":\\\"mouthCornersVertical\\\",\\\"value\\\":189}]}\"},{\"dnaType\":\"DynamicUMADna\",\"dnaTypeHash\":587454143,\"packedDna\":\"{\\\"bDnaAsset\\\":{\\\"instanceID\\\":350888},\\\"bDnaAssetName\\\":\\\"BodyFemaleDnaAsset\\\",\\\"bDnaSettings\\\":[{\\\"name\\\":\\\"weight\\\",\\\"value\\\":122},{\\\"name\\\":\\\"muscle\\\",\\\"value\\\":135},{\\\"name\\\":\\\"gluteSize\\\",\\\"value\\\":205},{\\\"name\\\":\\\"hipSize\\\",\\\"value\\\":25},{\\\"name\\\":\\\"thighThickness\\\",\\\"value\\\":115},{\\\"name\\\":\\\"calfThickness\\\",\\\"value\\\":87},{\\\"name\\\":\\\"bellySize\\\",\\\"value\\\":62},{\\\"name\\\":\\\"neckSize\\\",\\\"value\\\":128},{\\\"name\\\":\\\"waistSize\\\",\\\"value\\\":108},{\\\"name\\\":\\\"trapeziusSize\\\",\\\"value\\\":134},{\\\"name\\\":\\\"shoulderSize\\\",\\\"value\\\":114},{\\\"name\\\":\\\"armThickness\\\",\\\"value\\\":79},{\\\"name\\\":\\\"forearmThickness\\\",\\\"value\\\":81},{\\\"name\\\":\\\"ribcageSize\\\",\\\"value\\\":145},{\\\"name\\\":\\\"chestSize\\\",\\\"value\\\":128},{\\\"name\\\":\\\"breastSize\\\",\\\"value\\\":177}]}\"},{\"dnaType\":\"DynamicUMADna\",\"dnaTypeHash\":1782541532,\"packedDna\":\"{\\\"bDnaAsset\\\":{\\\"instanceID\\\":351444},\\\"bDnaAssetName\\\":\\\"EyeFemaleDnaAsset\\\",\\\"bDnaSettings\\\":[{\\\"name\\\":\\\"eyeSize\\\",\\\"value\\\":122},{\\\"name\\\":\\\"eyePosition\\\",\\\"value\\\":117},{\\\"name\\\":\\\"eyeWidth\\\",\\\"value\\\":232},{\\\"name\\\":\\\"eyeRotate\\\",\\\"value\\\":87},{\\\"name\\\":\\\"eyeSizeWidth\\\",\\\"value\\\":110},{\\\"name\\\":\\\"eyeLowerlid\\\",\\\"value\\\":194},{\\\"name\\\":\\\"eyeUpperlid\\\",\\\"value\\\":173},{\\\"name\\\":\\\"monolid\\\",\\\"value\\\":255},{\\\"name\\\":\\\"creasedlid\\\",\\\"value\\\":133},{\\\"name\\\":\\\"doublelid\\\",\\\"value\\\":133},{\\\"name\\\":\\\"eyeUpperlidInnerVertical\\\",\\\"value\\\":150},{\\\"name\\\":\\\"eyeUpperlidInnerHorizontal\\\",\\\"value\\\":122},{\\\"name\\\":\\\"eyeUpperlidMidVertical\\\",\\\"value\\\":163},{\\\"name\\\":\\\"eyeUpperlidMidHorizontal\\\",\\\"value\\\":138},{\\\"name\\\":\\\"eyeUpperlidOuterVertical\\\",\\\"value\\\":51},{\\\"name\\\":\\\"eyeUpperlidOuterHorizontal\\\",\\\"value\\\":145},{\\\"name\\\":\\\"eyeLowerlidInnerVertical\\\",\\\"value\\\":128},{\\\"name\\\":\\\"eyeLowerlidInnerHorizontal\\\",\\\"value\\\":128},{\\\"name\\\":\\\"eyeLowerlidMidVertical\\\",\\\"value\\\":128},{\\\"name\\\":\\\"eyeLowerlidMidHorizontal\\\",\\\"value\\\":128},{\\\"name\\\":\\\"eyeLowerlidOuterVertical\\\",\\\"value\\\":135},{\\\"name\\\":\\\"eyeLowerlidOuterHorizontal\\\",\\\"value\\\":185},{\\\"name\\\":\\\"eyeInner\\\",\\\"value\\\":176},{\\\"name\\\":\\\"eyeOuter\\\",\\\"value\\\":95}]}\"},{\"dnaType\":\"DynamicUMADna\",\"dnaTypeHash\":947784845,\"packedDna\":\"{\\\"bDnaAsset\\\":{\\\"instanceID\\\":277030},\\\"bDnaAssetName\\\":\\\"FaceDnaAsset\\\",\\\"bDnaSettings\\\":[{\\\"name\\\":\\\"cheekSize\\\",\\\"value\\\":122},{\\\"name\\\":\\\"cheekBoneSize\\\",\\\"value\\\":0},{\\\"name\\\":\\\"earSize\\\",\\\"value\\\":128},{\\\"name\\\":\\\"chinLength\\\",\\\"value\\\":124},{\\\"name\\\":\\\"chinHeigth\\\",\\\"value\\\":115},{\\\"name\\\":\\\"chinWidth\\\",\\\"value\\\":13},{\\\"name\\\":\\\"jawLength\\\",\\\"value\\\":82},{\\\"name\\\":\\\"jawSize\\\",\\\"value\\\":128},{\\\"name\\\":\\\"jawWidth\\\",\\\"value\\\":150},{\\\"name\\\":\\\"jawline\\\",\\\"value\\\":87},{\\\"name\\\":\\\"faceWidth\\\",\\\"value\\\":65},{\\\"name\\\":\\\"faceHeigth\\\",\\\"value\\\":118},{\\\"name\\\":\\\"headSize\\\",\\\"value\\\":128},{\\\"name\\\":\\\"earPosition\\\",\\\"value\\\":128}]}\"},{\"dnaType\":\"DynamicUMADna\",\"dnaTypeHash\":2140988384,\"packedDna\":\"{\\\"bDnaAsset\\\":{\\\"instanceID\\\":319960},\\\"bDnaAssetName\\\":\\\"NoseDnaAsset\\\",\\\"bDnaSettings\\\":[{\\\"name\\\":\\\"noseSize\\\",\\\"value\\\":128},{\\\"name\\\":\\\"nosePosition\\\",\\\"value\\\":99},{\\\"name\\\":\\\"noseWidth\\\",\\\"value\\\":161},{\\\"name\\\":\\\"noseLength\\\",\\\"value\\\":105},{\\\"name\\\":\\\"noseTipLength\\\",\\\"value\\\":0},{\\\"name\\\":\\\"noseTipSize\\\",\\\"value\\\":204},{\\\"name\\\":\\\"noseWingWidth\\\",\\\"value\\\":160},{\\\"name\\\":\\\"noseWingPosition\\\",\\\"value\\\":128},{\\\"name\\\":\\\"noseTipRotate\\\",\\\"value\\\":210}]}\"},{\"dnaType\":\"DynamicUMADna\",\"dnaTypeHash\":1147519000,\"packedDna\":\"{\\\"bDnaAsset\\\":{\\\"instanceID\\\":352812},\\\"bDnaAssetName\\\":\\\"EyebrowDnaAsset\\\",\\\"bDnaSettings\\\":[{\\\"name\\\":\\\"eyebrowSize\\\",\\\"value\\\":105},{\\\"name\\\":\\\"eyebrowThickness\\\",\\\"value\\\":102},{\\\"name\\\":\\\"eyebrowRotate\\\",\\\"value\\\":176},{\\\"name\\\":\\\"eyebrowPosition\\\",\\\"value\\\":0}]}\"},{\"dnaType\":\"DynamicUMADna\",\"dnaTypeHash\":296061143,\"packedDna\":\"{\\\"bDnaAsset\\\":{\\\"instanceID\\\":351416},\\\"bDnaAssetName\\\":\\\"EyelashesFemaleDnaAsset\\\",\\\"bDnaSettings\\\":[{\\\"name\\\":\\\"eyelashSize\\\",\\\"value\\\":255}]}\"}],\"characterColors\":[{\"name\":\"Skin\",\"colors\":[255,244,239,255,0,0,0,0,255,255,255,255,0,0,0,0,255,255,255,255,0,0,0,0]},{\"name\":\"Hair\",\"colors\":[127,109,255,255,0,0,0,0,255,255,255,255,0,0,0,0,255,255,255,255,0,0,0,0]},{\"name\":\"Eyes\",\"colors\":[138,204,229,255,0,0,0,0,255,255,255,255,0,0,0,0,255,255,255,255,0,0,0,0]},{\"name\":\"Eyebrow\",\"colors\":[86,0,117,255,0,0,0,0,255,255,255,255,0,0,0,0,255,255,255,255,0,0,0,0]},{\"name\":\"Eyeliner\",\"colors\":[0,0,0,255,0,0,0,0,255,255,255,255,0,0,0,0,255,255,255,255,0,0,0,0]},{\"name\":\"Blush\",\"colors\":[254,94,101,255,0,0,0,0,255,255,255,255,0,0,0,0,255,255,255,255,0,0,0,0]},{\"name\":\"Eyeshadow\",\"colors\":[255,10,189,255,0,0,0,0,255,255,255,255,0,0,0,0,255,255,255,255,0,0,0,0]}],\"wardrobeSet\":[{\"slot\":\"Skirts\",\"recipe\":\"skirt_F_Medium_Aline_v1_Recipe\"},{\"slot\":\"EyeMakeup\",\"recipe\":\"makeup_Eyeshadow_v4_Recipe\"},{\"slot\":\"Hair\",\"recipe\":\"hair_Short_cap_hair_v1_Recipe\"},{\"slot\":\"Facial Hair\",\"recipe\":\"lashes_CutOut_v1_Recipe\"},{\"slot\":\"Shirts\",\"recipe\":\"shirt_F_Top_Crop_fullsleeve_v1_Striped_Recipe\"},{\"slot\":\"Jewelry\",\"recipe\":\"jewelry_Earring_Stud_v1_Recipe\"},{\"slot\":\"FaceMakeup\",\"recipe\":\"facemakeup_Blush_v2_Recipe\"}],\"raceAnimatorController\":\"Locomotion\"}";

            var character = new Character
            {
                CharacterAndUmaRecipe = new List<CharacterAndUmaRecipe>
                {
                    new CharacterAndUmaRecipe
                    {
                        UmaRecipe = new UmaRecipe {J = Encoding.ASCII.GetBytes(recipeJson)}
                    }
                },
                GenderId = 1,
                CharacterStyleId = 1,
                DefaultOutfitId = await GetAnyAvailableEntityId<Outfit>()
            };


            var thumbnail128 = new FileInfo(GetFilePath(ThumbnailPath1), FileType.Thumbnail, Resolution._128x128);
            var thumbnail256 = new FileInfo(GetFilePath(ThumbnailPath1), FileType.Thumbnail, Resolution._256x256);
            var thumbnail512 = new FileInfo(GetFilePath(ThumbnailPath1), FileType.Thumbnail, Resolution._512x512);
            character.Files = new List<FileInfo>(){thumbnail128, thumbnail256, thumbnail512};

            var res = await Bridge.PostAsync(character);
            if (res.IsSuccess)
            {
                if (res.ResultObject == null)
                    Debug.Log("NULL");

                Debug.Log(JsonConvert.SerializeObject(res.ResultObject));
                character.Id = res.ResultObject.Id;
                character.CharacterAndUmaRecipe.First().UmaRecipeId =
                    res.ResultObject.CharacterAndUmaRecipe.First().UmaRecipeId;
                character.CharacterAndUmaRecipe.First().CharacterId =
                    res.ResultObject.CharacterAndUmaRecipe.First().CharacterId;
                character.CharacterAndUmaRecipe.First().UmaRecipe.Id =
                    res.ResultObject.CharacterAndUmaRecipe.First().UmaRecipe.Id;
                Debug.Log("Success");
            }
            else
            {
                Debug.LogError(res.ErrorMessage);
            }
        }
    }
}