namespace ColorHistogramImageRetrieval;

internal class AppImageMatch
{
    public string ImagePath { get; set; }
    public double Score { get; set; }

    public AppImageMatch(string imagePath, double score)
    {
        ImagePath = imagePath;
        Score = score;
    }

    public override string ToString()
    {
        return $"{Path.GetFileName(ImagePath)}: {Score}";
    }
}
