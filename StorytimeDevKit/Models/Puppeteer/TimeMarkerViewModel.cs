using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class TimeMarkerViewModel : BaseViewModel
    {
        private double _xOrigin;
        private double _pixelsPerUnit;
        private double _x;
        private Visibility _visible;

        public event Action<double> OnSecondsChange;

        public double Seconds
        {
            get
            {
                return _x / _pixelsPerUnit;
            }
            set
            {
                if (value < 0) return;
                if (Seconds == value) return;
                X = value * _pixelsPerUnit;
            }
        }

        public double LineXPosition
        {
            get
            {
                return _x + _xOrigin;
            }
        }

        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                if (_x == value) return;
                _x = value;
                OnPropertyChanged("X");
                OnPropertyChanged("LineXPosition");

                if (OnSecondsChange != null)
                    OnSecondsChange(Seconds);
            }
        }

        public Visibility Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible == value) return;
                _visible = value;
                OnPropertyChanged("Visible");
            }
        }

        public double XOrigin
        {
            get
            {
                return _xOrigin;
            }
            set
            {
                if (_xOrigin == value) return;
                _xOrigin = value;
                OnPropertyChanged("XOrigin");
                OnPropertyChanged("LineXPosition");
            }
        }

        public TimeMarkerViewModel(double pixelsPerUnit)
        {
            _pixelsPerUnit = pixelsPerUnit;
            _visible = Visibility.Hidden;
        }
    }
}
