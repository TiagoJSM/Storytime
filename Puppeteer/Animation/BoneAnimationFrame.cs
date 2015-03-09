using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Utils;
using Puppeteer.Armature;
using StoryTimeCore.Extensions;

namespace Puppeteer.Animation
{
    public class BoneAnimationFrame
    {
        public TimeSpan Duration 
        {
            get 
            {
                return EndTime - StartTime;
            }
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Vector2 StartTranslation { get; set; }
        public float StartRotation { get; set; }

        public Vector2 EndTranslation { get; set; }
        public float EndRotation { get; set; }

        public Vector2 InterpolateTranslation(TimeSpan totalElapsed)
        {
            return MathematicalUtils.LinearInterpolation(StartTranslation, EndTranslation, Duration, totalElapsed - StartTime);
        }

        public float InterpolateRotation(TimeSpan totalElapsed)
        {
            //var endRotation = StartRotation + TotalRotation;
            return MathematicalUtils.LinearInterpolation(StartRotation, EndRotation, Duration, totalElapsed - StartTime).ReduceRotationToOneTurn();
        }

        public void SetBoneTransformationValues(Bone bone, TimeSpan totalElapsed)
        {
            bone.Translation = InterpolateTranslation(totalElapsed);
            bone.Rotation = InterpolateRotation(totalElapsed);
        }

        public bool IsContainedInInterval(TimeSpan time, bool containEnding = false)
        {
            if (time < StartTime)
                return false;
            if (time > EndTime)
                return false;
            if (!containEnding)
                return true;
            return time == EndTime;
        }
    }
}
