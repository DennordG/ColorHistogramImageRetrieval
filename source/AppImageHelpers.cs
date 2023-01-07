namespace ColorHistogramImageRetrieval;

internal static class AppImageHelpers
{
    internal static double GetColorHistogramMatchDistance(AppImage databaseImage, AppImage queryImage)
    {
        var totalDistance = 0.0;

        var keys = databaseImage.ColorHistogram.Keys.Union(queryImage.ColorHistogram.Keys);

        foreach (var key in keys)
        {
            if (!databaseImage.ColorHistogram.TryGetValue(key, out var databaseFrequency))
            {
                databaseFrequency = 0;
            }

            if (!queryImage.ColorHistogram.TryGetValue(key, out var queryFrequency))
            {
                queryFrequency = 0;
            }

            var distance = Math.Pow(databaseFrequency - queryFrequency, 2);

            totalDistance += distance;
        }

        return Math.Sqrt(totalDistance);
    }
}