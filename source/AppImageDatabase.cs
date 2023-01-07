using System.Collections;

namespace ColorHistogramImageRetrieval;

internal class AppImageDatabase : IDisposable, IEnumerable<AppImage>
{
    #region Private members
    private readonly IReadOnlyCollection<AppImage> _images;
    #endregion


    #region ctor
    public AppImageDatabase(IReadOnlyCollection<AppImage> images)
    {
        _images = images;
    }
    #endregion


    #region Public methods
    public IReadOnlyCollection<AppImageMatch> GetMatches(AppImage queryImage)
    {
        var matches = new List<AppImageMatch>();

        var maxColorHistogramDistance = 0.0;

        foreach (var databaseImage in _images)
        {
            if (databaseImage.ImagePath == queryImage.ImagePath)
            {
                continue;
            }

            var colorHistogramDistance = AppImageHelpers.GetColorHistogramMatchDistance(databaseImage, queryImage);

            maxColorHistogramDistance = Math.Max(colorHistogramDistance, maxColorHistogramDistance);

            var match = new AppImageMatch(databaseImage.ImagePath, colorHistogramDistance);
            matches.Add(match);
        }

        foreach (var match in matches)
        {
            match.Score = 1.0 - (match.Score / maxColorHistogramDistance);
        }

        return matches;
    }
    #endregion


    #region IDisposable
    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                foreach (var image in _images)
                {
                    image.Dispose();
                }
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion


    #region IEnumerable
    public IEnumerator<AppImage> GetEnumerator()
    {
        return _images.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_images).GetEnumerator();
    }
    #endregion
}
