using StreamBED.Backend.Helper;
using StreamBED.Backend.Models;
using StreamBED.Backend.Models.ProtocolModels;
using StreamBED.Frontend.UWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StreamBED.Frontend.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FinalAssessmentPage : Page
    {
        private ProtocolModel SelectedModel;

        private XElement icons;

        private bool epifaunalLoaded = false, bankLoaded = false;

        private EpifaunalSubstrateModel EpifaunalSubstrateModel = new EpifaunalSubstrateModel();

        private BankStabilityModel BankStabilityModel = new BankStabilityModel();

        public FinalAssessmentPage()
        {
            this.InitializeComponent();
        }

        private void BankStability_Tapped(object sender, TappedRoutedEventArgs e)
        {
            protocolTitle.Text = "BANK STABILITY";
            SelectedModel = FeatureEvalPage.BankStabilityModel;

            if (!bankLoaded)
            {
                foreach (KeyValuePair<Keyword, FeatureDataModel> elem in FeatureEvalPage.bankStabilityFeatures)
                {
                    foreach (ImageDataModel image in elem.Value.ImageList)
                    {
                        SolidColorBrush brush = AreaPage.AreaList.Where(i => i.Name.Equals(image.Image.Location)).First().ItemColorBrush;

                        Border border = new Border()
                        {
                            Width = 50,
                            Height = 50,
                            BorderBrush = brush,
                            BorderThickness = new Thickness(4),
                            Margin = new Thickness(0, 2.5, 0, 2.5)
                        };

                        Image icon = new Image()
                        {
                            Height = 45,
                            Width = 45,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Source = GetFeatureIcon(elem.Key)
                        };

                        border.Child = icon;

                        if (image.Image.BankStabilityScore % 2 == 0)
                        {
                            foreach (StackPanel stack in bankStackTop.Children)
                            {
                                if (stack.Tag.Equals(image.Image.BankStabilityScore.ToString()))
                                {
                                    stack.Children.Add(border);
                                }
                            }
                        }
                        else
                        {
                            foreach (StackPanel stack in bankStackBottom.Children)
                            {
                                if (stack.Tag.Equals(image.Image.BankStabilityScore.ToString()))
                                {
                                    stack.Children.Add(border);
                                }
                            }
                        }
                    }
                }

                foreach (KeyValuePair<Keyword, List<ImageDataModel>> elem in FeaturePage.biasDict)
                {
                    if (BankStabilityModel.GetKeywords().Contains(elem.Key))
                    {
                        foreach (ImageDataModel image in elem.Value)
                        {
                            SolidColorBrush brush = AreaPage.AreaList.Where(i => i.Name.Equals(image.Image.Location)).First().ItemColorBrush;

                            Border border = new Border()
                            {
                                Width = 50,
                                Height = 50,
                                BorderBrush = brush,
                                BorderThickness = new Thickness(4),
                                Margin = new Thickness(0, 2.5, 0, 2.5)
                            };

                            Image icon = new Image()
                            {
                                Height = 45,
                                Width = 45,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Source = GetFeatureIcon(elem.Key)
                            };

                            border.Child = icon;

                            if (image.Image.BankStabilityScore % 2 == 0)
                            {
                                foreach (StackPanel stack in bankStackTop.Children)
                                {
                                    if (stack.Tag.Equals(image.Image.BankStabilityScore.ToString()))
                                    {
                                        stack.Children.Add(border);
                                    }
                                }
                            }
                            else
                            {
                                foreach (StackPanel stack in bankStackBottom.Children)
                                {
                                    if (stack.Tag.Equals(image.Image.BankStabilityScore.ToString()))
                                    {
                                        stack.Children.Add(border);
                                    }
                                }
                            }
                        }
                    }
                }

                bankLoaded = true;
            }

            layoutPivot.SelectedIndex = 4;
        }

        private void EpifaunalSubstrate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            protocolTitle.Text = "EPIFAUNAL SUBSTRATE";
            SelectedModel = FeatureEvalPage.EpifaunalSubstrateModel;

            if (!epifaunalLoaded)
            {
                foreach (KeyValuePair<Keyword, FeatureDataModel> elem in FeatureEvalPage.epifaunalSubstrateFeatures)
                {
                    foreach (ImageDataModel image in elem.Value.ImageList)
                    {
                        SolidColorBrush brush = AreaPage.AreaList.Where(i => i.Name.Equals(image.Image.Location)).First().ItemColorBrush;

                        Border border = new Border()
                        {
                            Width = 50,
                            Height = 50,
                            BorderBrush = brush,
                            BorderThickness = new Thickness(4),
                            Margin = new Thickness(0, 2.5, 0, 2.5)
                        };

                        Image icon = new Image()
                        {
                            Height = 45,
                            Width = 45,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Source = GetFeatureIcon(elem.Key)
                        };

                        border.Child = icon;

                        if (image.Image.EpifaunalSubstrateScore % 2 == 0)
                        {
                            foreach (StackPanel stack in epifaunalStackTop.Children)
                            {
                                if (stack.Tag.Equals(image.Image.EpifaunalSubstrateScore.ToString()))
                                {
                                    stack.Children.Add(border);
                                }
                            }
                        }
                        else
                        {
                            foreach (StackPanel stack in epifaunalStackBottom.Children)
                            {
                                if (stack.Tag.Equals(image.Image.EpifaunalSubstrateScore.ToString()))
                                {
                                    stack.Children.Add(border);
                                }
                            }
                        }
                    }
                }

                foreach (KeyValuePair<Keyword, List<ImageDataModel>> elem in FeaturePage.biasDict)
                {
                    if (EpifaunalSubstrateModel.GetKeywords().Contains(elem.Key))
                    {
                        foreach (ImageDataModel image in elem.Value)
                        {
                            SolidColorBrush brush = AreaPage.AreaList.Where(i => i.Name.Equals(image.Image.Location)).First().ItemColorBrush;

                            Border border = new Border()
                            {
                                Width = 50,
                                Height = 50,
                                BorderBrush = brush,
                                BorderThickness = new Thickness(4),
                                Margin = new Thickness(0, 2.5, 0, 2.5)
                            };

                            Image icon = new Image()
                            {
                                Height = 45,
                                Width = 45,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Source = GetFeatureIcon(elem.Key)
                            };

                            border.Child = icon;

                            if (image.Image.EpifaunalSubstrateScore % 2 == 0)
                            {
                                foreach (StackPanel stack in epifaunalStackTop.Children)
                                {
                                    if (stack.Tag.Equals(image.Image.EpifaunalSubstrateScore.ToString()))
                                    {
                                        stack.Children.Add(border);
                                    }
                                }
                            }
                            else
                            {
                                foreach (StackPanel stack in epifaunalStackBottom.Children)
                                {
                                    if (stack.Tag.Equals(image.Image.EpifaunalSubstrateScore.ToString()))
                                    {
                                        stack.Children.Add(border);
                                    }
                                }
                            }
                        }
                    }
                }

                epifaunalLoaded = true;
            }

            layoutPivot.SelectedIndex = 2;
        }

        private void LayoutPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = layoutPivot.SelectedIndex;

            if (index == 0)
            {
                navBar.Visibility = Visibility.Collapsed;
                protocolBlock.Visibility = Visibility.Collapsed;
            }
            else if (index == 1)
            {
                navBar.Visibility = Visibility.Visible;
                protocolBlock.Visibility = Visibility.Collapsed;
            }
            else if (index >= 2)
            {
                navBar.Visibility = Visibility.Visible;
                protocolBlock.Visibility = Visibility.Visible;
            }
        }

        private void EvalTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (layoutPivot.SelectedIndex > 1)
            {
                layoutPivot.SelectedIndex = 1;
            }
        }

        private void ProtocolTitle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectedModel is BankStabilityModel && layoutPivot.SelectedIndex > 4)
            {
                layoutPivot.SelectedIndex = 4;
            }
            else if (SelectedModel is EpifaunalSubstrateModel && layoutPivot.SelectedIndex > 2)
            {
                layoutPivot.SelectedIndex = 2;
            }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            using (Stream stream = await(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/FeatureIcons.xml"))).OpenStreamForReadAsync())
            {
                icons = XDocument.Load(stream).Root;
            }
            ++layoutPivot.SelectedIndex;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedModel is BankStabilityModel)
            {
                layoutPivot.SelectedIndex = 5;

                BankStabilityModel.Score = bankSlider.Value;
                progressBar.Value++;
            }
            else if (SelectedModel is EpifaunalSubstrateModel)
            {
                layoutPivot.SelectedIndex = 3;

                EpifaunalSubstrateModel.Score = epifaunalSlider.Value;
                progressBar.Value++;
            }
        }

        private void EpifaunalSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue == 0)
            {
                epifaunalSubmitButton.IsEnabled = false;
            }
            else
            {
                epifaunalSubmitButton.IsEnabled = true;
            }

            foreach (TextBlock block in epifaunalValueStack.Children)
            {
                if (block.Text.Equals(epifaunalSlider.Value.ToString()))
                {
                    block.Opacity = 1;
                }
                else
                {
                    if (!block.Text.Equals("20") && !block.Text.Equals("15") && !block.Text.Equals("10") && !block.Text.Equals("5") && !block.Text.Equals("0"))
                    {
                        if (block.Opacity == 1)
                        {
                            block.Opacity = 0;
                        }
                    }
                }
            }
        }

        private void NextProtocolButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BankSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue == 0)
            {
                bankSubmitButton.IsEnabled = false;
            }
            else
            {
                bankSubmitButton.IsEnabled = true;
            }

            foreach (TextBlock block in bankValueStack.Children)
            {
                if (block.Text.Equals(bankSlider.Value.ToString()))
                {
                    block.Opacity = 1;
                }
                else
                {
                    if (!block.Text.Equals("10") && !block.Text.Equals("6") && !block.Text.Equals("4") && !block.Text.Equals("0"))
                    {
                        if (block.Opacity == 1)
                        {
                            block.Opacity = 0;
                        }
                    }
                }
            }
        }

        private BitmapImage GetFeatureIcon(Keyword keyword)
        {
            return new BitmapImage(new Uri(icons.Elements().Where(i => i.Attribute("name").Value.Equals(keyword.FriendlyName)).First().Attribute("src").Value));
        }
    }
}
