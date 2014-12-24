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
        private Dictionary<BoneActor, ObservableCollection<ITimeLineDataItem>> _mapper;

        public AnimationTimeLineMapper()
        {
            _mapper = new Dictionary<BoneActor, ObservableCollection<ITimeLineDataItem>>();
        }

        public void AddTimeLineFor(BoneActor actor)
        {
            _mapper.Add(actor, new ObservableCollection<ITimeLineDataItem>());
        }

        public void AddAnimationFrame(BoneActor actor, double animationEndTimeInSeconds)
        {
            ITimeLineDataItem frameAtTime = GetFrameAt(actor, animationEndTimeInSeconds);
            if (frameAtTime != null) return;

            ObservableCollection<ITimeLineDataItem> dataCollection = GetCollectionBoundToActor(actor);
            AnimationDataItem frame = GetLastAnimationFrame(dataCollection);

            AnimationDataItem item = null;
            if (frame == null)
            {
                item = new AnimationDataItem()
                    {
                        StartTime = PuppeteerDefaults.StartDate,
                        EndTime = PuppeteerDefaults.StartDate.AddHours(animationEndTimeInSeconds)
                    };
            }
            else
            {
                item = new AnimationDataItem()
                {
                    StartTime = frame.StartTime,
                    EndTime = PuppeteerDefaults.StartDate.AddHours(animationEndTimeInSeconds)
                };
            }
            dataCollection.Add(item);
        }

        public ObservableCollection<ITimeLineDataItem> GetCollectionBoundToActor(BoneActor actor)
        {
            if (_mapper.ContainsKey(actor))
                return _mapper[actor];
            return null;
        }

        public ITimeLineDataItem GetFrameAt(BoneActor actor, double seconds)
        {
            ObservableCollection<ITimeLineDataItem> items = GetCollectionBoundToActor(actor);
            if (items == null) return null;
            DateTime convertedSeconds = PuppeteerDefaults.StartDate.AddHours(seconds);
            return items.FirstOrDefault(i => i.IsIntervalIntesected(convertedSeconds));
        }

        private AnimationDataItem GetLastAnimationFrame(ObservableCollection<ITimeLineDataItem> items)
        {
            if (!items.Any()) return null;
            return items.MaxBy(i => i.EndTime) as AnimationDataItem;
        }
    }
}
