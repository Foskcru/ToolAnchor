using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ToolAnchor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        float widthRatio = -10;
        float heightRatio = -10;

        float offsetLeft = -10;
        float offsetTop = -10;
        float offsetRight = -10;
        float offsetBottom = -10;

        float MinimumX = -10;
        float MinimumY = -10;
        float MaximumX = -10;
        float MaximumY = -10;

        private void ChangeVisibility(TextBox _textBox, TextBlock _textBlock)
        {
            if (_textBox.Text.Length > 0)
                _textBlock.Visibility = Visibility.Collapsed;
            else if (_textBox.Text.Length == 0)
                _textBlock.Visibility = Visibility.Visible;
        }

        private void SetAnchorAdjust()
        {
            //Console.WriteLine("--------------------");
            //Console.WriteLine($"widthRatio {widthRatio}");
            //Console.WriteLine($"heightRatio {heightRatio}");
                              
            //Console.WriteLine($"MinimumX {MinimumX}");
            //Console.WriteLine($"MinimumY {MinimumY}");
            //Console.WriteLine($"MaximumX {MaximumX}");
            //Console.WriteLine($"MaximumY {MaximumY}");
                              
            //Console.WriteLine($"offsetLeft {offsetLeft}");
            //Console.WriteLine($"offsetTop {offsetTop}");
            //Console.WriteLine($"offsetRight {offsetRight}");
            //Console.WriteLine($"offsetBottom {offsetBottom}");

            if (widthRatio == -10 || heightRatio == -10) return;
            if (MinimumX == -10 || MinimumY == -10 || MaximumX == -10 || MaximumY == -10) return;
            if (offsetLeft == -10 || offsetTop == -10 || offsetRight == -10 || offsetBottom == -10) return;

            string _minimumX = (MinimumX + (offsetLeft * widthRatio)).ToString();
            string _minimumY = (MinimumY + (offsetTop * heightRatio)).ToString();
            string _maximumX = (MaximumX - (offsetRight * widthRatio)).ToString();
            string _maximumY = (MaximumY - (offsetBottom * heightRatio)).ToString();

            AnchorAdjustText.Text = "(Minimum=(X=" + _minimumX + ",Y=" + _minimumY + "),Maximum=(X=" + _maximumX + ",Y=" + _maximumY + "))";
        }

        private void WidthText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(WidthText, DefaultWidth);
            if (!string.IsNullOrEmpty(WidthText.Text) && float.TryParse(WidthText.Text, out _))
                widthRatio = 1 / float.Parse(WidthText.Text);
            SetAnchorAdjust();
        }

        private void HeightText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(HeightText, DefaultHeight);
            if (!string.IsNullOrEmpty(HeightText.Text) && float.TryParse(HeightText.Text, out _))
                heightRatio = 1 / float.Parse(HeightText.Text);
            SetAnchorAdjust();
        }

        private void AnchorsText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(AnchorsText, DefaultAnchors);

            string pattern = @"\(Minimum=\(X=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Y=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\),Maximum=\(X=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Y=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\)\)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(AnchorsText.Text))
            {
                string[] _value = regex.Split(AnchorsText.Text);

                MinimumX = float.Parse(_value[1]);
                MinimumY = float.Parse(_value[2]);
                MaximumX = float.Parse(_value[3]);
                MaximumY = float.Parse(_value[4]);
            }
            SetAnchorAdjust();
        }

        private void OffsetLeftText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(OffsetLeftText, DefaultOffsetLeft);
            if (!string.IsNullOrEmpty(OffsetLeftText.Text) && float.TryParse(OffsetLeftText.Text, out _))
                offsetLeft = float.Parse(OffsetLeftText.Text);
            SetAnchorAdjust();
        }

        private void OffsetTopText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(OffsetTopText, DefaultOffsetTop);
            if (!string.IsNullOrEmpty(OffsetTopText.Text) && float.TryParse(OffsetTopText.Text, out _))
                offsetTop = float.Parse(OffsetTopText.Text);
            SetAnchorAdjust();
        }

        private void OffsetRightText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(OffsetRightText, DefaultOffsetRight);
            if (!string.IsNullOrEmpty(OffsetRightText.Text) && float.TryParse(OffsetRightText.Text, out _))
                offsetRight = float.Parse(OffsetRightText.Text);
            SetAnchorAdjust();
        }

        private void OffsetBottomText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(OffsetBottomText, DefaultOffsetBottom);
            if (!string.IsNullOrEmpty(OffsetBottomText.Text) && float.TryParse(OffsetBottomText.Text, out _))
                offsetBottom = float.Parse(OffsetBottomText.Text);
            SetAnchorAdjust();
        }

        private void AnchorAdjustText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(AnchorAdjustText, DefaultAnchorAdjust);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            //widthRatio = -10;
            //heightRatio = -10;

            offsetLeft = -10;
            offsetTop = -10;
            offsetRight = -10;
            offsetBottom = -10;

            MinimumX = -10;
            MinimumY = -10;
            MaximumX = -10;
            MaximumY = -10;


            //WidthText.Text = "";
            //HeightText.Text = "";
            AnchorsText.Text = "";
            OffsetLeftText.Text = "";
            OffsetTopText.Text = "";
            OffsetRightText.Text = "";
            OffsetBottomText.Text = "";
            AnchorAdjustText.Text = "";
;

        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            //Clipboard.SetDataObject("test");
            //if (!string.IsNullOrEmpty(AnchorAdjustText.Text))
            //    Clipboard.SetText(AnchorAdjustText.Text);
        }
    }
}
