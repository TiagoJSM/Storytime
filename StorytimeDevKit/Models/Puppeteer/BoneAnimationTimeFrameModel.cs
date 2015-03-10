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
        public Vector2 StartPosition
        {
            get
            {
                return AnimationFrame.StartTranslation;
            }
            set
            {
                AnimationFrame.StartTranslation = value;
            }
        }
        public float StartRotation
        {
            get
            {
                return AnimationFrame.StartRotation;
            }
            set
            {
                AnimationFrame.StartRotation = value;
            }
        }
        public Vector2 EndTranslation 
        {
            get
            {
                return AnimationFrame.EndTranslation;
            }
            set
            {
                AnimationFrame.EndTranslation = value;
            } 
        }
        public float EndRotation 
        {
            get
            {
                return AnimationFrame.TotalRotation;
            }
            set
            {
                AnimationFrame.TotalRotation = value;
            }
        }

        public BoneAnimationFrame AnimationFrame { get; private set; }

        public BoneAnimationTimeFrameModel(BoneAnimationFrame animationFrame)
        {
            AnimationFrame = animationFrame;
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
