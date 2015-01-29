using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppeteer.Armature;
using Puppeteer.Extensions;

namespace Puppeteer.Animation
{
    public class SkeletonAnimation
    {
        private Dictionary<Bone, List<BoneAnimationFrame>> _framesMapping;
        private Skeleton _skeleton;
        private TimeSpan _elapsedTime;
        private TimeSpan _longestAnimationTime;

        public Dictionary<Bone, List<BoneAnimationFrame>> FramesMapping { get { return _framesMapping; } }

        public SkeletonAnimation(Skeleton skeleton)
        {
            _framesMapping = new Dictionary<Bone, List<BoneAnimationFrame>>();
            _skeleton = skeleton;
        }

        public void Update(TimeSpan elapsedTimeSinceLastUpdate)
        {
            _elapsedTime += elapsedTimeSinceLastUpdate;
            UpdateBones();
        }

        public void SetTime(TimeSpan totalElapsedTime)
        {
            _elapsedTime = totalElapsedTime;
            UpdateBones();
        }

        public void AddAnimationFrame(Bone bone, BoneAnimationFrame frame)
        {
            if (_framesMapping.ContainsKey(bone))
            {
                _framesMapping[bone].Add(frame);
            }
            else
            {
                var frames = new List<BoneAnimationFrame>() { frame };
                _framesMapping.Add(bone, frames);
            }

            if (_longestAnimationTime < frame.EndTime)
            {
                _longestAnimationTime = frame.EndTime;
            }
        }

        void Reset()
        {
            _elapsedTime = new TimeSpan();
        }

        private void UpdateBones()
        {
            foreach (var bone in _skeleton)
            {
                var endTime = _elapsedTime;
                if (!_framesMapping.ContainsKey(bone)) 
                    continue;
                var frames = _framesMapping[bone];
                var frame = frames.GetAt(_elapsedTime);

                if (frame == null)
                {
                    frame = frames.GetLastFrame();
                    endTime = frame.EndTime;
                }

                if (frame == null) 
                    continue;
                frame.SetBoneTransformationValues(bone, endTime);
            }
        }
    }
}
