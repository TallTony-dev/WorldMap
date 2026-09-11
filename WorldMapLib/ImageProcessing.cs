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

        internal static async Task<List<WorldObject>> GetObjectsFromImageAsync(byte[] jpegBytes)
        {
            using var chatClient = await GetModel();

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
                    new DataContent(jpegBytes, "image/jpeg")
                ])
            };

            ChatResponse response = await chatClient.GetResponseAsync(messages, options);

            Console.WriteLine($"Objects found in image: {response}");

            return JsonSerializer.Deserialize<List<WorldObject>>(response.Text) ?? new List<WorldObject>();
        }

        internal static async Task<List<WorldObject>> GetObjectsFromImageAsync(string jpegImagePath)
        {
            byte[] jpegBytes = await File.ReadAllBytesAsync(jpegImagePath);
            return await GetObjectsFromImageAsync (jpegBytes);
        }



    }
}
