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

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public class PuppeteerController 
        :   StackedCommandsController<IPuppeteerEditorControl>, 
            IPuppeteerController, 
            ISkeletonViewerController,
            IPuppeteerWorkingModeContext
    {
        private SkeletonViewModel _skeleton;

        private IPuppeteerEditorControl _puppeteerEditorControl;
        private ISkeletonTreeViewControl _skeletonTreeViewControl;
        private IPuppeteerWorkingMode _activeWorkingMode;
        private SceneControlData _sceneControlData;

        private GameWorld _world;
        private IGraphicsContext GraphicsContext { get { return _world.GraphicsContext; } }
        private Dictionary<PuppeteerWorkingMode, IPuppeteerWorkingMode> _workingModes;
        private BoneActor _selectedBone;

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
        public BoneActor SelectedBone 
        {
            get
            {
                return _selectedBone;
            }
            set
            {
                if (_selectedBone == value) return;
                _selectedBone = value;

                if (_selectedBone != null)
                {
                    _sceneControlData.TransformActorModel.Actor = _selectedBone;
                    TransformModeModel.HasActor = true;
                    //if (TransformModeModel.WidgetMode == WidgetMode.None)
                    //    TransformModeModel.WidgetMode = WidgetMode.Translate;
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

        [Inject]
        public TransformModeViewModel TransformModeModel { get; set; }

        public PuppeteerController()
        {
            _workingModes =
                new Dictionary<PuppeteerWorkingMode, IPuppeteerWorkingMode>()
                {
                    { PuppeteerWorkingMode.SelectionMode, new SelectionMode(this) },
                    { PuppeteerWorkingMode.AddBoneMode, new AddBoneMode(this) }
                };
        }

        public BoneActor AddBoneActor(Vector2 boneStartPosition)
        {
            BoneActor bone = new BoneActor();
            bone.Body = Scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f);
            bone.Body.Position = boneStartPosition;
            Scene.AddActor(bone);
            return bone;
        }

        public BoneActor AddBoneActor(Vector2 boneStartPosition, Vector2 boneEndPosition)
        {
            BoneActor bone = AddBoneActor(boneStartPosition);
            bone.BoneEnd = boneEndPosition;
            return bone;
        }

        public BoneActor GetIntersectedBone(Vector2 position)
        {
            return Scene.Intersect(position).OfType<BoneActor>().FirstOrDefault();
        }

        private void AssignPuppeteerEditorControlEvents()
        {
            _puppeteerEditorControl.OnLoaded += PuppeteerOnLoadedHandler;
            _puppeteerEditorControl.OnUnloaded += PuppeteerOnUnloadedHandler;
            _puppeteerEditorControl.OnWorkingModeChanges += OnWorkingModeChangesHandler;
            _puppeteerEditorControl.OnMouseClick += OnMouseClickHandler;
        }

        private void UnassignPuppeteerEditorControlEvents()
        {
            _puppeteerEditorControl.OnLoaded -= PuppeteerOnLoadedHandler;
            _puppeteerEditorControl.OnUnloaded -= PuppeteerOnUnloadedHandler;
            _puppeteerEditorControl.OnWorkingModeChanges -= OnWorkingModeChangesHandler;
            _puppeteerEditorControl.OnMouseClick -= OnMouseClickHandler;
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

        private void OnWorkingModeChangesHandler(PuppeteerWorkingMode mode)
        {
            _activeWorkingMode = _workingModes[mode];
            _activeWorkingMode.Reset();
        }

        private void SkeletonViewerOnLoadedHandler(ISkeletonTreeViewControl control)
        {
        }

        private void SkeletonViewerOnUnloadedHandler(ISkeletonTreeViewControl control)
        {
            UnassignSkeletonTreeViewControlEvents();
        }

        private void OnMouseClickHandler(Vector2 position)
        {
            if (_activeWorkingMode == null) return;
            _activeWorkingMode.Click(position);
        }

        private void MoveActor(BaseActor actor, Vector2 fromPosition, Vector2 toPosition)
        {

        }

        private void RotateActor(BaseActor actor, float previousRotation, float rotation)
        {

        }

        private void ConfigureSceneUI()
        {
            TranslateSceneWidget translateWidg = new TranslateSceneWidget();
            Scene.GUI.Children.Add(translateWidg);
            translateWidg.OnTranslate += OnTranslateHandler;
            translateWidg.OnTranslationComplete += OnTranslationCompleteHandler;

            RotateSceneWidget rotateWidg = new RotateSceneWidget();
            Scene.GUI.Children.Add(rotateWidg);
            rotateWidg.OnRotation += OnRotationHandler;
            rotateWidg.OnStopRotation += OnStopRotationHandler;

            _sceneControlData = new SceneControlData(translateWidg, rotateWidg, TransformModeModel);
        }

        private void OnTranslateHandler(Vector2 translation)
        {
        }

        private void OnTranslationCompleteHandler(Vector2 fromPosition, Vector2 toPosition)
        {
        }

        private void OnRotationHandler(float rotation)
        {
        }

        private void OnStopRotationHandler(float originalRotation, float finalRotation)
        {
        }
    }
}
