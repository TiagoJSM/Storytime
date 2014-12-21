using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.SceneWidgets.Transformation;
using StoryTimeUI.DataBinding.Engines;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Entities.SceneWidgets;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeFramework.WorldManagement;
using Microsoft.Xna.Framework;
using Puppeteer.Armature;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeCore.Resources.Graphic;
using StoryTimeDevKit.DataStructures.Factories;
using Puppeteer.Resources;
using StoryTimeDevKit.Commands.UICommands.Puppeteer;
using StoryTimeDevKit.DataStructures.BindingEngines;
using System.Collections.ObjectModel;
using TimeLineTool;

namespace StoryTimeDevKit.DataStructures
{
    public class PuppeteerEditorControlData : INodeAddedCallback, IPuppeteerSceneOjectActionContext
    {
        private TransformModeViewModel _transformModeModel;
        
        private SceneBonesMapper _sceneBoneMapper;
        private SkeletonTreeViewMapper _skeletonTreeViewMapper;
        private AnimationTimeLineMapper _animationTimeLineMapper;
        
        private Scene _scene;
        private ArmatureActor _armatureActor;

        private TranslateSceneWidget _translateWidget;
        private RotateSceneWidget _rotateWidget;
        private TranslateSceneObjectBindingEngine _translateBindingEngine;
        private RotateSceneObjectBindingEngine _rotateBindingEngine;
        private SceneObjectViewModel _sceneObjectModel;

        private PuppeteerSceneObjectFactory _factory;
        
        public SkeletonViewModel SkeletonViewModel { get { return _skeletonTreeViewMapper.SkeletonViewModel; } }

        public object Selected
        {
            get
            {
                if (_sceneObjectModel.SceneObject == null) return null;
                return _sceneObjectModel.SceneObject.Object;
            }
            set
            {
                if (_sceneObjectModel.SceneObject == value) return;
                if (value == null)
                {
                    _sceneObjectModel.SceneObject = null;
                    return;
                }
                _sceneObjectModel.SceneObject = _factory.CreateSceneObject(value);
            }
        }

        public BoneAttachedRenderableAsset SelectedBoneRenderableAsset 
        { 
            get 
            {
                if (_sceneObjectModel.SceneObject == null) return null;
                return _sceneObjectModel.SceneObject.Object as BoneAttachedRenderableAsset; 
            } 
        }
        public double? Seconds { get; set; }

        public bool EnableUI
        {
            get
            {
                return _sceneObjectModel.Enabled;
            }
            set
            {
                _sceneObjectModel.Enabled = value;
            }
        }

        public PuppeteerEditorControlData(
            TranslateSceneWidget translateWidg, RotateSceneWidget rotateWidg,
            TransformModeViewModel transformModeModel, Scene sene)
        {
            _sceneObjectModel = new SceneObjectViewModel();
            _sceneBoneMapper = new SceneBonesMapper();
            _animationTimeLineMapper = new AnimationTimeLineMapper();

            _translateWidget = translateWidg;
            _rotateWidget = rotateWidg;

            _translateBindingEngine = new TranslateSceneObjectBindingEngine(_translateWidget, _sceneObjectModel);
            _rotateBindingEngine = new RotateSceneObjectBindingEngine(_rotateWidget, _sceneObjectModel);

            _transformModeModel = transformModeModel;
            _transformModeModel.OnWidgetModeChanges += OnWidgetModeChanges;
            _sceneObjectModel.WidgetMode = _transformModeModel.WidgetMode;

            _translateWidget.OnTranslate += OnTranslateHandler;
            _rotateWidget.OnRotation += OnRotateHandler;

            _scene = sene;
            _skeletonTreeViewMapper = new SkeletonTreeViewMapper(this, new AttachToBoneCommand(this));
            _scene.AddActor<ArmatureActor>();

            _factory = new PuppeteerSceneObjectFactory(this);
        }

        public void UnassignEvents()
        {
            _transformModeModel.OnWidgetModeChanges += OnWidgetModeChanges;
        }

        public BoneActor AddBone(Vector2 boneStartPosition, BoneActor parent)
        {
            BoneActor actor = _scene.AddActor<BoneActor>();
            actor.Parent = parent;
            actor.Body.Position = boneStartPosition;
            
            _sceneBoneMapper.Add(actor);
            _skeletonTreeViewMapper.AddBone(actor);
            _animationTimeLineMapper.AddTimeLineFor(actor);
            return actor;
        }

        public BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition, BoneActor parent)
        {
            BoneActor actor = AddBone(boneStartPosition, parent);
            Bone bone = _sceneBoneMapper.GetFromActor(actor);
            bone.AbsoluteEnd = boneEndPosition;
            _sceneBoneMapper.SynchronizeBoneChain(bone);
            return actor;
        }

        public void NodeAddedCallback(TreeViewItemViewModel parent, IEnumerable<TreeViewItemViewModel> newModels)
        {
        }

        public void AddRenderableAsset(BoneAttachedRenderableAsset asset)
        {
            _armatureActor.ArmatureRenderableAsset.Add(asset);
        }

        public Bone GetBoneFrom(BoneViewModel model)
        {
            BoneActor actor = _skeletonTreeViewMapper.GetBoneActorFrom(model);
            return _sceneBoneMapper.GetFromActor(actor);
        }

        public BoneActor GetBoneActorFrom(BoneViewModel model)
        {
            return _skeletonTreeViewMapper.GetBoneActorFrom(model);
        }

        public BoneViewModel GetBoneViewModelByName(string name)
        {
            return _skeletonTreeViewMapper.GetBoneViewModelByName(name);
        }

        public BoneViewModel GetBoneViewModelFromActor(BoneActor actor)
        {
            return _skeletonTreeViewMapper.GetBoneViewModelFromActor(actor);
        }

        public ObservableCollection<ITimeLineDataItem> GetTimeLineFor(BoneActor actor)
        {
            return _animationTimeLineMapper.GetCollectionBoundToActor(actor);
        }

        public void SynchronizeBoneChain(Bone bone)
        {
            _sceneBoneMapper.SynchronizeBoneChain(bone);
        }

        public void AddAnimationFrameFor(BoneActor actor)
        {
            if (Seconds == null) return;
            _animationTimeLineMapper.AddAnimationFrame(actor, GetFromActor(actor), Seconds.Value);
        }

        public Bone GetFromActor(BoneActor actor)
        {
            return _sceneBoneMapper.GetFromActor(actor);
        }

        private void OnWidgetModeChanges(WidgetMode mode)
        {
            _sceneObjectModel.WidgetMode = mode;
        }

        private void OnTranslateHandler(Vector2 translation)
        {
            _sceneObjectModel.SceneObject.Translate(translation);
        }

        private void OnRotateHandler(float rotation)
        {
            _sceneObjectModel.SceneObject.Rotate(rotation);
        }
    }
}
