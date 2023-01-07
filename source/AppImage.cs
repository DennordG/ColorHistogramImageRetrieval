using System.Drawing;

namespace ColorHistogramImageRetrieval;

internal record AppImage(Bitmap Image, string ImagePath) : IDisposable
{
    #region Properties
    private readonly Lazy<IReadOnlyDictionary<(int R, int G, int B), int>> _colorHistogram = new(() => Image.GetColorHistogram(binsCount: 3));

    public IReadOnlyDictionary<(int R, int G, int B), int> ColorHistogram => _colorHistogram.Value;
    #endregion


    #region IDisposable
    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Image.Dispose();
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
}
