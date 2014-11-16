using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeDevKit.Controls;

namespace StoryTimeDevKit.DataStructures
{
    public class SkeletonMapper
    {
        private Dictionary<BoneActor, BoneViewModel> _actorDictionary;
        private INodeAddedCallback _callback;
        private SkeletonViewModel _skeletonVM;

        public SkeletonViewModel SkeletonViewModel { get { return _skeletonVM; } }

        public SkeletonMapper(INodeAddedCallback callback)
        {
            _actorDictionary = new Dictionary<BoneActor, BoneViewModel>();
            _callback = callback;
            _skeletonVM = new SkeletonViewModel(_callback);
        }

        public void AddBone(BoneActor actor)
        {
            BoneViewModel parentVM = null;
            if (actor.Parent != null)
            {
                parentVM = _actorDictionary[actor.Parent];
            }

            BoneViewModel boneVM = new BoneViewModel(parentVM, _callback);
            _actorDictionary.Add(actor, boneVM);
            if(parentVM != null)
                parentVM.Children.Add(boneVM);
            else
                SkeletonViewModel.Children.Add(boneVM);
        }
    }
}
