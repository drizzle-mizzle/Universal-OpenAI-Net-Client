namespace UniversalOpenAi.Client.Models;


public class OpenAiModel
{
    public string Id { get; set; } = null!; // "id": "model-id-0",
    public string Object { get; set; } = null!; // "object": "model",
    public int Created { get; set; } // "created": 1686935002,
    public string OwnedBy { get; set; } = null!; // "owned_by": "organization-owner"
}
