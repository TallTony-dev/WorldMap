using SeeShark;
using SeeShark.Decode;
using SeeShark.Device;
using SeeShark.FFmpeg;
using System.IO;
using System.Text.Json;
using WorldMapLib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


internal class Program
{

    static bool isProcessing = false;
    static int framesSinceTaken = 0;
    static int frameCount = 0;

    static WorldGraph graph;

    public static void Main()
    {
        graph = new();

        using var manager = new CameraManager();
        using var camera = manager.GetDevice();
        camera.OnFrame += frameEventHandler;

        camera.StartCapture();

        string objectsJson;
        while ((objectsJson = Console.ReadLine()!) != "exit")
        {

        }

        camera.StopCapture();

    }

    
    private static void frameEventHandler(object? sender, FrameEventArgs e)
    {
        if (framesSinceTaken > 60 && !isProcessing)
        {
            int frameNum = frameCount;
            if (e.Status != DecodeStatus.NewFrame)
                return;

            Frame frame = e.Frame;
            
            var image = Image.LoadPixelData<Bgr24>(frame.RawData, frame.Width, frame.Height);
            using var jpegDataStream = new MemoryStream();
            image.SaveAsJpeg(jpegDataStream);

            graph.GetBaseArea().ConglomerateImage(jpegDataStream.ToArray());
            Console.WriteLine($"Processed frame {frameNum}");
        }
        framesSinceTaken++;
    }
}