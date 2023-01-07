using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;

namespace ColorHistogramImageRetrieval;

internal class Program
{
    static void Main(string[] args)
    {
        var config = ReadConfig();

        EnsureDirectories(config);

        PreprocessDatabaseImages(config);

        using var database = LoadDatabaseImages(config);

        // using var queryImage = LoadQueryImage(config);

        foreach (var queryImage in database)
        {
            Console.WriteLine(Path.GetFileName(queryImage.ImagePath));
            Console.WriteLine("------------------------------------------------------");

            var bestMatches = database.GetMatches(queryImage).OrderByDescending(m => m.Score).Take(5);

            foreach (var match in bestMatches)
            {
                Console.WriteLine(match);
            }

            Console.WriteLine("------------------------------------------------------");
        }
    }

    private static void EnsureDirectories(Config config)
    {
        Directory.CreateDirectory(config.PreprocessedDatabasePath);
        Directory.CreateDirectory(config.ImageDatabasePath);
    }

    private static void PreprocessDatabaseImages(Config config)
    {
        foreach (var imagePath in Directory.EnumerateFiles(config.ImageDatabasePath))
        {
            var outputPath = Path.Combine(config.PreprocessedDatabasePath, Path.GetFileName(imagePath));

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            using var outputFileStream = new FileStream(outputPath, FileMode.CreateNew);

            using var image = ReadAndPreprocessImage(imagePath);

            image.Save(outputFileStream, ImageFormat.Png);
        }
    }

    private static AppImageDatabase LoadDatabaseImages(Config config)
    {
        var images = new List<AppImage>();

        foreach (var imagePath in Directory.EnumerateFiles(config.PreprocessedDatabasePath))
        {
            var image = ReadImage(imagePath);

            images.Add(new AppImage(image, imagePath));
        }

        return new AppImageDatabase(images);
    }

    private static AppImage LoadQueryImage(Config config)
    {
        var queryImagePath = Path.Combine(config.PreprocessedDatabasePath, config.QueryImage);

        var image = ReadImage(queryImagePath);

        return new AppImage(image, queryImagePath);
    }

    private static Bitmap ReadImage(string path)
    {
        var image = Image.FromFile(path) ?? throw new InvalidOperationException($"Image is null (path: {path})");

        return (Bitmap)image;
    }

    private static Bitmap ReadAndPreprocessImage(string path)
    {
        using var image = ReadImage(path);

        var resizedImage = image.Resize(640, 320);

        return resizedImage;
    }

    private static Config ReadConfig()
    {
        using var stream = File.OpenRead("config.json");
        var config = JsonSerializer.Deserialize<Config>(stream);

        return config ?? throw new InvalidOperationException("Config is null");
    }
}