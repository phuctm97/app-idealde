#region Using Namespace

using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

#endregion

namespace Idealde.Framework.Services
{
    public class ResourceManager : IResourceManager
    {
        public Stream GetStream(string relativeUri, string assemblyName)
        {
            try
            {
                var resource = Application.GetResourceStream(new Uri($"{assemblyName};component/{relativeUri}",
                                   UriKind.Relative))
                               ?? Application.GetResourceStream(new Uri(relativeUri, UriKind.Relative));

                return resource?.Stream;
            }
            catch
            {
                return null;
            }
        }

        public BitmapImage GetBitmap(string relativeUri, string assemblyName)
        {
            var s = GetStream(relativeUri, assemblyName);
            if (s == null) return null;

            using (s)
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = s;
                bmp.EndInit();
                bmp.Freeze();
                return bmp;
            }
        }

        public BitmapImage GetBitmap(string relativeUri)
        {
            return GetBitmap(relativeUri, Assembly.GetExecutingAssembly().GetAssemblyName());
        }
    }
}