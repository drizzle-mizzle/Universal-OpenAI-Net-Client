namespace UniversalOpenAi.Client.Models;


public class GenerationSettings
{
    public float Temperature { get; set; } = 1f;
    public float TopP { get; set; } = 1f;
    public int TopK { get; set; } = 0;
    public float FrequencyPenalty { get; set; } = 0f;
    public float PresencePenalty { get; set; } = 0f;
    public float RepetitionPenalty { get; set; } = 1f;
    public float MinP { get; set; } = 0f;
    public float TopA { get; set; } = 0f;
    // public int? Seed { get; set; } = null;
    public int MaxTokens { get; set; } = 1000;
}
