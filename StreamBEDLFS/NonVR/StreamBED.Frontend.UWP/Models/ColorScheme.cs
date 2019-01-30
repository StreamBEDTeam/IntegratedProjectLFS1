using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace StreamBED.Frontend.UWP.Models
{
    public class ColorScheme
    {
        private int Index = 0;

        private readonly string[] ColorSchemeList = {
            "#A6CEE3",
            "#1F78B4",
            "#B2DF8A",
            "#33A02C",
            "#FB9A99",
            "#FDBF6F",
            "#FF7F00"
        };

        public Color GetColor()
        {
            if (Index < ColorSchemeList.Count())
            {
                return ColorFromHex(ColorSchemeList[Index++]);
            }
            else
            {
                return new Color();
            }
        }

        public static Color ColorFromHex(string hex)
        {
            System.Drawing.Color colorReference = (System.Drawing.Color)(new System.Drawing.ColorConverter()).ConvertFromString(hex);

            Color color = new Color();
            color.A = colorReference.A;
            color.R = colorReference.R;
            color.G = colorReference.G;
            color.B = colorReference.B;

            return color;
        }

    }
}
