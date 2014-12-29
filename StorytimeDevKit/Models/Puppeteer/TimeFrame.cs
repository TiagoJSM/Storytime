using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class TimeFrame : BaseViewModel
    {
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public TimeSpan StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                if (_startTime == value) return;
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                if (_endTime == value) return;
                _endTime = value;
                OnPropertyChanged("EndTime");
            }
        }
    }
}
