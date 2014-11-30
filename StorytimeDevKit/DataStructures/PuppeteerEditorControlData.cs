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

namespace StoryTimeDevKit.DataStructures
{
    public class PuppeteerEditorControlData : INodeAddedCallback
    {
        private TransformModeViewModel _transformModeModel;
        private BoneMapper _boneMapper;
        private SkeletonMapper _skeletonMapper;
        private Scene _scene;
        private ArmatureActor _armatureActor;

        private TranslateSceneWidget _translateWidget;
        private RotateSceneWidget _rotateWidget;
        private BindingEngine<TranslateSceneWidget, SceneObjectViewModel> _translateBindingEngine;
        private BindingEngine<RotateSceneWidget, SceneObjectViewModel> _rotateBindingEngine;
        private SceneObjectViewModel _sceneObjectModel;

        private PuppeteerSceneObjectFactory _factory;
        
        public SkeletonViewModel SkeletonViewModel { get { return _skeletonMapper.SkeletonViewModel; } }

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
            _boneMapper = new BoneMapper();
            _translateWidget = translateWidg;
            _rotateWidget = rotateWidg;

            _translateBindingEngine =
                    new BindingEngine<TranslateSceneWidget, SceneObjectViewModel>(
                        _translateWidget, _sceneObjectModel)
                            .Bind(tw => tw.Position, a => a.Position)
                            .Bind(tw => tw.Active, a => a.TranslateWidgetMode)
                            .Bind(tw => tw.Visible, a => a.TranslateWidgetMode);

            _rotateBindingEngine =
                    new BindingEngine<RotateSceneWidget, SceneObjectViewModel>(
                        _rotateWidget, _sceneObjectModel)
                            .Bind(tw => tw.Position, a => a.Position)
                            .Bind(tw => tw.Rotation, a => a.Rotation)
                            .Bind(tw => tw.Active, a => a.RotateWidgetMode)
                            .Bind(tw => tw.Visible, a => a.RotateWidgetMode);

            _transformModeModel = transformModeModel;
            _transformModeModel.OnWidgetModeChanges += OnWidgetModeChanges;
            _sceneObjectModel.WidgetMode = _transformModeModel.WidgetMode;

            _translateWidget.OnTranslate += OnTranslateHandler;
            _rotateWidget.OnRotation += OnRotateHandler;

            _scene = sene;
            _skeletonMapper = new SkeletonMapper(this, new AttachToBoneCommand(this));
            _armatureActor = new ArmatureActor();
            _armatureActor.Body = _scene.PhysicalWorld.CreateRectangularBody(1, 1, 1);
            _scene.AddActor(_armatureActor);

            _factory = 
                new PuppeteerSceneObjectFactory(_boneMapper);
        }

        public void UnassignEvents()
        {
            _transformModeModel.OnWidgetModeChanges += OnWidgetModeChanges;
        }

        public BoneActor AddBone(Vector2 boneStartPosition, BoneActor parent)
        {
            BoneActor actor = new BoneActor();
            actor.Parent = parent;
            actor.Body = _scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f);
            actor.Body.Position = boneStartPosition;
            _scene.AddActor(actor);
            _boneMapper.Add(actor);
            _skeletonMapper.AddBone(actor);
            return actor;
        }

        public BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition, BoneActor parent)
        {
            BoneActor actor = AddBone(boneStartPosition, parent);
            Bone bone = _boneMapper.GetFromActor(actor);
            bone.AbsoluteEnd = boneEndPosition;
            _boneMapper.SynchronizeBoneChain(bone);
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
            BoneActor actor = _skeletonMapper.GetBoneActorFrom(model);
            return _boneMapper.GetFromActor(actor);
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
