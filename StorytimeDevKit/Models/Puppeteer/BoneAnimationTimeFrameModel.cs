using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Puppeteer.Animation;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class BoneAnimationTimeFrameModel : TimeFrame
    {
        public BoneState StartState { get; set; }
        public BoneState EndState { get; set; }
        public BoneAnimationFrame AnimationFrame { get; set; }

        public BoneAnimationTimeFrameModel()
        {
            base.OnStartTimeChanges += OnStartTimeChangesHandler;
            base.OnEndTimeChanges += OnEndTimeChangesHandler;
        }

        private void OnStartTimeChangesHandler(TimeFrame frame)
        {
            if (AnimationFrame == null) return;
            AnimationFrame.StartTime = StartTime;
        }

        private void OnEndTimeChangesHandler(TimeFrame frame)
        {
            if (AnimationFrame == null) return;
            AnimationFrame.EndTime = EndTime;
        }
    }
}
