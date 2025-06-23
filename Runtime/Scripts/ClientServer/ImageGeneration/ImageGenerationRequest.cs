using System.Collections.Generic;

namespace Bridge.ClientServer.ImageGeneration
{
    public sealed class CreateImageRequest
    {
        public string Engine { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CfgScale { get; set; }
        public int Steps { get; set; }
        public List<TextPrompt> TextPrompts { get; set; }
    }
    
    public static class Engine
    {
        public const string SD_v1_6 = "stable-diffusion-v1-6";
        public const string SD_v2_1 = "stable-diffusion-512-v2-1";
        public const string SDXL_v1_0 = "stable-diffusion-xl-1024-v1-0";
        public const string SDXL_BETA_v2_2 = "stable-diffusion-xl-beta-v2-2-2";
    }

    public static class ImageStyle
    {
        public const string Vivid = "vivid";
        public const string Natural = "natural";
    }
        
    public struct TextPrompt
    {
        public string Text { get; set; }
        public float Weight { get; set; }
    }
}