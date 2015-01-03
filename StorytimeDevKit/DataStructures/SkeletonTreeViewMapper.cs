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
    public class SkeletonTreeViewMapper
    {
        private const string DefaultName = "Bone";

        private Dictionary<BoneActor, BoneViewModel> _actorDictionary;
        private INodeAddedCallback _callback;
        private SkeletonViewModel _skeletonVM;
        private ICommand _attachToBoneCommand;

        public SkeletonViewModel SkeletonViewModel { get { return _skeletonVM; } }

        public SkeletonTreeViewMapper(INodeAddedCallback callback, ICommand attachToBoneCommand)
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

            var boneVM = new BoneViewModel(parentVM, _callback, _attachToBoneCommand, GenerateName());
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

        public BoneViewModel GetBoneViewModelByName(string name)
        {
            return _actorDictionary.Values.FirstOrDefault(b => b.Name == name);
        }

        public BoneViewModel GetBoneViewModelFromActor(BoneActor actor)
        {
            if (_actorDictionary.ContainsKey(actor))
            {
                return _actorDictionary[actor];
            }
            return null;
        }

        private string GenerateName()
        {
            for (var idx = 1; ; idx++)
            {
                var name = DefaultName + idx;
                if (GetBoneViewModelByName(name) == null)
                    return name;
            }
        }
    }
}
