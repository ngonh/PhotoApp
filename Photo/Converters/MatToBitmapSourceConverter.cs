using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.IO;

namespace Photo.Converters
{
    public class MatToBitmapSourceConverter : IValueConverter
    {
        #region Method(s)
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Mat mat)
            {
                try
                {
                    Bitmap bitmap = mat.ToBitmap();
                    BitmapImage bitmapImage = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        stream.Position = 0;
                        bitmapImage.SetSource(stream.AsRandomAccessStream());
                    }
                    return bitmapImage;
                }
                catch (Exception ex)
                {
                }
            }
            return new BitmapImage();
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
