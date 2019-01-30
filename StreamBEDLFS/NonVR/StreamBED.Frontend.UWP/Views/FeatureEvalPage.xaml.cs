using StreamBED.Backend.Helper;
using StreamBED.Backend.Models;
using StreamBED.Backend.Models.ProtocolModels;
using StreamBED.Frontend.UWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.ApplicationModel;
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
    public sealed partial class FeatureEvalPage : Page
    {
        public static EpifaunalSubstrateModel EpifaunalSubstrateModel = new EpifaunalSubstrateModel();

        public static BankStabilityModel BankStabilityModel = new BankStabilityModel();

        internal static Dictionary<Keyword, FeatureDataModel> epifaunalSubstrateFeatures = new Dictionary<Keyword, FeatureDataModel>();

        internal static Dictionary<Keyword, FeatureDataModel> bankStabilityFeatures = new Dictionary<Keyword, FeatureDataModel>();

        internal static FeatureDataModel SelectedFeature;

        internal static ProtocolModel SelectedModel;

        internal static FeatureEvalPage Current = null;

        internal static List<XElement> EpifaunalReference;

        internal static List<XElement> BankReference;

        internal static List<XElement> SelectedFeatureReference;

        internal static XDocument referenceImages;

        public FeatureEvalPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        private void PivotRoot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivotRoot.SelectedIndex == 1)
            {
                nextButton.Content = "Get Started";
                nextButton.Width = 310;
            }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (pivotRoot.SelectedIndex < 1)
            {
                pivotRoot.SelectedIndex = ++pivotRoot.SelectedIndex;
            }
            else
            {
                ++layoutPivot.SelectedIndex;

                InitializeProtocol(BankStabilityModel.GetKeywords(), BankStabilityModel);
                InitializeProtocol(EpifaunalSubstrateModel.GetKeywords(), EpifaunalSubstrateModel);

                if (epifaunalSubstrateFeatures.Count == 0)
                {
                    epifaunalStack.Visibility = Visibility.Collapsed;
                    EpifaunalSubstrateModel.IsCompleted = true;
                }

                if (bankStabilityFeatures.Count == 0)
                {
                    bankStack.Visibility = Visibility.Collapsed;
                    BankStabilityModel.IsCompleted = true;
                }

                using (Stream stream = await (await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/ReferenceImages.xml"))).OpenStreamForReadAsync())
                {
                    referenceImages = XDocument.Load(stream);
                }

                EpifaunalReference = referenceImages.Root.Elements().ElementAt(0).Elements().ToList();

                BankReference = referenceImages.Root.Elements().ElementAt(1).Elements().ToList();
            }
        }

        private void BankStability_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ChangeToBankStability();

            ++layoutPivot.SelectedIndex;
        }

        public void ChangeToBankStability()
        {
            protocolTitle.Text = "BANK STABILITY";
            SelectedModel = BankStabilityModel;

            listViewRoot.Items.Clear();

            bool? flag = null;

            foreach (FeatureDataModel feature in bankStabilityFeatures.Values)
            {
                listViewRoot.Items.Add(feature);

                if (flag == null || !feature.IsComplete)
                {
                    flag = feature.IsComplete;
                }
            }

            if (flag != null && !flag.Value)
            {
                nextProtocolButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                nextProtocolButton.Visibility = Visibility.Visible;
            }
        }

        private void EpifaunalSubstrate_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ChangeToEpifaunalSubstrate();

            ++layoutPivot.SelectedIndex;
        }

        public void ChangeToEpifaunalSubstrate()
        {
            protocolTitle.Text = "EPIFAUNAL SUBSTRATE";
            SelectedModel = EpifaunalSubstrateModel;

            listViewRoot.Items.Clear();

            bool? flag = null;

            foreach (FeatureDataModel feature in epifaunalSubstrateFeatures.Values)
            {
                listViewRoot.Items.Add(feature);

                if (flag == null || !feature.IsComplete)
                {
                    flag = feature.IsComplete;
                }
            }

            if (flag != null && !flag.Value)
            {
                nextProtocolButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                nextProtocolButton.Visibility = Visibility.Visible;
            }
        }

        private void InitializeProtocol(Keyword[] keywords, ProtocolModel model) 
        {
            foreach (Keyword keyword in keywords)
            {
                foreach (ImageDataModel image in FeaturePage.SelectedItems)
                {
                    if (image.Image.Keywords.Contains(keyword))
                    {
                        if (model is EpifaunalSubstrateModel)
                        {
                            if (!epifaunalSubstrateFeatures.ContainsKey(keyword))
                            {
                                epifaunalSubstrateFeatures.Add(keyword, new FeatureDataModel(keyword));
                            }

                            if (!epifaunalSubstrateFeatures.GetValueOrDefault(keyword).ImageList.Where(i => i.Image.Equals(image.Image)).Any())
                            {
                                epifaunalSubstrateFeatures.GetValueOrDefault(keyword).ImageList.Add(new ImageDataModel(image.Image));
                            }
                        }
                        else if (model is BankStabilityModel)
                        {
                            if (!bankStabilityFeatures.ContainsKey(keyword))
                            {
                                bankStabilityFeatures.Add(keyword, new FeatureDataModel(keyword));
                            }

                            if (!bankStabilityFeatures.GetValueOrDefault(keyword).ImageList.Select(i => i.Image.Equals(image.Image)).Any())
                            {
                                bankStabilityFeatures.GetValueOrDefault(keyword).ImageList.Add(new ImageDataModel(image.Image));
                            }
                        }
                    }
                }
            }
        }

        private void LayoutPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = layoutPivot.SelectedIndex;

            if (index == 0)
            {
                navBar.Visibility = Visibility.Collapsed;
                protocolBlock.Visibility = Visibility.Collapsed;
                featureBlock.Visibility = Visibility.Collapsed;
                nextImageButton.Visibility = Visibility.Collapsed;
            }
            else if (index == 1)
            {
                navBar.Visibility = Visibility.Visible;
                protocolBlock.Visibility = Visibility.Collapsed;
                featureBlock.Visibility = Visibility.Collapsed;
                nextImageButton.Visibility = Visibility.Collapsed;
            }
            else if (index == 2)
            {
                navBar.Visibility = Visibility.Visible;
                protocolBlock.Visibility = Visibility.Visible;
                featureBlock.Visibility = Visibility.Collapsed;
                nextImageButton.Visibility = Visibility.Collapsed;
            }
            else if (index == 3)
            {
                navBar.Visibility = Visibility.Visible;
                protocolBlock.Visibility = Visibility.Visible;
                featureBlock.Visibility = Visibility.Visible;
                nextImageButton.Visibility = Visibility.Collapsed;
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
            if (layoutPivot.SelectedIndex > 2)
            {
                listViewRoot.Items.Clear();

                if (SelectedModel is EpifaunalSubstrateModel)
                {
                    foreach (FeatureDataModel feature in epifaunalSubstrateFeatures.Values)
                    {
                        listViewRoot.Items.Add(feature);
                    }
                }
                else if (SelectedModel is BankStabilityModel)
                {
                    foreach (FeatureDataModel feature in bankStabilityFeatures.Values)
                    {
                        listViewRoot.Items.Add(feature);
                    }
                }

                layoutPivot.SelectedIndex = 2;
            }
        }

        private void TemplateRoot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelectedFeature = (sender as StackPanel).DataContext as FeatureDataModel;

            if (!SelectedFeature.IsComplete)
            {
                featureTitle.Text = SelectedFeature.Keyword.Content.ToUpper();

                if (SelectedModel is EpifaunalSubstrateModel)
                {
                    SelectedFeatureReference = EpifaunalReference.Where(i => i.Attribute("name").Value.Equals(SelectedFeature.Keyword.FriendlyName)).First().Elements().ToList();

                    assessmentFrame.Navigate(typeof(EpifaunalAssessmentPage));
                }
                else if (SelectedModel is BankStabilityModel)
                {
                    SelectedFeatureReference = BankReference.Where(i => i.Attribute("name").Value.Equals(SelectedFeature.Keyword.FriendlyName)).First().Elements().ToList();

                    assessmentFrame.Navigate(typeof(BankAssessmentPage));
                }

                ++layoutPivot.SelectedIndex;
            }
        }

        public void ChangeTitle(string text)
        {
            featureTitle.Text = text;
        }

        private void NextImageButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectedModel is EpifaunalSubstrateModel)
            {
                var page = assessmentFrame.Content as EpifaunalAssessmentPage;

                if (page.NextImage() == -1)
                {
                    //nextImageButton.Visibility = Visibility.Visible;
                }
            }
            else if (SelectedModel is BankStabilityModel)
            {
                var page = assessmentFrame.Content as BankAssessmentPage;

                if (page.NextImage() == -1)
                {

                }
            }
        }

        private void NextProtocolButton_Click(object sender, RoutedEventArgs e)
        {
            ++layoutPivot.SelectedIndex;
        }
    }
}
