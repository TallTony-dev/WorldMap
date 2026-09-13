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
    public static class LLMProcessing
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
        Do NOT include data about location in your responses.
        Do NOT repeat objects, and only include objects you can guarantee are correct.";

        private const string AreaDescriptionPrompt = @"";

        private static IChatCompletionService GetModel()
        {
            Console.WriteLine("Getting model for image recognition");
            string modelId = "gemma4:e2b";
            string endpoint = "http://localhost:11434";

            var builder = Kernel.CreateBuilder();
            builder.AddOllamaChatCompletion(
                modelId: modelId,
                endpoint: new Uri(endpoint)
            );

            Kernel kernel = builder.Build();

            return kernel.GetRequiredService<IChatCompletionService>();
        }

        internal static async Task<List<WorldObject>> GetObjectsFromImageAsync(byte[] data, string imageType)
        {
            var chatClient = GetModel();
            var schema = AIJsonUtilities.CreateJsonSchema(
                type: typeof(List<WorldObject>),
                description: "A structured list of world objects."
            );

            var history = new ChatHistory();

            var multiPartMessage = new ChatMessageContentItemCollection
            {
                new Microsoft.SemanticKernel.TextContent(ImageDescriptionPrompt),
                new ImageContent(data, $"image/{imageType}")
            };

            OllamaPromptExecutionSettings settings = new() {
                ExtensionData = new Dictionary<string, object>
                {
                    { "format", schema },
                    { "think", true }
                },
            };

            history.AddMessage(AuthorRole.User, multiPartMessage);
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


        internal static async Task AddApplicableAreasFromImageAsync(byte[] data, string imageType, Area currentArea) 
        { 
            throw new NotImplementedException();
        }


        internal static async Task CleanupObjectsInAreaAsync(Area targetArea) 
        {
            //string objects = targetArea.GetObjectsAsString();
            throw new NotImplementedException();
        }

    }
}
