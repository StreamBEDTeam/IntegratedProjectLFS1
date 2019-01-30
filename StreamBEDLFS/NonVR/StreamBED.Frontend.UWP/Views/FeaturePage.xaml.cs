using StreamBED.Frontend.UWP.Models;
using StreamBED.Backend.Models.ProtocolModels;
using StreamBED.Backend.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Text;

namespace StreamBED.Frontend.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeaturePage : Page
    {
        private AreaDataModel Area;

        private int Count = 0;

        private int NumberOfKeywords = 0;

        private FontIcon CompletionIcon;

        private Dictionary<FeatureDataModel, ImageDataModel> imageDict = new Dictionary<FeatureDataModel, ImageDataModel>();

        internal static List<ImageDataModel> SelectedItems = new List<ImageDataModel>();

        internal static Dictionary<Keyword, List<ImageDataModel>> biasDict = new Dictionary<Keyword, List<ImageDataModel>>();

        public FeaturePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (SelectedItems.Count != 0)
            {
                messagePopup.Visibility = Visibility.Collapsed;
            }

            Area = e.Parameter as AreaDataModel;

            foreach (Keyword keyword in EpifaunalSubstrateModel.GetKeywords())
            {
                var model = FeatureDataModel.GetFeatureDataModel(keyword, Area.ImageList);

                if (model != null)
                {
                    listViewRoot.Items.Add(model);

                    NumberOfKeywords++;
                }
                else
                {
                    ImageDataModel image = new ImageDataModel(new ImageWithMetadata(null));
                    image.Image.ChangeEpifaunalSubstrateScore(0);
                    image.Image.AddKeyword(keyword);
                    image.isComplete = true;
                    image.Image.Location = Area.Name;

                    if (keyword.Equals(EpifaunalSubstrateModel.Keywords.SnagsLogs) || keyword.Equals(EpifaunalSubstrateModel.Keywords.UnderwaterVegetation) || keyword.Equals(EpifaunalSubstrateModel.Keywords.UndercutBanks))
                    {
                        if (!biasDict.ContainsKey(keyword))
                        {
                            biasDict.Add(keyword, new List<ImageDataModel>());
                        }

                        biasDict.GetValueOrDefault(keyword).Add(image);
                    }
                }
            }

            foreach (Keyword keyword in BankStabilityModel.GetKeywords())
            {
                var model = FeatureDataModel.GetFeatureDataModel(keyword, Area.ImageList);

                if (model != null)
                {
                    listViewRoot.Items.Add(model);

                    NumberOfKeywords++;
                }
                else
                {
                    ImageDataModel image = new ImageDataModel(new ImageWithMetadata(null));
                    image.Image.ChangeBankStabilityScore(10);
                    image.Image.AddKeyword(keyword);
                    image.isComplete = true;
                    image.Image.Location = Area.Name;

                    if (keyword.Equals(BankStabilityModel.Keywords.ExposedTreeRoots) || keyword.Equals(BankStabilityModel.Keywords.ErodedAreas) || keyword.Equals(BankStabilityModel.Keywords.BankFailure))
                    {
                        if (!biasDict.ContainsKey(keyword))
                        {
                            biasDict.Add(keyword, new List<ImageDataModel>());
                        }

                        biasDict.GetValueOrDefault(keyword).Add(image);
                    }
                }
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        { 
            if (Count == 0)
            {
                StackPanel stack = sender as StackPanel;

                Border border = new Border()
                {
                    BorderBrush = new SolidColorBrush(ColorScheme.ColorFromHex("#D9D9D9")),
                    CornerRadius = new CornerRadius(8, 8, 8, 8),
                    BorderThickness = new Thickness(8, 8, 8, 8),
                    Margin = new Thickness(150, 12.5, 150, 12.5),
                    Width = 1200
                };

                Grid titleGrid = new Grid()
                {
                    Height = 75,
                    Background = Area.ItemColorBrush
                };

                TextBlock title = new TextBlock()
                {
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = Area.Name.ToUpper()
                };

                FontIcon icon = new FontIcon()
                {
                    FontFamily = new FontFamily("Segoe MDL2 Assets"),
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 45,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(15),
                    Height = 75,
                    Width = 75,
                    Visibility = Visibility.Collapsed,
                    Glyph = "\uE73E"
                };

                CompletionIcon = icon;

                titleGrid.Children.Add(title);
                titleGrid.Children.Add(icon);
                border.Child = titleGrid;

                stack.Children.Insert(0, border);

                Count--;
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ImageDataModel image in imageDict.Values)
            {
                SelectedItems.Add(image);
            }

            DataContext = null;
            listViewRoot.DataContext = null;

            this.Frame.Navigate(typeof(AreaPage));
        }

        private void ImageGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;

            if (gridView.DataContext != null)
            {
                if (gridView.SelectedItem as ImageDataModel != null)
                {
                    if (!gridView.SelectedItem.Equals(imageDict.GetValueOrDefault(gridView.DataContext as FeatureDataModel) as ImageDataModel))
                    {
                        if (imageDict.ContainsKey(gridView.DataContext as FeatureDataModel))
                        {
                            imageDict.Remove(gridView.DataContext as FeatureDataModel);
                        }

                        imageDict.Add(gridView.DataContext as FeatureDataModel, gridView.SelectedItem as ImageDataModel);
                    }
                }

                if (imageDict.Count == NumberOfKeywords)
                {
                    CompletionIcon.Visibility = Visibility.Visible;
                    nextButton.Visibility = Visibility.Visible;

                    Area.IsCompleted = true;
                }
                else
                {
                    CompletionIcon.Visibility = Visibility.Collapsed;
                    nextButton.Visibility = Visibility.Collapsed;

                    Area.IsCompleted = false;
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var list = AreaPage.AreaList;
            int currIndex = list.IndexOf(Area);

            if (currIndex < list.Count - 1)
            {
                foreach (ImageDataModel image in imageDict.Values)
                {
                    SelectedItems.Add(image);
                }

                list[currIndex].IsCompleted = true;

                this.Frame.Navigate(typeof(FeaturePage), AreaPage.AreaList.ElementAt(AreaPage.AreaList.IndexOf(Area) + 1));
            }
            else
            {
                bool flag = false;

                foreach (AreaDataModel model in list)
                {
                    if (model.IsCompleted)
                    {
                        flag = true;
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (flag)
                {
                    foreach (ImageDataModel image in imageDict.Values)
                    {
                        SelectedItems.Add(image);
                    }

                    this.Frame.Navigate(typeof(FeatureEvalPage));
                }
                else
                {
                    this.Frame.Navigate(typeof(AreaPage));
                }
            }
        }

        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {
            messagePopup.Visibility = Visibility.Collapsed;
        }
    }
}
