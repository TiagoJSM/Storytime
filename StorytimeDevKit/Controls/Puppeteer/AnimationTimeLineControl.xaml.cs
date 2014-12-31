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
using System.Windows.Threading;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for AnimationTimeLine.xaml
    /// </summary>
    public partial class AnimationTimeLineControl : UserControl, IAnimationTimeLineControl
    {
        private double maxWith = 4000;

        private IAnimationTimeLineController _timelineController;

        private TimeMarkerViewModel _timeMarkerModel;

        public ObservableCollection<TimeLineTuple> Controls { get; private set; }

        private DispatcherTimer _timeMarkerTimer;

        public event Action<double> OnTimeMarkerChange;

        public AnimationTimeLineControl()
        {
            InitializeComponent();

            Controls = new ObservableCollection<TimeLineTuple>();
            Controls.CollectionChanged += ControlsCollectionChangeHandler;
            _timeMarkerTimer = new DispatcherTimer();
            _timeMarkerTimer.Interval = TimeSpan.FromSeconds(0.1);
            _timeMarkerTimer.Tick += timeMarkerTimer_TickHandler;
            
            _timeMarkerModel = new TimeMarkerViewModel(Ruler.PixelsPerUnit);
            line.DataContext = _timeMarkerModel;

            TimeLines.LayoutUpdated += TimeLines_LayoutUpdated;
            Loaded += LoadedHandler;
        }

        public void AddTimeLine(BoneViewModel bone, ObservableCollection<TimeFrame> items)
        {
            Controls.Add(
                new TimeLineTuple()
                {
                    Bone = bone,
                    Control = new SingleTimeLineControl() { Width = maxWith, MaxWidth = maxWith, TimeFrames = items }
                });
            
        }

        public void AddFrame(BoneViewModel bone, float rotation, Vector2 position)
        {
           //this.InvalidateVisual();
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
            var gridView = listView.View as GridView;
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
            _timeMarkerModel.X = Mouse.GetPosition(Ruler).X;
            _timeMarkerModel.Visible = Visibility.Visible;

            if (OnTimeMarkerChange != null)
                OnTimeMarkerChange(_timeMarkerModel.Seconds);
        }

        private void TimeLines_LayoutUpdated(object sender, EventArgs e)
        {
            _timeMarkerModel.XOrigin = Ruler.TranslatePoint(new System.Windows.Point(0, 0), this).X;
        }

        void timeMarkerTimer_TickHandler(object sender, EventArgs e)
        {
            _timeMarkerModel.Seconds += _timeMarkerTimer.Interval.TotalSeconds;
        }
    }
}
