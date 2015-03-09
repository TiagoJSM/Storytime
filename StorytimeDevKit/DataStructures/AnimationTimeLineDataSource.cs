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
using Puppeteer.Animation;
using Microsoft.Xna.Framework;
using System.Collections.Specialized;

namespace StoryTimeDevKit.DataStructures
{
    public class AnimationTimeLineDataSource
    {
        private Dictionary<BoneActor, ObservableCollection<TimeFrame>> _timeFramesMapper;
        private Skeleton _skeleton;

        public SkeletonAnimation Animation { get; private set; }
        public TimeSpan AnimationTotalTime { get; private set; }

        public AnimationTimeLineDataSource(Skeleton skeleton)
        {
            _skeleton = skeleton;
            _timeFramesMapper = new Dictionary<BoneActor, ObservableCollection<TimeFrame>>();
            Animation = new SkeletonAnimation(skeleton);
        }

        public void AddTimeLineFor(BoneActor actor)
        {
            var timeFramecollection = new ObservableCollection<TimeFrame>();
            timeFramecollection.CollectionChanged += CheckIfAnimationTimeFramesCountChanges;
            _timeFramesMapper.Add(actor, timeFramecollection);
        }

        public void AddAnimationFrame(BoneActor actor, double animationEndTimeInSeconds, BoneState fromState, BoneState toState)
        {
            var frameAtTime = GetFrameAt(actor, animationEndTimeInSeconds);
            if (frameAtTime == null)
            {
                AddNewAnimationFrame(actor, animationEndTimeInSeconds, fromState, toState);
                return;
            }

            if (frameAtTime.EndTime.TotalSeconds != animationEndTimeInSeconds)
            {
                return;
            }

            SetActorPropertiesToBoneEndState(frameAtTime, actor, toState);
        }

        public ObservableCollection<TimeFrame> GetCollectionBoundToActor(BoneActor actor)
        {
            if (_timeFramesMapper.ContainsKey(actor))
                return _timeFramesMapper[actor];
            return null;
        }

        public BoneAnimationTimeFrameModel GetFrameAt(BoneActor actor, double seconds)
        {
            var items = GetCollectionBoundToActor(actor);
            if (items == null) return null;
            var convertedSeconds = TimeSpan.FromSeconds(seconds);
            return items.FirstOrDefault(i => i.IsIntervalIntesected(convertedSeconds, true)) as BoneAnimationTimeFrameModel;
        }

        public bool HasAnimations()
        {
            return _timeFramesMapper.Any(tf => tf.Value.Any());
        }

        public void Clear()
        {
            _timeFramesMapper = new Dictionary<BoneActor, ObservableCollection<TimeFrame>>();
            Animation = new SkeletonAnimation(_skeleton);
        }

        private BoneAnimationTimeFrameModel GetLastTimeFrame(ObservableCollection<TimeFrame> items)
        {
            if (!items.Any()) return null;
            return items.MaxBy(i => i.EndTime) as BoneAnimationTimeFrameModel;
        }

        private BoneAnimationTimeFrameModel GetFrameAfter(ObservableCollection<TimeFrame> items, TimeFrame frame)
        {
            if (!items.Any()) return null;
            return items.FirstOrDefault(i => i.StartTime == frame.EndTime) as BoneAnimationTimeFrameModel;
        }

        private BoneAnimationTimeFrameModel GetFrameBefore(ObservableCollection<TimeFrame> items, TimeFrame frame)
        {
            if (!items.Any()) return null;
            return items.FirstOrDefault(i => i.EndTime == frame.StartTime) as BoneAnimationTimeFrameModel;
        }

        private void AddNewAnimationFrame(BoneActor actor, double animationEndTimeInSeconds, BoneState fromState, BoneState toState)
        {
            var dataCollection = GetCollectionBoundToActor(actor);
            var frame = GetLastTimeFrame(dataCollection);

            var startTime = frame == null ? new TimeSpan() : frame.EndTime;
            var endTime = TimeSpan.FromSeconds(animationEndTimeInSeconds);

            var animationFrame = new BoneAnimationFrame()
            {
                StartTime = startTime,
                EndTime = endTime,
                StartTranslation = frame == null ? fromState.Translation : frame.StartPosition,
                StartRotation = frame == null ? fromState.Rotation : frame.StartRotation,
                EndTranslation = toState.Translation,
                EndRotation = toState.Rotation
            };

            BoneAnimationTimeFrameModel item = new BoneAnimationTimeFrameModel(animationFrame)
            {
                StartTime = startTime,
                EndTime = endTime
            };

            Animation.AddAnimationFrame(actor.AssignedBone, animationFrame);

            dataCollection.Add(item);
        }

        private void SetActorPropertiesToBoneEndState(BoneAnimationTimeFrameModel frame, BoneActor actor, BoneState toState)
        {
            frame.EndTranslation = toState.Translation;
            frame.EndRotation = toState.Rotation;

            frame.AnimationFrame.EndTranslation = toState.Translation;
            frame.AnimationFrame.EndRotation = toState.Rotation;

            var items = GetCollectionBoundToActor(actor);
            var nextFrame = GetFrameAfter(items, frame);

            if (nextFrame == null) return;

            nextFrame.AnimationFrame.StartTranslation = toState.Translation;
            nextFrame.AnimationFrame.StartRotation = toState.Rotation;
        }

        private void CheckIfAnimationTimeFramesCountChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var timeFrames = sender as ObservableCollection<TimeFrame>;
                var boneAnimationMaximum = timeFrames.MaxBy(tf => tf.EndTime);
                if (boneAnimationMaximum.EndTime > AnimationTotalTime)
                    AnimationTotalTime = boneAnimationMaximum.EndTime;

                var newTimeFrames = e.NewItems.Cast<TimeFrame>();
                foreach (var newTimeFrame in newTimeFrames)
                    newTimeFrame.OnEndTimeChanges += OnTimeFrameEndTimeChangesHandler;
            }
        }

        private void OnTimeFrameEndTimeChangesHandler(TimeFrame timeFrame)
        {
            if (timeFrame.EndTime > AnimationTotalTime)
                AnimationTotalTime = timeFrame.EndTime;
        }
    }
}
