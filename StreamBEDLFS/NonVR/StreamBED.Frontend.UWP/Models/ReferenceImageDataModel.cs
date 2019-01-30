using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace StreamBED.Frontend.UWP.Models
{
    public class ReferenceImageDataModel
    {
        public ImageSource Image;

        public string Content;

        public ReferenceImageDataModel(string Image, string Content)
        {
            this.Image = new BitmapImage(new Uri("ms-appx://" + Image));
            this.Content = Content;
        }
    }
}
