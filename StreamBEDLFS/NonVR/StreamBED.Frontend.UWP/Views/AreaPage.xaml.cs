using StreamBED.Backend.Helper;
using StreamBED.Backend.Models.ProtocolModels;
using StreamBED.Frontend.UWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace StreamBED.Frontend.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AreaPage : Page
    {
        internal static List<AreaDataModel> AreaList = new List<AreaDataModel>();

        private bool AllAreasCompleted = false;

        public AreaPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (AreaList.Count == 0)
            {
                var buffer = await Convert(new BitmapImage(
                                        new Uri("ms-appx:///Assets/Logo/image.png")
                                    ));

                ColorScheme scheme = new ColorScheme();
                Random r = new Random();

                List<ImageWithMetadata> imageList = new List<ImageWithMetadata>();

                for (int i = 0; i < 2; i++)
                {
                    var image = new ImageWithMetadata(buffer);

                    for (int j = 0; j < r.Next(0, 4); j++)
                    {
                        var k = EpifaunalSubstrateModel.GetKeywords()[r.Next(0, 4)];

                        if (!image.Keywords.Contains(k))
                        {
                            image.AddKeyword(k);
                        }
                    }

                    for (int j = 0; j < r.Next(0, 2); j++)
                    {
                        var k = BankStabilityModel.GetKeywords()[r.Next(0, 5)];

                        if (!image.Keywords.Contains(k))
                        {
                            image.AddKeyword(k);
                        }
                    }

                    image.Location = "Area " + r.Next(1, 7);

                    imageList.Add(image);
                }

                List<ImageDataModel> imageModelList = new List<ImageDataModel>();

                foreach (ImageWithMetadata image in imageList)
                {
                    imageModelList.Add(new ImageDataModel(image));
                }

                for (int i = 1; i <= 7; i++)
                {
                    var area = new AreaDataModel("Area " + i, scheme.GetColor());

                    foreach (ImageDataModel image in imageModelList)
                    {
                        if (image.Image.Location.Equals(area.Name))
                        {
                            area.ImageList.Add(image);
                        }
                    }

                    if (area.ImageList.Count != 0)
                    {
                        listViewRoot.Items.Add(area);
                        AreaList.Add(area);
                    }
                }
            }
            else
            {
                foreach (AreaDataModel area in AreaList)
                {
                    listViewRoot.Items.Add(area);
                }
            }

            foreach (AreaDataModel area in AreaList)
            {
                if (area.IsCompleted)
                {
                    AllAreasCompleted = true;
                }
                else
                {
                    AllAreasCompleted = false;
                    break;
                }
            }

            if (AllAreasCompleted)
            {
                nextButton.Visibility = Visibility.Visible;
            }

            progressRing.IsActive = false;
        }

        private async Task<byte[]> Convert(BitmapImage image)
        {
            byte[] buffer;

            using (IRandomAccessStreamWithContentType streamWithContent = await RandomAccessStreamReference.CreateFromUri(image.UriSource).OpenReadAsync())
            {
                buffer = new byte[streamWithContent.Size];
                await streamWithContent.ReadAsync(buffer.AsBuffer(), (uint)streamWithContent.Size, InputStreamOptions.None);
            }

            return buffer;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            AreaDataModel model = (sender as Button).DataContext as AreaDataModel;

            this.Frame.Navigate(typeof(FeaturePage), model);
        }

        private void NextButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FeatureEvalPage));
        }
    }
}
