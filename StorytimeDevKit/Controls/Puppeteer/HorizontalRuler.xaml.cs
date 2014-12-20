using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for HorizontalRuler.xaml
    /// </summary>
    public partial class HorizontalRuler : UserControl
    {
        private Typeface _typeface;

        private double majorLineHeight;
        private double minorLineHeight;

        public static readonly DependencyProperty PixelsPerUnitProperty =
            DependencyProperty.Register("PixelsPerUnit", typeof(double), typeof(HorizontalRuler));

        public static readonly DependencyProperty MajorUnitIntervalProperty =
            DependencyProperty.Register("MajorUnitInterval", typeof(int), typeof(HorizontalRuler));

        public double PixelsPerUnit
        {
            get { return (double)GetValue(PixelsPerUnitProperty); }
            set { SetValue(PixelsPerUnitProperty, value); }
        }

        public int MajorUnitInterval
        {
            get { return (int)GetValue(MajorUnitIntervalProperty); }
            set { SetValue(MajorUnitIntervalProperty, value); }
        }

        public HorizontalRuler()
        {
            PixelsPerUnit = 20;
            MajorUnitInterval = 5;

            majorLineHeight = 10;
            minorLineHeight = 5;

            _typeface = new Typeface("Tahoma");

            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            bool major = true;
            double interval = MajorUnitInterval;
            double linesHeightDifference = majorLineHeight - minorLineHeight;

            double maxTextHeight = 10;

            for (int idx = 0; idx < this.ActualWidth / PixelsPerUnit; idx++)
            {
                major = idx == 0 || idx % interval == 0;
                double xPosition = idx * PixelsPerUnit;

                if (major)
                {
                    FormattedText ft = new FormattedText(idx.ToString(), CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, _typeface, 8, Brushes.Black);
                    drawingContext.DrawText(ft, new Point(xPosition - ft.Width/2, 0));

                    drawingContext.DrawLine(
                        new Pen(Brushes.Black, 1),
                        new Point(xPosition, maxTextHeight),
                        new Point(xPosition, maxTextHeight + majorLineHeight));
                }
                else
                {
                    drawingContext.DrawLine(
                        new Pen(Brushes.Black, 1),
                        new Point(xPosition, maxTextHeight + linesHeightDifference),
                        new Point(xPosition, maxTextHeight + majorLineHeight));
                }
            }
        }
    }
}
