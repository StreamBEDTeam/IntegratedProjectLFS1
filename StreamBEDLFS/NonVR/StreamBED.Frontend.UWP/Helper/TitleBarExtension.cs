using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace StreamBED.Frontend.UWP.Helper
{
    public class TitleBarExtension
    {
        public static ApplicationViewTitleBar TitleBar { get { return ApplicationView.GetForCurrentView().TitleBar; } }

        public static CoreApplicationViewTitleBar CoreTitleBar { get { return CoreApplication.GetCurrentView().TitleBar; } }

        public static double Height { get { return CoreTitleBar.Height; } }

        public static void ModifyTitleBar()
        {
            TitleBar.ButtonForegroundColor = Colors.Black;
            TitleBar.ButtonInactiveForegroundColor = Colors.Black;
            TitleBar.ButtonBackgroundColor = Colors.Transparent;
            TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(64, 0, 0, 0);
            TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(128, 0, 0, 0);

            CoreTitleBar.ExtendViewIntoTitleBar = true;
        }
    }
}
