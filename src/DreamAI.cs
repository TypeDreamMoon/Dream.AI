using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dream.AI
{
    // ==================================================================================
    
    public class RequestBody
    {
        [JsonPropertyName("model")] 
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<RequestMessage> Messages { get; set; }

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("stop")]
        public string Stop { get; set; } // 修改为 string 类型

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("top_p")]
        public double TopP { get; set; }

        [JsonPropertyName("top_k")] 
        public int TopK { get; set; }

        [JsonPropertyName("frequency_penalty")]
        public double FrequencyPenalty { get; set; }

        [JsonPropertyName("n")] 
        public int N { get; set; }

        [JsonPropertyName("response_format")] 
        public RequestFormat ResponseFormat { get; set; }

        [JsonPropertyName("tools")] 
        public List<RequestTool> Tools { get; set; }

        public string BuildString()
        {
            return JsonSerializer.Serialize(this);
        }

        public StringContent BuildStringContent()
        {
            return new StringContent(BuildString(), Encoding.UTF8, "application/json");
        }
    }

    public class RequestMessage
    {
        [JsonPropertyName("role")] 
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class RequestFormat
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class RequestTool
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("function")]
        public RequestFunction RequestFunction { get; set; }
    }

    public class RequestFunction
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parameters")]
        public object Parameters { get; set; }

        [JsonPropertyName("strict")]
        public bool Strict { get; set; }
    }
    
    // ==================================================================================

    public class Response
    {
        [JsonPropertyName("id")] 
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")] 
        public long Created { get; set; }

        [JsonPropertyName("model")] 
        public string Model { get; set; }

        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }

        [JsonPropertyName("usage")] 
        public Usage Usage { get; set; }

        [JsonPropertyName("system_fingerprint")]
        public string SystemFingerprint { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("index")] 
        public int Index { get; set; }

        [JsonPropertyName("message")] 
        public Message Message { get; set; }

        [JsonPropertyName("finish_reason")] 
        public string FinishReason { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("role")] 
        public string Role { get; set; }

        [JsonPropertyName("content")] 
        public string Content { get; set; }

        [JsonPropertyName("reasoning_content")]
        public string ReasoningContent { get; set; }
    }

    public class Usage
    {
        [JsonPropertyName("prompt_tokens")] 
        public int PromptTokens { get; set; }

        [JsonPropertyName("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonPropertyName("total_tokens")] 
        public int TotalTokens { get; set; }

        [JsonPropertyName("completion_tokens_details")]
        public CompletionTokensDetails CompletionTokensDetails { get; set; }
    }

    public class CompletionTokensDetails
    {
        [JsonPropertyName("reasoning_tokens")] public int ReasoningTokens { get; set; }
    }

    // ==================================================================================

    public static class Tools
    {
        public static string GetContentFromJsonString(string jsonResponse)
        {
            try
            {
                // 反序列化 JSON
                var responseObj = JsonSerializer.Deserialize<Response>(jsonResponse);

                // 检查 Choices 是否为 null，并访问其内容
                if (responseObj?.Choices != null && responseObj.Choices.Count > 0)
                {
                    var content = responseObj.Choices[0]?.Message?.Content;

                    // 检查是否包含 <tool_call>
                    if (content == "<tool_call>")
                    {
                        return "This response requires the execution of a tool call, please further process as required.";
                    }
                    else
                    {
                        return content ?? "No content returned";
                    }
                }
                else
                {
                    return "No selection or content is empty";
                }
            }
            catch (Exception ex)
            {
                return $"An error has occurred: {ex.Message}";
            }
        }
    }
}
