using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeLineTool;
using StoryTimeDevKit.Configurations;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class AnimationDataItem : TempDataType
    {
        public string Interval 
        { 
            get 
            {
                double startingHours = (StartTime.Value - PuppeteerDefaults.StartDate).TotalHours;
                double endingHours = (EndTime.Value - PuppeteerDefaults.StartDate).TotalHours;

                return string.Format("{0}s - {1}s", Math.Round(startingHours, 2), Math.Round(endingHours, 2));
            } 
        }
    }
}
