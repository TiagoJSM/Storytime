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
        private Dictionary<BoneActor, BoneState> _boneInitialStateMapper;
        private Dictionary<BoneActor, ObservableCollection<TimeFrame>> _timeFramesMapper;

        public AnimationTimeLineMapper()
        {
            _timeFramesMapper = new Dictionary<BoneActor, ObservableCollection<TimeFrame>>();
            _boneInitialStateMapper = new Dictionary<BoneActor, BoneState>();
        }

        public void AddTimeLineFor(BoneActor actor)
        {
            _timeFramesMapper.Add(actor, new ObservableCollection<TimeFrame>());
            AddBoneInitialSate(actor);
        }

        public void AddBoneInitialSate(BoneActor actor)
        {
            BoneState state = null;
            if (_boneInitialStateMapper.TryGetValue(actor, out state))
            {
                state.Translation = actor.AssignedBone.Translation;
                state.Rotation = actor.AssignedBone.Rotation;
            }
            else
            {
                state = new BoneState() 
                { 
                    Translation = actor.AssignedBone.Translation,
                    Rotation = actor.AssignedBone.Rotation
                };
                _boneInitialStateMapper.Add(actor, state);
            }
        }

        public void AddAnimationFrame(BoneActor actor, double animationEndTimeInSeconds)
        {
            var frameAtTime = GetFrameAt(actor, animationEndTimeInSeconds);
            if (frameAtTime != null) return;

            var dataCollection = GetCollectionBoundToActor(actor);
            var frame = GetLastTimeFrame(dataCollection);

            var currentState = new BoneState()
                {
                    Translation = actor.AssignedBone.Translation,
                    Rotation = actor.AssignedBone.Rotation
                };

            BoneAnimationTimeFrame item = null;
            if (frame == null)
            {
                item = new BoneAnimationTimeFrame()
                    {
                        StartTime = new TimeSpan(),
                        EndTime = TimeSpan.FromSeconds(animationEndTimeInSeconds),
                        StartState = _boneInitialStateMapper[actor],
                        EndState = currentState 
                    };
            }
            else
            {
                item = new BoneAnimationTimeFrame()
                {
                    StartTime = frame.EndTime,
                    EndTime = TimeSpan.FromSeconds(animationEndTimeInSeconds),
                    StartState = frame.EndState,
                    EndState = currentState 
                };
            }
            dataCollection.Add(item);
        }

        public ObservableCollection<TimeFrame> GetCollectionBoundToActor(BoneActor actor)
        {
            if (_timeFramesMapper.ContainsKey(actor))
                return _timeFramesMapper[actor];
            return null;
        }

        public BoneAnimationTimeFrame GetFrameAt(BoneActor actor, double seconds)
        {
            var items = GetCollectionBoundToActor(actor);
            if (items == null) return null;
            var convertedSeconds = TimeSpan.FromSeconds(seconds);
            return items.FirstOrDefault(i => i.IsIntervalIntesected(convertedSeconds, true)) as BoneAnimationTimeFrame;
        }

        private BoneAnimationTimeFrame GetLastTimeFrame(ObservableCollection<TimeFrame> items)
        {
            if (!items.Any()) return null;
            return items.MaxBy(i => i.EndTime) as BoneAnimationTimeFrame;
        }
    }
}
