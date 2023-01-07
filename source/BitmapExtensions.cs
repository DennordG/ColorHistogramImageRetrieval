using System.Drawing;

namespace ColorHistogramImageRetrieval;

internal static class BitmapExtensions
{
    public static IReadOnlyDictionary<(int R, int G, int B), int> GetColorHistogram(this Bitmap bitmap, int binsCount)
    {
        if (binsCount > 255)
        {
            throw new ArgumentException("binsCount cannot be greater than 255", nameof(binsCount));
        }

        var dictionary = new Dictionary<(int R, int G, int B), int>();

        var splitBy = 255 / binsCount;

        for (var i = 0; i < bitmap.Width; i++)
        {
            for (var j = 0; j < bitmap.Height; j++)
            {
                var pixel = bitmap.GetPixel(i, j);
                if (pixel.A == 0)
                {
                    continue;
                }

                var color = (pixel.R / splitBy, pixel.G / splitBy, pixel.B / splitBy);

                if (!dictionary.ContainsKey(color))
                {
                    dictionary[color] = 0;
                }

                dictionary[color]++;
            }
        }

        return dictionary;
    }
}
