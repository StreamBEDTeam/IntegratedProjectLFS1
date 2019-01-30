using StreamBED.Backend.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace StreamBED.Frontend.UWP.Models
{
    public class ImageDataModel
    {
        public ImageWithMetadata Image { get; }

        public ImageSource ImageSource { get; }

        public bool isComplete { get; set; }

        public ImageDataModel(ImageWithMetadata Image)
        {
            this.Image = Image;
            this.ImageSource = GetImageSource().Result;
            this.isComplete = false;
        }

        public async Task<BitmapImage> GetImageSource()
        {
            if (Image.Data != null)
            {
                BitmapImage bitmapImage = new BitmapImage();

                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(Image.Data.AsBuffer());
                    stream.Seek(0);
                    bitmapImage.SetSource(stream);
                }

                return bitmapImage;
            }

            return null;
        }
    }
}
