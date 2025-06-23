namespace Bridge.ClientServer.ImageGeneration
{
    public sealed class ReplicateRequest
    {
        public string Version { get; set; }
        public ReplicateInput Input { get; set; }
    }

    public sealed class ReplicateInput
    {
        public int Width { get; set; } = 1024;
        public int Height { get; set; } = 1024;
        public string Prompt { get; set; }
        public Scheduler Scheduler { get; set; } = Scheduler.K_EULER;
        public Refine Refine { get; set; } = Refine.no_refiner;
        public float Lora_scale { get; set; } = 0.6f;
        public int Num_outputs { get; set; } = 1;
        public float Guidance_scale { get; set; } = 7.5f;
        public bool Apply_watermark { get; set; } = true;
        public float High_noise_frac { get; set; } = 0.8f;
        public string Negative_prompt { get; set; }
        public float Prompt_strength { get; set; } = 0.8f;
        public int Num_inference_steps { get; set; } = 50;
    }
    
    public enum Scheduler
    {
        DDIM,
        DPMSolverMultistep,
        HeunDiscrete,
        KarrasDPM,
        K_EULER_ANCESTRAL,
        K_EULER,
        PNDM
    }
    
    //no_refiner, expert_ensemble_refiner, base_image_refiner
    public enum Refine
    {
        no_refiner,
        expert_ensemble_refiner,
        base_image_refiner
    }
}