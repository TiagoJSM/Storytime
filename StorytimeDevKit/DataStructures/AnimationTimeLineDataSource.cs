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

namespace StoryTimeDevKit.DataStructures
{
    public class AnimationTimeLineDataSource
    {
        //private Dictionary<BoneActor, BoneState> _boneInitialStateMapper;
        private Dictionary<BoneActor, ObservableCollection<TimeFrame>> _timeFramesMapper;
        private Skeleton _skeleton;

        public SkeletonAnimation Animation { get; private set; }

        public AnimationTimeLineDataSource(Skeleton skeleton)
        {
            _skeleton = skeleton;
            _timeFramesMapper = new Dictionary<BoneActor, ObservableCollection<TimeFrame>>();
            //_boneInitialStateMapper = new Dictionary<BoneActor, BoneState>();
            Animation = new SkeletonAnimation(skeleton);
        }

        public void AddTimeLineFor(BoneActor actor)
        {
            _timeFramesMapper.Add(actor, new ObservableCollection<TimeFrame>());
            //AddBoneInitialSate(actor);
        }

        /*public void AddBoneInitialSate(BoneActor actor)
        {
            BoneState state = null;
            if (_boneInitialStateMapper.TryGetValue(actor, out state))
            {
                state.Translation = actor.AssignedBone.Translation;
                state.Rotation = actor.AssignedBone.Rotation;
            }
            else
            {
                state = GetStateFromActor(actor);
                _boneInitialStateMapper.Add(actor, state);
            }
        }*/

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
            //_boneInitialStateMapper = new Dictionary<BoneActor, BoneState>();
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

            //var currentState = GetStateFromActor(actor);

            BoneAnimationTimeFrameModel item = null;
            if (frame == null)
            {
                item = new BoneAnimationTimeFrameModel()
                {
                    StartTime = new TimeSpan(),
                    EndTime = TimeSpan.FromSeconds(animationEndTimeInSeconds),
                    StartState = new BoneState()
                    {
                        Rotation = fromState.Rotation,
                        Translation = fromState.Translation
                    },
                    EndState = new BoneState()
                    {
                        Rotation = toState.Rotation,
                        Translation = toState.Translation
                    }
                };
            }
            else
            {
                item = new BoneAnimationTimeFrameModel()
                {
                    StartTime = frame.EndTime,
                    EndTime = TimeSpan.FromSeconds(animationEndTimeInSeconds),
                    StartState = frame.EndState,
                    EndState = new BoneState()
                    {
                        Rotation = toState.Rotation,
                        Translation = toState.Translation
                    }
                };
            }

            var animationFrame = new BoneAnimationFrame()
                {
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    StartTranslation = item.StartState.Translation,
                    StartRotation = item.StartState.Rotation,
                    EndTranslation = item.EndState.Translation,
                    EndRotation = item.EndState.Rotation
                };
            item.AnimationFrame = animationFrame;
            Animation.AddAnimationFrame(actor.AssignedBone, animationFrame);

            dataCollection.Add(item);
        }

        /*private BoneState GetStateFromActor(BoneActor actor)
        {
            return new BoneState()
            {
                Translation = actor.AssignedBone.Translation,
                Rotation = actor.AssignedBone.Rotation
            };
        }*/

        private void SetActorPropertiesToBoneEndState(BoneAnimationTimeFrameModel frame, BoneActor actor, BoneState toState)
        {
            frame.EndState.Translation = toState.Translation;
            frame.EndState.Rotation = toState.Rotation;

            frame.AnimationFrame.EndTranslation = toState.Translation;
            frame.AnimationFrame.EndRotation = toState.Rotation;

            var items = GetCollectionBoundToActor(actor);
            var nextFrame = GetFrameAfter(items, frame);

            if (nextFrame == null) return;

            nextFrame.AnimationFrame.StartTranslation = toState.Translation;
            nextFrame.AnimationFrame.StartRotation = toState.Rotation;
        }
    }
}
