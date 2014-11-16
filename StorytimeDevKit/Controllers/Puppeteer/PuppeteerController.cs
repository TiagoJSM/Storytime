using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers.TemplateControllers;
using StoryTimeDevKit.Controls.Puppeteer;
using StoryTimeDevKit.Models.SceneViewer;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Controllers.Puppeteer.WorkingModes;
using StoryTime;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Resources.Graphic;
using FarseerPhysicsWrapper;
using FarseerPhysics.Factories;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeDevKit.DataStructures;
using StoryTimeDevKit.SceneWidgets.Transformation;
using Ninject;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Entities.SceneWidgets;
using StoryTimeDevKit.Commands.ReversibleCommands;
using Puppeteer.Armature;
using StoryTimeDevKit.Controls;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using Puppeteer.Resources;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public class PuppeteerController 
        :   StackedCommandsController<IPuppeteerEditorControl>, 
            IPuppeteerController, 
            ISkeletonViewerController,
            IPuppeteerWorkingModeContext
    {
        private SkeletonViewModel _skeletonViewModel;
        private Skeleton _skeleton;

        private IPuppeteerEditorControl _puppeteerEditorControl;
        private ISkeletonTreeViewControl _skeletonTreeViewControl;
        private PuppeteerWorkingMode _activeWorkingMode;
        private PuppeteerEditorControlData _puppeteerEdControlData;

        private GameWorld _world;
        private IGraphicsContext GraphicsContext { get { return _world.GraphicsContext; } }
        private Dictionary<PuppeteerWorkingModeType, PuppeteerWorkingMode> _workingModes;

        public Scene Scene { get { return _world.ActiveScene; } }

        public GameWorld GameWorld 
        {
            get
            {
                return _world;
            }
            set
            {
                _world = value;

                Scene scene = new Scene();
                scene.PhysicalWorld = new FarseerPhysicalWorld(Vector2.Zero);
                _world.AddScene(scene);
                _world.SetActiveScene(scene);

                ConfigureSceneUI();
            }
        }
        public IPuppeteerEditorControl PuppeteerControl 
        {
            get
            {
                return _puppeteerEditorControl;
            }
            set
            {
                if (_puppeteerEditorControl == value) return;
                if (_puppeteerEditorControl != null)
                    UnassignPuppeteerEditorControlEvents();
                _puppeteerEditorControl = value;
                if (value != null)
                    AssignPuppeteerEditorControlEvents();
            }
        }
        public object Selected 
        {
            get
            {
                return _puppeteerEdControlData.Selected;
            }
            set
            {
                if (_puppeteerEdControlData.Selected == value) return;
                _puppeteerEdControlData.Selected = value;

                if (_puppeteerEdControlData.Selected != null)
                {
                    TransformModeModel.HasActor = true;
                }
                else
                {
                    TransformModeModel.HasActor = false;
                }
            }
        }

        public ISkeletonTreeViewControl SkeletonTreeViewControl
        {
            get
            {
                return _skeletonTreeViewControl;
            }
            set
            {
                if (_skeletonTreeViewControl == value) return;
                if (_skeletonTreeViewControl != null)
                    UnassignSkeletonTreeViewControlEvents();
                _skeletonTreeViewControl = value;
                if (value != null)
                    AssignSkeletonTreeViewControlEvents();
            }
        }
        public SkeletonViewModel SkeletonViewModel
        {
            get { return _puppeteerEdControlData.SkeletonViewModel; }
        }

        [Inject]
        public TransformModeViewModel TransformModeModel { get; set; }

        public PuppeteerController()
        {
            _workingModes =
                new Dictionary<PuppeteerWorkingModeType, PuppeteerWorkingMode>()
                {
                    { PuppeteerWorkingModeType.BoneSelectionMode, new BoneSelectionMode(this) },
                    { PuppeteerWorkingModeType.AssetSelectionMode, new AssetSelectionMode(this) },
                    { PuppeteerWorkingModeType.AddBoneMode, new AddBoneMode(this) }
                };

            _skeleton = new Skeleton();
        }

        public BoneActor AddBone(Vector2 boneStartPosition)
        {
            BoneActor boneActor = Selected as BoneActor;
            return _puppeteerEdControlData.AddBone(boneStartPosition, boneActor);
        }

        public BoneActor AddBone(Vector2 boneStartPosition, Vector2 boneEndPosition)
        {
            BoneActor boneActor = Selected as BoneActor;
            return _puppeteerEdControlData.AddBone(boneStartPosition, boneEndPosition, boneActor);
        }

        public BaseActor GetIntersectedActor(Vector2 position)
        {
            return Scene.Intersect(position).OfType<BaseActor>().FirstOrDefault();
        }

        public void EnableTransformationUI(bool enable)
        {
            _puppeteerEdControlData.EnableUI = enable;
        }

        private void AssignPuppeteerEditorControlEvents()
        {
            _puppeteerEditorControl.OnLoaded += PuppeteerOnLoadedHandler;
            _puppeteerEditorControl.OnUnloaded += PuppeteerOnUnloadedHandler;
            _puppeteerEditorControl.OnWorkingModeChanges += OnWorkingModeChangesHandler;
            _puppeteerEditorControl.OnMouseClick += OnMouseClickHandler;
            _puppeteerEditorControl.OnMouseDown += OnMouseDownHandler;
            _puppeteerEditorControl.OnMouseUp += OnMouseUpHandler;
            _puppeteerEditorControl.OnMouseMove += OnMouseMoveHandler;
            _puppeteerEditorControl.OnAssetListItemViewModelDrop += OnAssetListItemViewModelDropHandler;
        }

        private void UnassignPuppeteerEditorControlEvents()
        {
            _puppeteerEditorControl.OnLoaded -= PuppeteerOnLoadedHandler;
            _puppeteerEditorControl.OnUnloaded -= PuppeteerOnUnloadedHandler;
            _puppeteerEditorControl.OnWorkingModeChanges -= OnWorkingModeChangesHandler;
            _puppeteerEditorControl.OnMouseClick -= OnMouseClickHandler;
            _puppeteerEditorControl.OnMouseDown -= OnMouseDownHandler;
            _puppeteerEditorControl.OnMouseUp -= OnMouseUpHandler;
            _puppeteerEditorControl.OnMouseMove -= OnMouseMoveHandler;
            _puppeteerEditorControl.OnAssetListItemViewModelDrop -= OnAssetListItemViewModelDropHandler;
        }

        private void AssignSkeletonTreeViewControlEvents()
        {
            _skeletonTreeViewControl.OnLoaded += SkeletonViewerOnLoadedHandler;
            _skeletonTreeViewControl.OnUnloaded += SkeletonViewerOnUnloadedHandler;
        }

        private void UnassignSkeletonTreeViewControlEvents()
        {
            _skeletonTreeViewControl.OnLoaded -= SkeletonViewerOnLoadedHandler;
            _skeletonTreeViewControl.OnUnloaded -= SkeletonViewerOnUnloadedHandler;
        }

        private void PuppeteerOnLoadedHandler(IPuppeteerEditorControl control)
        {
        }

        private void PuppeteerOnUnloadedHandler(IPuppeteerEditorControl control)
        {
            UnassignPuppeteerEditorControlEvents();
        }

        private void OnWorkingModeChangesHandler(PuppeteerWorkingModeType mode)
        {
            if (_activeWorkingMode != null)
                _activeWorkingMode.OnLeaveMode();
            _activeWorkingMode = _workingModes[mode];
            if (_activeWorkingMode != null)
                _activeWorkingMode.OnEnterMode();
        }

        private void SkeletonViewerOnLoadedHandler(ISkeletonTreeViewControl control)
        {
        }

        private void SkeletonViewerOnUnloadedHandler(ISkeletonTreeViewControl control)
        {
            UnassignSkeletonTreeViewControlEvents();
        }

        private void OnMouseClickHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode == null) return;
            _activeWorkingMode.Click(position);
        }

        private void OnMouseMoveHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode != null)
            {
                _activeWorkingMode.MouseMove(position);
            }
        }

        private void OnMouseDownHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode != null)
            {
                _activeWorkingMode.MouseDown(position);
            }
        }

        private void OnMouseUpHandler(Scene scene, Vector2 position)
        {
            if (_activeWorkingMode != null)
            {
                _activeWorkingMode.MouseUp(position);
            }
        }

        private void ConfigureSceneUI()
        {
            TranslateSceneWidget translateWidg = new TranslateSceneWidget();
            Scene.GUI.Children.Add(translateWidg);
            translateWidg.OnTranslationComplete += OnTranslationCompleteHandler;

            RotateSceneWidget rotateWidg = new RotateSceneWidget();
            Scene.GUI.Children.Add(rotateWidg);
            rotateWidg.OnStopRotation += OnStopRotationHandler;

            _puppeteerEdControlData = 
                new PuppeteerEditorControlData(
                    translateWidg, rotateWidg, 
                    TransformModeModel, Scene);
        }

        private void OnTranslationCompleteHandler(Vector2 fromPosition, Vector2 toPosition)
        {
            if (_puppeteerEdControlData.Selected == null) return;
            //IReversibleCommand command = new MoveActorCommand(_puppeteerEdControlData._transformSceneObjectModel.Actor, fromPosition, toPosition);
            //Commands.Push(command);
        }

        private void OnStopRotationHandler(float originalRotation, float finalRotation)
        {
            if (_puppeteerEdControlData.Selected == null) return;
            //IReversibleCommand command = new RotateActorCommand(_puppeteerEdControlData._transformSceneObjectModel.Actor, originalRotation, finalRotation);
            //Commands.Push(command);
        }

        private void OnAssetListItemViewModelDropHandler(AssetListItemViewModel viewModel, Vector2 dropPosition)
        {
            ITexture2D texture = Scene.GraphicsContext.CreateTexture2D(viewModel.FullPath);
            BoneAttachedRenderableAsset asset = new BoneAttachedRenderableAsset() 
            { 
                Texture = texture,
                RenderingOffset = dropPosition
            };
            _puppeteerEdControlData.AddRenderableAsset(asset);
        }
    }
}
