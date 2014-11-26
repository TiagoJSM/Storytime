using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeDevKit.Controls;
using System.Windows.Input;
using StoryTimeCore.Extensions;

namespace StoryTimeDevKit.DataStructures
{
    public class SkeletonMapper
    {
        private Dictionary<BoneActor, BoneViewModel> _actorDictionary;
        private INodeAddedCallback _callback;
        private SkeletonViewModel _skeletonVM;
        private ICommand _attachToBoneCommand;

        public SkeletonViewModel SkeletonViewModel { get { return _skeletonVM; } }

        public SkeletonMapper(INodeAddedCallback callback, ICommand attachToBoneCommand)
        {
            _actorDictionary = new Dictionary<BoneActor, BoneViewModel>();
            _callback = callback;
            _skeletonVM = new SkeletonViewModel(_callback);
            _attachToBoneCommand = attachToBoneCommand;
        }

        public void AddBone(BoneActor actor)
        {
            BoneViewModel parentVM = null;
            if (actor.Parent != null)
            {
                parentVM = _actorDictionary[actor.Parent];
            }

            BoneViewModel boneVM = new BoneViewModel(parentVM, _callback, _attachToBoneCommand);
            _actorDictionary.Add(actor, boneVM);
            if(parentVM != null)
                parentVM.Children.Add(boneVM);
            else
                SkeletonViewModel.Children.Add(boneVM);
        }

        public BoneActor GetBoneActorFrom(BoneViewModel model)
        {
            return _actorDictionary.GetKeyFromValue(model);
        }
    }
}
