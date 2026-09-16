using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using System.Text;
using System.Text.Json;
using WorldMapLib;
using DriverLib;

namespace LLMLib
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

        private static string AreaDescriptionPrompt(string currentAreaName) => @$"Internally describe the area that the point from which the given image(s) were taken from with a name and description.
        Evaluate if that area is the name as the area described by the name: {currentAreaName}. And output either true if the area is different, or false if it is semantically equivalent.";

        private static string NewAreaPrompt() => $@"";

        private static IChatCompletionService GetLightImageModel()
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

        public static async Task<string> AskPromptToModelAsync(IChatCompletionService model, string prompt, ImageContent[]? images = null, OllamaPromptExecutionSettings? settings = null)
        {
            Console.WriteLine("Asking model a prompt");
            var history = new ChatHistory();

            var multiPartMessage = new ChatMessageContentItemCollection { new Microsoft.SemanticKernel.TextContent(prompt) };
            if (images != null)
            {
                foreach (var image in images)
                {
                    multiPartMessage.Add(image);
                }
            }
            history.AddMessage(AuthorRole.User, multiPartMessage);

            settings = settings == null ? new OllamaPromptExecutionSettings() : settings;
            settings.Think = true;

            var responseStream = model.GetStreamingChatMessageContentsAsync(history, settings);
            var responseText = new StringBuilder();

            await foreach (var chunk in responseStream)
            {
                Console.Write(chunk);
                responseText.Append(chunk);
            }

            return responseText.ToString();
        }

        public static async Task<string> AskPromptToModelAsync(IChatCompletionService model, string prompt, ImageContent? image = null, OllamaPromptExecutionSettings? settings = null)
        {
            return await AskPromptToModelAsync(model, prompt, image == null ? null : new[] { image }, settings);
        }


        internal static async Task<List<WorldObject>> GetWorldObjectsFromImageAsync(byte[] imageData, string imageType)
        {
            string text = await AskPromptToModelAsync(GetLightImageModel(), ImageDescriptionPrompt, new ImageContent(imageData, $"image/{imageType}"),
                new OllamaPromptExecutionSettings() { ExtensionData = new Dictionary<string, object> { { "response_format", "json_object" } } });
            
            return JsonSerializer.Deserialize<WorldObject[]>(text)?.ToList() ?? new List<WorldObject>();
        }

        internal static async Task<List<WorldObject>> GetWorldObjectsFromImageAsync(string imagePath, string imageType)
        {
            byte[] data = await File.ReadAllBytesAsync(imagePath);
            return await GetWorldObjectsFromImageAsync (data, imageType);
        }


        public static async Task AddApplicableAreasFromImageAsync(Image[] images, Area currentArea)
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
