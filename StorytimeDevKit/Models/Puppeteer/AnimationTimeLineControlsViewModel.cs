using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class AnimationTimeLineControlsViewModel : BaseViewModel
    {
        private bool _record;
        private bool _play;
        private bool _playEnabled;

        public bool Record
        {
            get
            {
                return _record;
            }
            set
            {
                if (_record == value) return;
                _record = value;
                if (_record)
                {
                    PlayEnabled = false;
                }
                else
                {
                    PlayEnabled = true;
                }
                Play = false;
                OnPropertyChanged("Record");
            }
        }

        public bool Play
        {
            get
            {
                return _play;
            }
            set
            {
                if (_play == value) return;
                _play = value;
                OnPropertyChanged("Play");
            }
        }

        public bool PlayEnabled
        {
            get
            {
                return _playEnabled;
            }
            private set
            {
                if (_playEnabled == value) return;
                _playEnabled = value;
                OnPropertyChanged("PlayEnabled");
            }
        }

        public AnimationTimeLineControlsViewModel()
        {
            PlayEnabled = true;
        }
    }
}
