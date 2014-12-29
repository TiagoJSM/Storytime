using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeDevKit.Models.Puppeteer;
using System.Collections.ObjectModel;
using TimeLineTool;
using Puppeteer.Armature;
using MoreLinq;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Extensions;

namespace StoryTimeDevKit.DataStructures
{
    public class AnimationTimeLineMapper
    {
        private Dictionary<BoneActor, ObservableCollection<TimeFrame>> _mapper;

        public AnimationTimeLineMapper()
        {
            _mapper = new Dictionary<BoneActor, ObservableCollection<TimeFrame>>();
        }

        public void AddTimeLineFor(BoneActor actor)
        {
            _mapper.Add(actor, new ObservableCollection<TimeFrame>());
        }

        public void AddAnimationFrame(BoneActor actor, double animationEndTimeInSeconds)
        {
            TimeFrame frameAtTime = GetFrameAt(actor, animationEndTimeInSeconds);
            if (frameAtTime != null) return;

            ObservableCollection<TimeFrame> dataCollection = GetCollectionBoundToActor(actor);
            TimeFrame frame = GetLastTimeFrame(dataCollection);

            TimeFrame item = null;
            if (frame == null)
            {
                item = new TimeFrame()
                    {
                        StartTime = new TimeSpan(),
                        EndTime = TimeSpan.FromSeconds(animationEndTimeInSeconds)
                    };
            }
            else
            {
                item = new TimeFrame()
                {
                    StartTime = frame.EndTime,
                    EndTime = frame.EndTime.Add(TimeSpan.FromSeconds(animationEndTimeInSeconds))
                };
            }
            dataCollection.Add(item);
        }

        public ObservableCollection<TimeFrame> GetCollectionBoundToActor(BoneActor actor)
        {
            if (_mapper.ContainsKey(actor))
                return _mapper[actor];
            return null;
        }

        public TimeFrame GetFrameAt(BoneActor actor, double seconds)
        {
            ObservableCollection<TimeFrame> items = GetCollectionBoundToActor(actor);
            if (items == null) return null;
            TimeSpan convertedSeconds = TimeSpan.FromSeconds(seconds);
            return items.FirstOrDefault(i => i.IsIntervalIntesected(convertedSeconds, true));
        }

        private TimeFrame GetLastTimeFrame(ObservableCollection<TimeFrame> items)
        {
            if (!items.Any()) return null;
            return items.MaxBy(i => i.EndTime) as TimeFrame;
        }
    }
}
