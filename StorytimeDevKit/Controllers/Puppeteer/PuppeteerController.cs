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

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public class PuppeteerController 
        :   StackedCommandsController<IPuppeteerEditorControl>, 
            IPuppeteerController, 
            ISkeletonViewerController
    {
        private SkeletonViewModel _skeleton;

        private IPuppeteerEditorControl _puppeteerEditorControl;
        private ISkeletonTreeViewControl _skeletonTreeViewControl;
        private IPuppeteerWorkingMode _activeWorkingMode;
        BoneActor _actor;

        private GameWorld _world;
        private IGraphicsContext GraphicsContext { get { return _world.GraphicsContext; } }
        private Scene Scene { get { return _world.ActiveScene; } }

        private Dictionary<PuppeteerWorkingMode, IPuppeteerWorkingMode> _workingModes;

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

        public PuppeteerController(GameWorld world)
        {
            _workingModes =
                new Dictionary<PuppeteerWorkingMode, IPuppeteerWorkingMode>()
                {
                    { PuppeteerWorkingMode.SelectionMode, new SelectionMode() },
                    { PuppeteerWorkingMode.AddBoneMode, new AddBoneMode() }
                };

            _world = world;

            Scene scene = new Scene();
            scene.PhysicalWorld = new FarseerPhysicalWorld(Vector2.Zero);
            _world.AddScene(scene);
            _world.SetActiveScene(scene);
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
            //TODO: create a specific actor for this
            _actor = new BoneActor();
            string name = "one";
            _actor.Body = Scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f, name);
            _actor.Body.Position = position;
            Scene.AddActor(_actor);
        }

        private void MoveActor(BaseActor actor, Vector2 fromPosition, Vector2 toPosition)
        {

        }

        private void RotateActor(BaseActor actor, float previousRotation, float rotation)
        {

        }
    }
}
