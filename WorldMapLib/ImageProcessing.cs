using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.AI;
using ElBruno.LocalLLMs;

namespace WorldMapLib
{
    public static class ImageProcessing
    {

        private const string ImageDescriptionPrompt = "Describe the objects visible in this image and pass a json formatted array";

        private static async Task<IChatClient> GetModel()
        {
            LocalLLMsOptions options = new()
            {
                Model = KnownModels.Gemma4E2BIT
            };
            return await LocalChatClient.CreateAsync(options);
        }

        internal static async Task<List<WorldObject>> GetObjectsFromImageAsync(string imagePath)
        {
            using var chatClient = await GetModel();

            byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

            var jsonSchemaOptions = new ChatResponseFormatJson(
                AIJsonUtilities.CreateJsonSchema(
                    type: typeof(List<WorldObject>),
                    description: "A structured list of world objects."
                )
            );
            var options = new ChatOptions
            {
                ResponseFormat = jsonSchemaOptions
            };

            var messages = new List<ChatMessage>
            {
                new ChatMessage (ChatRole.System, [
                    new TextContent(ImageDescriptionPrompt),
                    new DataContent(imageBytes, "image/jpeg")
                ])
            };

            ChatResponse response = await chatClient.GetResponseAsync(messages, options);
            return JsonSerializer.Deserialize<List<WorldObject>>(response.Text) ?? new List<WorldObject>();
        }



    }
}
