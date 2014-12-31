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
using TimeLineTool;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Extensions;
using StoryTimeDevKit.Models;
using StoryTimeCore.Extensions;
using MoreLinq;
using System.Collections.Specialized;
using System.ComponentModel;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for SingleTimeLineControl.xaml
    /// </summary>
    public partial class SingleTimeLineControl : UserControl
    {
        private class DataPoint : BaseViewModel
        {
            private double _logicalX;
            private double _logicalY;
            
            private DataPoint _previous;
            private TimeFrame _frame;
            private SingleTimeLineControl _control;

            private int _zIndex;

            public event Action<DataPoint> OnEndChanges;

            public double LogicalX
            {
                get
                {
                    return _logicalX;
                }
            }

            public double LogicalY
            {
                get
                {
                    return _logicalY;
                }
            }

            public double? PreviousThumbX
            {
                get
                {
                    if (_previous == null) return null;
                    return _previous.StartX;
                }
            }

            public double? PreviousThumbY
            {
                get
                {
                    if (_previous == null) return null;
                    return _previous.StartY;
                }
            }

            public double StartX
            {
                get
                {
                    if (_previous == null) return 0;
                    return _previous.EndX;
                }
            }

            public double StartY
            {
                get
                {
                    if (_previous == null) return 0;
                    return _previous.EndY;
                }
            }

            public double EndX
            {
                get
                {
                    return _logicalX.NearestMultipleOf(_control.Step);
                }
                set
                {
                    if (_logicalX == value) return;
                    _logicalX = value;
                    if (OnEndChanges != null)
                        OnEndChanges(this);
                    base.OnPropertyChanged("EndX");
                    base.OnPropertyChanged("Width");
                }
            }

            public double EndY
            {
                get
                {
                    return _logicalY.NearestMultipleOf(_control.Step);
                }
                set
                {
                    if (_logicalY == value) return;
                    _logicalY = value;
                    if (OnEndChanges != null)
                        OnEndChanges(this);
                    base.OnPropertyChanged("EndY");
                }
            }

            public double Width
            {
                get
                {
                    return EndX - StartX;
                }
            }

            public int ZIndex
            {
                get 
                {
                    return _zIndex;
                }
                set 
                {
                    if (_zIndex == value) return;
                    _zIndex = value;
                    OnPropertyChanged("ZIndex");
                }
            }

            public DataPoint(DataPoint previous, TimeFrame frame, SingleTimeLineControl control)
            {
                _previous = previous;
                _frame = frame;
                _control = control;
                if (_previous != null)
                    _previous.OnEndChanges += OnEndChangesHandler;
                if (_frame != null)
                {
                    _logicalX = _control.GetDataPointXFromFrame(frame);
                    _frame.PropertyChanged += PropertyChangedHandler;
                }
            }

            private void OnEndChangesHandler(DataPoint previous)
            {
                base.OnPropertyChanged("StartX");
                base.OnPropertyChanged("StartY");
                base.OnPropertyChanged("Width"); ;
            }

            private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
            {
                EndX = _control.GetDataPointXFromFrame(_frame);
            }
        }

        private ObservableCollection<TimeFrame> _timeFrames;
        private ObservableCollection<DataPoint> _points;

        public ObservableCollection<TimeFrame> TimeFrames 
        {
            get
            {
                return _timeFrames;
            }
            set
            {
                if (_timeFrames == value) return;
                if(_timeFrames != null)
                    _timeFrames.CollectionChanged -= timeframes_CollectionChanged;
                _timeFrames = value;
                if (_timeFrames != null)
                    _timeFrames.CollectionChanged += timeframes_CollectionChanged;
            }
        }

        public static readonly DependencyProperty PixelsPerUnitProperty =
            DependencyProperty.Register("PixelsPerUnit", typeof(double), typeof(SingleTimeLineControl));

        public double PixelsPerUnit
        {
            get { return (double)GetValue(PixelsPerUnitProperty); }
            set { SetValue(PixelsPerUnitProperty, value); }
        }

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(SingleTimeLineControl));

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set 
            {
                if (value < 0)
                    value = 1;
                SetValue(StepProperty, value); 
            }
        }

        public SingleTimeLineControl()
        {
            InitializeComponent();
            Step = 5;
            PixelsPerUnit = 20;
            //TimeFrames = new ObservableCollection<TimeFrame>();
            //TimeFrames.CollectionChanged += timeframes_CollectionChanged;
            _points = new ObservableCollection<DataPoint>();

            list.ItemsSource = _points;

            /*TimeFrames.Add(
                new TimeFrame()
                {
                    StartTime = new TimeSpan(),
                    EndTime = new TimeSpan(0, 0, 3)
                });

            TimeFrames.Add(
                new TimeFrame()
                {
                    StartTime = new TimeSpan(0, 0, 3),
                    EndTime = new TimeSpan(0, 0, 7)
                });*/
        }

        /*public AnimationDataItem GetDataItemAt(DateTime time)
        {
            foreach (ITimeLineDataItem item in DataItems)
            {
                if (item.IsIntervalIntesected(time))
                    return item as AnimationDataItem;
            }
            return null;
        }*/

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = e.Source as FrameworkElement;
            var point = thumb.DataContext as DataPoint;
            var newX = point.EndX + e.HorizontalChange;
            
            var next = GetDataPointAfter(point);
            if (next != null)
            {
                if (newX >= next.LogicalX)
                {
                    return;
                }
            }

            var previous = GetDataPointBefore(point);
            if (previous != null)
            {
                if (newX <= previous.LogicalX)
                {
                    return;
                }
            }

            if (newX < 0)
                newX = 0;
            point.EndX = newX;
        }

        private DataPoint GetDataPointAfter(DataPoint point)
        {
            return GetDataPointAfter(point.LogicalX);
        }

        private DataPoint GetDataPointAfter(double logicalX)
        {
            return _points.Where(dp => dp != null && dp.LogicalX > logicalX).MinOrDefaultBy(dp => dp.LogicalX);
        }

        private DataPoint GetDataPointBefore(DataPoint point)
        {
            return GetDataPointBefore(point.LogicalX);
        }

        private DataPoint GetDataPointBefore(double logicalX)
        {
            return _points.Where(dp => dp != null && dp.LogicalX < logicalX).MaxOrDefaultBy(dp => dp.LogicalX);
        }

        private void timeframes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!_points.Any())
                {
                    _points.Add(new DataPoint(null, null, this));
                }
                var frames = e.NewItems.Cast<TimeFrame>();
                foreach(var frame in frames)
                {
                    var logicalX = GetDataPointXFromFrame(frame);
                    var previous = GetDataPointBefore(logicalX);
                    _points.Add(new DataPoint(previous, frame, this));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
            }

            OrderDataPoints();
        }

        private void OrderDataPoints()
        {
            var index = 0;
            _points.OrderByDescending(dp => dp.LogicalX).ForEach(dp => dp.ZIndex = index++);
        }

        private double GetDataPointXFromFrame(TimeFrame frame)
        {
            return frame.EndTime.TotalSeconds * PixelsPerUnit;
        }
    }
}
