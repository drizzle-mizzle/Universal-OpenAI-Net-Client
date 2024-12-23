using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UniversalOpenAi.Client.Models;

namespace UniversalOpenAi.Client;


public class UniversalOpenAiClient
{
    private readonly HttpClient HTTP_CLIENT;

    private string? DEFAULT_URL_BASE;
    private string? DEFAULT_API_KEY;
    private IDictionary<string, string>? DEFAULT_HEADERS;

    private readonly JsonSerializerSettings _defaultSettings;
    private readonly JsonSerializer _defaultSerializer;


    public UniversalOpenAiClient()
    {
        HTTP_CLIENT = new HttpClient();

        _defaultSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        _defaultSerializer = JsonSerializer.Create(_defaultSettings);
    }


    public void SetDefaultBaseUrl(string? url)
    {
        DEFAULT_URL_BASE = url;
    }

    public void SetDefaultApiKey(string? apiKey)
    {
        DEFAULT_API_KEY = apiKey;
    }

    public void SetDefaultHeaders(IDictionary<string, string>? headers)
    {
        DEFAULT_HEADERS = headers;
    }

    public Task<CompletionsResponse> CompleteAsync(string model, ChatMessage[] messages, GenerationSettings? generationSettings = null)
        => CompleteAsync(DEFAULT_URL_BASE!, DEFAULT_API_KEY!, model, messages, generationSettings);

    public Task<CompletionsResponse> CompleteAsync(string apiKey, string model, ChatMessage[] messages, GenerationSettings? generationSettings = null)
        => CompleteAsync(DEFAULT_URL_BASE!, apiKey, model, messages, generationSettings);

    public async Task<CompletionsResponse> CompleteAsync(string baseUrl, string apiKey, string model, ChatMessage[] messages, GenerationSettings? generationSettings = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, baseUrl + "/chat/completions");

        request.Headers.Add("Authorization", $"Bearer {apiKey}");

        if (DEFAULT_HEADERS is not null)
        {
            foreach (var header in DEFAULT_HEADERS)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        var body = new JObject()
        {
            ["model"] = model,
            ["messages"] = JToken.FromObject(messages, _defaultSerializer),
        };

        if (generationSettings is not null)
        {
            body["temperature"] = generationSettings.Temperature;
            body["top_p"] = generationSettings.TopP;
            body["top_k"] = generationSettings.TopK;
            body["frequency_penalty"] = generationSettings.FrequencyPenalty;
            body["presence_penalty"] = generationSettings.PresencePenalty;
            body["repetition_penalty"] = generationSettings.RepetitionPenalty;
            body["min_p"] = generationSettings.MinP;
            body["top_a"] = generationSettings.TopA;
            body["max_tokens"] = generationSettings.MaxTokens;
        }

        var json = JsonConvert.SerializeObject(body, _defaultSettings);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await HTTP_CLIENT.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<CompletionsResponse>(content, _defaultSettings)!;
    }

    public Task<OpenAiModel[]> ModelsAsync()
        => ModelsAsync<OpenAiModel>();

    public Task<OpenAiModel[]> ModelsAsync(string apiKey)
        => ModelsAsync<OpenAiModel>(apiKey);

    public Task<OpenAiModel[]> ModelsAsync(string baseUrl, string apiKey)
        => ModelsAsync<OpenAiModel>(baseUrl, apiKey);

    public Task<T[]> ModelsAsync<T>()
        => ModelsAsync<T>(DEFAULT_URL_BASE!, DEFAULT_API_KEY!);

    public Task<T[]> ModelsAsync<T>(string apiKey)
        => ModelsAsync<T>(DEFAULT_URL_BASE!, apiKey);

    public async Task<T[]> ModelsAsync<T>(string baseUrl, string apiKey)
    {
        var requestUri = baseUrl + "/models";

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Add("Authorization", $"Bearer {apiKey}");

        if (DEFAULT_HEADERS is not null)
        {
            foreach (var header in DEFAULT_HEADERS)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        var response = await HTTP_CLIENT.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        var data = JObject.Parse(content)["data"]!;

        return JsonConvert.DeserializeObject<T[]>(data.ToString(), _defaultSettings)!;
    }
}
