#region Using Namespace

using System.IO;
using System.Windows.Media.Imaging;

#endregion

namespace Idealde.Framework.Services
{
    public interface IResourceManager
    {
        Stream GetStream(string relativeUri, string assemblyName);

        BitmapImage GetBitmap(string relativeUri, string assemblyName);

        BitmapImage GetBitmap(string relativeUri);
    }
}