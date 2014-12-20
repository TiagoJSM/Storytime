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
using System.Collections.ObjectModel;
using TimeLineTool;
using StoryTimeDevKit.Models.Puppeteer;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Controllers.Puppeteer;
using Ninject;
using System.Collections.Specialized;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for AnimationTimeLine.xaml
    /// </summary>
    public partial class AnimationTimeLineControl : UserControl, IAnimationTimeLineControl
    {
        private double maxWith = 4000;

        private IAnimationTimeLineController _timelineController;
        private double? _lineRulerRelativeX;

        public ObservableCollection<TimeLineTuple> Controls { get; private set; }
        public event Action<double> OnTimeMarkerChange;

        private double? Seconds
        {
            get
            {
                if(_lineRulerRelativeX == null) return null;
                return _lineRulerRelativeX.Value / Ruler.PixelsPerUnit;
            }
        }

        public AnimationTimeLineControl()
        {
            InitializeComponent();

            Controls = new ObservableCollection<TimeLineTuple>() 
            { 
                //new TimeLineTuple() { Control = new HorizontalRuler(){ Height = 30, Width = maxWith, MaxWidth = maxWith } }
                //new TimeLineTuple() { Name = "Bone1", Control = new SingleTimeLineControl() { Width = maxWith, MaxWidth = maxWith } },
                //new TimeLineTuple() { Name = "Bone2", Control = new SingleTimeLineControl() { Width = maxWith, MaxWidth = maxWith } }
            };

            Controls.CollectionChanged += ControlsCollectionChangeHandler;

            Loaded += LoadedHandler;
        }

        public void AddTimeLine(BoneViewModel bone, ObservableCollection<ITimeLineDataItem> items)
        {
            Controls.Add(
                new TimeLineTuple()
                {
                    Bone = bone,
                    Control = new SingleTimeLineControl() { Width = maxWith, MaxWidth = maxWith, DataItems = items }
                });
        }

        public void AddFrame(BoneViewModel bone, float rotation, Vector2 position)
        {
            throw new NotImplementedException();
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _timelineController =
                DependencyInjectorHelper
                    .PuppeteerKernel
                    .Get<IAnimationTimeLineController>();

            _timelineController.TimeLineControl = this;
        }

        private void ControlsCollectionChangeHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            AutoSizeGridViewColumns(TimeLines);
        }

        private void AutoSizeGridViewColumns(ListView listView)
        {
            GridView gridView = listView.View as GridView;
            if (gridView != null)
            {
                foreach (var column in gridView.Columns)
                {
                    if (double.IsNaN(column.Width))
                        column.Width = column.ActualWidth;
                    column.Width = double.NaN;
                }
            }
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            _lineRulerRelativeX = Mouse.GetPosition(Ruler).X;
            if (OnTimeMarkerChange != null)
                OnTimeMarkerChange(Seconds.Value);
            SetTimeLineCoordinates();
        }

        private void SetTimeLineCoordinates()
        {
            if (_lineRulerRelativeX == null) return;
            if (double.IsNaN(TimeLines.ActualHeight)) return;

            double timelineX = Ruler.TranslatePoint(new System.Windows.Point(0, 0), this).X;

            double rulerX = _lineRulerRelativeX.Value + timelineX;

            line.Visibility = Visibility.Visible;
            line.X1 = rulerX;
            line.Y1 = 0;
            line.X2 = rulerX;
            line.Y2 = TimeLines.ActualHeight;
        }

        private void TimeLines_LayoutUpdated(object sender, EventArgs e)
        {
            SetTimeLineCoordinates();
        }
    }
}
