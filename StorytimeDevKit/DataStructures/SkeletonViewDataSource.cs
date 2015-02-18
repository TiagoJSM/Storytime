using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeDevKit.Controls;
using System.Windows.Input;
using StoryTimeCore.Extensions;
using System.Collections.ObjectModel;
using Puppeteer.Resources;

namespace StoryTimeDevKit.DataStructures
{
    public class SkeletonViewDataSource
    {
        private const string DefaultName = "Bone";

        private Dictionary<BoneActor, BoneViewModel> _boneActorMapper;
        private Dictionary<BoneAttachedRenderableAsset, AssetViewModel> _boneAttachedMapper;
        private INodeAddedCallback _callback;
        private SkeletonViewModel _skeletonVM;
        private ObservableCollection<AssetViewModel> _renderableAssetsOrder;
        private ICommand _attachToBoneCommand;
        private ArmatureActor _armatureActor;

        public SkeletonViewModel SkeletonViewModel { get { return _skeletonVM; } }
        public ObservableCollection<AssetViewModel> RenderableAssetOrderModels { get { return _renderableAssetsOrder; } }

        public SkeletonViewDataSource(INodeAddedCallback callback, ICommand attachToBoneCommand, ArmatureActor armatureActor)
        {
            _boneActorMapper = new Dictionary<BoneActor, BoneViewModel>();
            _boneAttachedMapper = new Dictionary<BoneAttachedRenderableAsset, AssetViewModel>();
            _callback = callback;
            _skeletonVM = new SkeletonViewModel(_callback);
            _renderableAssetsOrder = new ObservableCollection<AssetViewModel>();
            _attachToBoneCommand = attachToBoneCommand;
            _armatureActor = armatureActor;
        }

        public void AddBone(BoneActor actor)
        {
            BoneViewModel parentVM = null;
            if (actor.Parent != null)
            {
                parentVM = _boneActorMapper[actor.Parent];
            }

            actor.AssignedBone.Name = GenerateName();
            var boneVM = new BoneViewModel(parentVM, _callback, _attachToBoneCommand, actor);
            boneVM.BoneActor = actor;
            _boneActorMapper.Add(actor, boneVM);
            if(parentVM != null)
                parentVM.Children.Add(boneVM);
            else
                SkeletonViewModel.Children.Add(boneVM);
        }

        public BoneViewModel GetBoneViewModelByName(string name)
        {
            return _boneActorMapper.Values.FirstOrDefault(b => b.Name == name);
        }

        public BoneViewModel GetBoneViewModelFromActor(BoneActor actor)
        {
            if (_boneActorMapper.ContainsKey(actor))
            {
                return _boneActorMapper[actor];
            }
            return null;
        }

        public void AddAssetToArmature(AssetViewModel assetVM)
        {
            _boneAttachedMapper.Add(assetVM.Asset, assetVM);
            _armatureActor.SkeletonComponent.Add(assetVM.Asset);
            assetVM.Parent = SkeletonViewModel;
            SkeletonViewModel.Children.Add(assetVM);
            RenderableAssetOrderModels.Add(assetVM);
        }

        public void AttachAssetToBone(BoneAttachedRenderableAsset asset, BoneViewModel model)
        {
            var assetVM = _boneAttachedMapper[asset];
            assetVM.Parent.Children.Remove(assetVM);
            assetVM.Parent = model;
            model.Children.Add(assetVM);
        }

        public void Move(AssetViewModel model, int index)
        {
            _armatureActor.SkeletonComponent.Move(model.Asset, index);
        }

        public void Clear()
        {
            _boneActorMapper = new Dictionary<BoneActor, BoneViewModel>();
            _skeletonVM = new SkeletonViewModel(_callback);
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
