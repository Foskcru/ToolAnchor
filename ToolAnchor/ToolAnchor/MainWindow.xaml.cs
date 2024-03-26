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
            ResizeMode = ResizeMode.CanMinimize;
            Topmost = true;
            WidgetRichTextBox.Document.Blocks.Clear(); 
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

            string _minimumX = "";
            string _minimumY = "";
            string _maximumX = "";
            string _maximumY = "";
            if (MinimumX != 0 || MinimumY != 0 || MaximumX != 0 || MaximumY != 0)
            {
                _minimumX = (MinimumX + (offsetLeft * widthRatio)).ToString();
                _minimumY = (MinimumY + (offsetTop * heightRatio)).ToString();
                _maximumX = (MaximumX - (offsetRight * widthRatio)).ToString();
                _maximumY = (MaximumY - (offsetBottom * heightRatio)).ToString();
            }
            else
            {
                float _minX = offsetLeft * widthRatio;
                float _minY = offsetTop * heightRatio;
                float _maxX = offsetRight * widthRatio;
                float _maxY = offsetBottom * heightRatio;
                
                _minimumX = (_minX).ToString();
                _minimumY = (_minY).ToString();
                _maximumX = (_maxX + _minX).ToString();
                _maximumY = (_maxY + _minY).ToString();
            }

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
        
        private void AnchorAdjustText_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeVisibility(AnchorAdjustText, DefaultAnchorAdjust);
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            WidgetRichTextBox.Document.Blocks.Clear();
            WidgetRichTextBox.AppendText(Clipboard.GetText());
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AnchorAdjustText.Text))
                Clipboard.SetDataObject(AnchorAdjustText.Text);
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
            
            WidgetRichTextBox.Document.Blocks.Clear(); 
            
            AnchorAdjustText.Text = "";
        }


        
        private void WidgetRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WidgetRichTextBox.Document.Blocks.Count() > 0)
            {
                if(DefaultWidget != null)
                    DefaultWidget.Visibility = Visibility.Collapsed;
            }
            else if (WidgetRichTextBox.Document.Blocks.Count()  == 0)
            {
                if(DefaultWidget != null)
                    DefaultWidget.Visibility = Visibility.Visible;
            }
            
            TextRange source = new TextRange(WidgetRichTextBox.Document.ContentStart, WidgetRichTextBox.Document.ContentEnd);
            
            const string patternV1 = @"\(Offsets=\(Left=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Top=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Right=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Bottom=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\),Anchors=\(Minimum=\(X=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Y=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\),Maximum=\(X=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Y=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\)\)";
            const string patternV2 = @"[A-Za-z0-9]+\(0\)=""\(Offsets=\(Left=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Top=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Right=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Bottom=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\),Anchors=\(Minimum=\(X=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Y=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\),Maximum=\(X=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?,Y=([+-]?(?=\.\d|\d)(?:\d+)?(?:\.?\d*))(?:[Ee]([+-]?\d+))?\)\)";
            Regex _regex = new Regex(patternV2, RegexOptions.IgnoreCase);

            if (_regex.IsMatch(source.Text))
            {
                string[] _value = _regex.Split(source.Text);
                if (_value.Count() >= 9)
                {
                    offsetLeft = float.Parse(_value[1]);
                    offsetTop = float.Parse(_value[2]);
                    offsetRight = float.Parse(_value[3]);
                    offsetBottom = float.Parse(_value[4]);
                    
                    MinimumX = float.Parse(_value[5]);
                    MinimumY = float.Parse(_value[6]);
                    MaximumX = float.Parse(_value[7]);
                    MaximumY = float.Parse(_value[8]);
                }
            }
            SetAnchorAdjust();
        }

    
    }
}
