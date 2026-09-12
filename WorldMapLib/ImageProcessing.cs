using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;

namespace WorldMapLib
{
    public static class ImageProcessing
    {

        private const string ImageDescriptionPrompt = @"Describe the objects visible in this image and output a JSON array describing them. 
        Object volatility (movementLikelihood) is how likely the object is to move or be destroyed. (e.g., 'Low', 'Medium', 'High').

        You MUST adhere exactly to this JSON schema:
        [
        {
            ""Name"": ""string"",
            ""Description"": ""string"",

            ""MovementLikelihood"": ""Low/Medium/High"" 
        }
        ]

        Return strictly valid JSON only, without markdown formatting or extra text.
        Do NOT include data about location in your responses.";

        private static IChatCompletionService GetModel()
        {
            // string modelPath = Path.Combine(AppContext.BaseDirectory, "models", "gemma-4-e2b-it-onnx");

            string modelId = "gemma4:e2b";
            string endpoint = "http://localhost:11434";

            var builder = Kernel.CreateBuilder();
            builder.AddOllamaChatCompletion(
                modelId: modelId,
                endpoint: new Uri(endpoint)
            );

            Kernel kernel = builder.Build();

            return kernel.GetRequiredService<IChatCompletionService>();

            // LocalLLMsOptions options = new()
            // {
            //     Model = KnownModels.Gemma4E2BIT with { ModelType = OnnxModelType.VisionGenAI },
            //     ModelPath = modelPath,
            //     EnsureModelDownloaded = false,
            //     // EnsureModelDownloaded = true
            // };
            
            // return await LocalVisionChatClient.CreateAsync(options);
        }

        internal static async Task<List<WorldObject>> GetObjectsFromImageAsync(byte[] data, string imageType)
        {
            var chatClient = GetModel();
            var schema = AIJsonUtilities.CreateJsonSchema(
                type: typeof(List<WorldObject>),
                description: "A structured list of world objects."
            );
            //     var jsonSchemaOptions = new ChatResponseFormatJson(
            //        
            //    );
            // var options = new ChatOptions
            // {
            //     // ResponseFormat = jsonSchemaOptions,
            //     // StopSequences = new List<string> { "<turn|>", "<|turn>" },
            //     Temperature = 0.3f,
                
                
            // };

            // var messages = new List<ChatMessage>
            // {
            //     new ChatMessage(ChatRole.User,
            //     [
            //         new TextContent($"{ImageDescriptionPrompt}"),
            //         // new DataContent(data, $"image/{imageType}")
            //     ])
            // };

            var history = new ChatHistory();

            var multiPartMessage = new ChatMessageContentItemCollection
            {
                new Microsoft.SemanticKernel.TextContent(ImageDescriptionPrompt),
                new ImageContent(data, $"image/{imageType}")
            };

            OllamaPromptExecutionSettings settings = new() {
                ExtensionData = new Dictionary<string, object>
                {
                    { "format", schema }
                },
            };

            history.AddMessage(AuthorRole.User, multiPartMessage);

            Console.WriteLine($"Schema: {schema}");
            Console.WriteLine($"Getting objects from image");
            
            var responseStream = chatClient.GetStreamingChatMessageContentsAsync(history, settings);
            var responseText = new StringBuilder();

            await foreach (var chunk in responseStream)
            {
                Console.Write(chunk);
                responseText.Append(chunk);
            }

            string text = responseText.ToString();
            
            var objects = JsonSerializer.Deserialize<WorldObject[]>(text)?.ToList() ?? new List<WorldObject>();
            //Console.WriteLine($"Objects found in image: {string.Join(',', objects.Select(t => t.ToString(true)))}");

            return objects;
        }

        internal static async Task<List<WorldObject>> GetObjectsFromImageAsync(string imagePath, string imageType)
        {
            byte[] data = await File.ReadAllBytesAsync(imagePath);
            return await GetObjectsFromImageAsync (data, imageType);
        }



    }
}
