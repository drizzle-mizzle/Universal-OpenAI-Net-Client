namespace UniversalOpenAi.Client.Models;


public class CompletionsResponse
{
    public string Id { get; set; } = null!; // "id": "chatcmpl-123",
    public string Object { get; set; } = null!; // "object": "chat.completion",
    public int Created { get; set; } // "created": 1677652288,
    public string Model { get; set; } = null!; // "model": "gpt-4o-mini"
    public string SystemFingerprint { get; set; } = null!; // "system_fingerprint": "fp_44709d6fcb",
    public Choice[] Choices { get; set; } = []; // "choices": [ {} ]
    public Usage? Usage { get; set; }  // "usage": { }
}


public class Choice
{
    public int Index { get; set; } // "index": 0,
    public ChatMessage? Message { get; set; } // "message": { },
    public string? Logprobs { get; set; } // "logprobs": null,
    public string FinishReason { get; set; } = null!; // "finish_reason": "stop"
}


public class Usage
{
    public int PromptTokens { get; set; }// "prompt_tokens": 9,
    public int CompletionTokens { get; set; }// "completion_tokens": 12,
    public int TotalTokens { get; set; }// "total_tokens": 21,
    public CompletionTokensDetails? CompletionTokensDetails { get; set; } // "completion_tokens_details": { }
}


public class CompletionTokensDetails
{
    public int ReasoningTokens { get; set; } // "reasoning_tokens": 0,
    public int AcceptedPredictionTokens { get; set; } // "accepted_prediction_tokens": 0,
    public int RejectedPredictionTokens { get; set; } // "rejected_prediction_tokens": 0
}
