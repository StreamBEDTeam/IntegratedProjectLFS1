using StreamBED.Backend.Helper;
using StreamBED.Frontend.UWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StreamBED.Frontend.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EpifaunalAssessmentPage : Page
    {
        private ImageDataModel CurrentImage;

        private int CurrentScore = 20;

        private int Index = 0;

        private List<int> visit = new List<int>() { 15, 10, 5, 0 };

        public EpifaunalAssessmentPage()
        {
            this.InitializeComponent();

            Index = FeatureEvalPage.SelectedFeature.CountComplete;

            if (Index < FeatureEvalPage.SelectedFeature.ImageList.Count)
            {
                progressBar.Maximum = FeatureEvalPage.SelectedFeature.ImageList.Count;
                progressBar.Value = FeatureEvalPage.SelectedFeature.CountComplete;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NextImage();
        }

        public int NextImage()
        {
            if (Index < FeatureEvalPage.SelectedFeature.ImageList.Count)
            {
                CurrentImage = FeatureEvalPage.SelectedFeature.ImageList[Index++];

                selectedImage.Source = CurrentImage.ImageSource;

                foreach (UIElement elem in radioStack.Children)
                {
                    RadioButton button = elem as RadioButton;

                    if (button.IsChecked.Value)
                    {
                        button.IsChecked = false;
                    }

                    if (button.Content.Equals("20"))
                    {
                        button.IsChecked = true;
                    }

                    if (button.Style.Equals((Style)App.Current.Resources["AssessmentRadioButtonSmall"]))
                    {
                        button.Opacity = 0;
                        button.IsHitTestVisible = true;
                    }
                }

                detailButton1.Opacity = 1;
                detailButton1.IsHitTestVisible = true;

                detailButton2.Opacity = 1;
                detailButton2.IsHitTestVisible = true;

                detailButton3.Opacity = 1;
                detailButton3.IsHitTestVisible = true;

                detailButton4.Opacity = 1;
                detailButton4.IsHitTestVisible = true;

                XElement selected = FeatureEvalPage.SelectedFeatureReference.Where(i => i.Attribute("score").Value.Equals("20")).First();

                refImage.Source = new BitmapImage(new Uri(selected.Attribute("ref").Value));
                refImageDetail.Source = new BitmapImage(new Uri(selected.Attribute("detail").Value));
                refDetailText.Text = selected.Value.Replace("\n        ", "\n").Trim();

                return 0;
            }

            FeatureEvalPage.SelectedFeature.IsComplete = true;

            FeatureDataModel next = null;

            foreach (Keyword keyword in FeatureEvalPage.epifaunalSubstrateFeatures.Keys)
            {
                if (!FeatureEvalPage.epifaunalSubstrateFeatures.GetValueOrDefault(keyword).IsComplete)
                {
                    next = FeatureEvalPage.epifaunalSubstrateFeatures.GetValueOrDefault(keyword);
                    break;
                }
            }

            if (next != null)
            {
                (completionGrid.Children[0] as TextBlock).Text = "Great job rating the " + FeatureEvalPage.SelectedFeature.Keyword.Content + " feature.\n\nNext you will rate the " + next.Keyword.Content + " feature.";
                FeatureEvalPage.SelectedFeature = next;
                FeatureEvalPage.SelectedFeatureReference = FeatureEvalPage.EpifaunalReference.Where(i => i.Attribute("name").Value.Equals(FeatureEvalPage.SelectedFeature.Keyword.FriendlyName)).First().Elements().ToList();

                completionGrid.Visibility = Visibility.Visible;
            }
            else
            {
                FeatureEvalPage.SelectedModel.IsCompleted = true;

                if (FeatureEvalPage.EpifaunalSubstrateModel.IsCompleted && FeatureEvalPage.BankStabilityModel.IsCompleted)
                {
                    (Window.Current.Content as Frame).Navigate(typeof(FinalAssessmentPage));
                }
                else
                {
                    (completionGrid.Children[0] as TextBlock).Text = "Great job rating the Epifunal Substrate protocol.\n\nNext you will rate the Bank Stability protocol.";
                    FeatureEvalPage.SelectedFeature = null;
                    FeatureEvalPage.SelectedFeatureReference = null;

                    FeatureEvalPage.Current.ChangeToEpifaunalSubstrate();
                    completionGrid.Visibility = Visibility.Visible;

                    (((this.Parent as Frame).Parent as PivotItem).Parent as Pivot).SelectedIndex = 2;
                }
            }

            return -1;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentImage.Image.ChangeEpifaunalSubstrateScore(CurrentScore);
            CurrentImage.isComplete = true;

            ++FeatureEvalPage.SelectedFeature.CountComplete;

            ++progressBar.Value;

            NextImage();
        }

        private void ReferenceImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Storyboard animation = new Storyboard();

            DoubleAnimation reveal = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            Storyboard.SetTarget(reveal, refImage);
            Storyboard.SetTargetProperty(reveal, "UIElement.Opacity");

            animation.Children.Add(reveal);

            animation.Begin();
        }

        private void ReferenceImage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Storyboard animation = new Storyboard();


            DoubleAnimation hide = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.25))
            };

            Storyboard.SetTarget(hide, refImage);
            Storyboard.SetTargetProperty(hide, "UIElement.Opacity");

            animation.Children.Add(hide);

            animation.Begin();
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name;

            int u = 0, l = 0;

            if (name.Equals("detailButton1"))
            {
                u = 20;
                l = 15;

                detailButton1.Opacity = 0;
                detailButton1.IsHitTestVisible = false;
            }
            else if (name.Equals("detailButton2"))
            {
                u = 15;
                l = 10;

                detailButton2.Opacity = 0;
                detailButton2.IsHitTestVisible = false;
            }
            else if (name.Equals("detailButton3"))
            {
                u = 10;
                l = 5;

                detailButton3.Opacity = 0;
                detailButton3.IsHitTestVisible = false;
            }
            else if (name.Equals("detailButton4"))
            {
                u = 5;
                l = 0;

                detailButton4.Opacity = 0;
                detailButton4.IsHitTestVisible = false;
            }

            foreach (UIElement elem in radioStack.Children)
            {
                RadioButton button = elem as RadioButton;

                if (Convert.ToInt32(button.Content) > l && Convert.ToInt32(button.Content) < u)
                {
                    button.Opacity = 1;
                    button.IsHitTestVisible = true;
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeRadioButtonSelection(Convert.ToInt32((sender as RadioButton).Content), false);
        }

        private void ChangeRadioButtonSelection(int score, bool flag)
        {
            CurrentScore = score;

            if (flag)
            {
                SelectRadioButton(score);
            }

            if (CurrentScore == 20)
            {
                XElement selected = FeatureEvalPage.SelectedFeatureReference.Where(i => i.Attribute("score").Value.Equals("20")).First();

                refImage.Source = new BitmapImage(new Uri(selected.Attribute("ref").Value));
                refImageDetail.Source = new BitmapImage(new Uri(selected.Attribute("detail").Value));
                refDetailText.Text = selected.Value.Replace("\n        ", "\n").Trim();
                refDetailText.Visibility = Visibility.Collapsed;
            }
            else if (CurrentScore >= 15 && CurrentScore < 20)
            {
                XElement selected = FeatureEvalPage.SelectedFeatureReference.Where(i => i.Attribute("score").Value.Equals("15")).First();

                refImage.Source = new BitmapImage(new Uri(selected.Attribute("ref").Value));
                refImageDetail.Source = new BitmapImage(new Uri(selected.Attribute("detail").Value));
                refDetailText.Text = selected.Value.Replace("\n        ", "\n").Trim();
                refDetailText.Visibility = Visibility.Collapsed;
            }
            else if (CurrentScore >= 10 && CurrentScore < 15)
            {
                XElement selected = FeatureEvalPage.SelectedFeatureReference.Where(i => i.Attribute("score").Value.Equals("10")).First();

                refImage.Source = new BitmapImage(new Uri(selected.Attribute("ref").Value));
                refImageDetail.Source = new BitmapImage(new Uri(selected.Attribute("detail").Value));
                refDetailText.Text = selected.Value.Replace("\n        ", "\n").Trim();
                refDetailText.Visibility = Visibility.Collapsed;
            }
            else if (CurrentScore >= 5 && CurrentScore < 10)
            {
                XElement selected = FeatureEvalPage.SelectedFeatureReference.Where(i => i.Attribute("score").Value.Equals("5")).First();

                refImage.Source = new BitmapImage(new Uri(selected.Attribute("ref").Value));
                refImageDetail.Source = new BitmapImage(new Uri(selected.Attribute("detail").Value));
                refDetailText.Text = selected.Value.Replace("\n        ", "\n").Trim();
                refDetailText.Visibility = Visibility.Collapsed;
            }
            else if (CurrentScore >= 0 && CurrentScore < 5)
            {
                XElement selected = FeatureEvalPage.SelectedFeatureReference.Where(i => i.Attribute("score").Value.Equals("0")).First();

                refImage.Source = new BitmapImage(new Uri(selected.Attribute("ref").Value));
                refImageDetail.Source = new BitmapImage(new Uri(selected.Attribute("detail").Value));
                refDetailText.Text = selected.Value.Replace("\n        ", "\n").Trim();
                refDetailText.Visibility = Visibility.Collapsed;
            }

            if (visit.Contains(CurrentScore))
            {
                visit.Remove(CurrentScore);
            }

            if (visit.Count == 0)
            {
                detailButton1.IsHitTestVisible = true;
                detailButton2.IsHitTestVisible = true;
                detailButton3.IsHitTestVisible = true;
                detailButton4.IsHitTestVisible = true;

                submitButton.IsEnabled = true;
            }
        }

        private void SelectRadioButton(int value)
        {
            foreach (UIElement elem in radioStack.Children)
            {
                RadioButton button = elem as RadioButton;

                if (button.IsChecked.Value)
                {
                    button.IsChecked = false;
                }

                if (button.Content.Equals(value.ToString()))
                {
                    button.IsChecked = true;
                }
            }
        }

        private void GlanceCloseButton_Click(object sender, RoutedEventArgs e)
        {
            glanceProtocol.Visibility = Visibility.Collapsed;
        }

        private void GlanceOpenButton_Click(object sender, RoutedEventArgs e)
        {
            glanceProtocol.Visibility = Visibility.Visible;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeatureEvalPage.SelectedFeature == null)
            {
                FeatureEvalPage.Current.ChangeToBankStability();

                (((this.Parent as Frame).Parent as PivotItem).Parent as Pivot).SelectedIndex = 2;
            }
            else
            {
                FeatureEvalPage.Current.ChangeTitle(FeatureEvalPage.SelectedFeature.Keyword.Content.ToUpper());
                
                (this.Parent as Frame).Navigate(typeof(EpifaunalAssessmentPage));
            }
        }

        private void Chevron_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (sender as Button).Name.Equals("rightChevron");

            if (CurrentScore < 20 && CurrentScore > 15)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(15, true);
                }
                else
                {
                    ChangeRadioButtonSelection(20, true);
                }
            }
            else if (CurrentScore < 15 && CurrentScore > 10)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(10, true);
                }
                else
                {
                    ChangeRadioButtonSelection(15, true);
                }
            }
            else if (CurrentScore < 10 && CurrentScore > 5)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(5, true);
                }
                else
                {
                    ChangeRadioButtonSelection(10, true);
                }
            }
            else if (CurrentScore < 5 && CurrentScore > 0)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(0, true);
                }
                else
                {
                    ChangeRadioButtonSelection(5, true);
                }
            }
            else if (CurrentScore == 20)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(15, true);
                }
            }
            else if (CurrentScore == 15)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(10, true);
                }
                else
                {
                    ChangeRadioButtonSelection(20, true);
                }
            }
            else if (CurrentScore == 10)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(5, true);
                }
                else
                {
                    ChangeRadioButtonSelection(15, true);
                }
            }
            else if (CurrentScore == 5)
            {
                if (flag)
                {
                    ChangeRadioButtonSelection(0, true);
                }
                else
                {
                    ChangeRadioButtonSelection(10, true);
                }
            }
            else if (CurrentScore == 0)
            {
                if (!flag)
                {
                    ChangeRadioButtonSelection(5, true);
                }
            }
        }

        private void RefImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            refDetailText.Visibility = Visibility.Visible;
        }
    }
}
