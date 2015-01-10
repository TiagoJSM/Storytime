using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Enums;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class PuppeteerWorkingModesModel : BaseViewModel
    {
        private PuppeteerWorkingModeType _workingMode;

        public PuppeteerWorkingModeType WorkingMode
        {
            get
            {
                return _workingMode;
            }
            set
            {
                if (_workingMode == value) return;
                _workingMode = value;
                base.OnPropertyChanged("WorkingMode");
            }
        }
    }
}
