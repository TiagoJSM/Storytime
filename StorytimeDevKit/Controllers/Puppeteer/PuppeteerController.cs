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

        private MyGame _game;
        private IGraphicsContext _graphicsContext;
        private Scene _scene;

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

        public PuppeteerController(IntPtr windowHandle)
        {
            _workingModes =
                new Dictionary<PuppeteerWorkingMode, IPuppeteerWorkingMode>()
                {
                    { PuppeteerWorkingMode.SelectionMode, new SelectionMode() },
                    { PuppeteerWorkingMode.AddBoneMode, new AddBoneMode() }
                };

            _game = new MyGame(windowHandle);
            _graphicsContext = _game.GraphicsContext;

            _scene = new Scene();
            _scene.PhysicalWorld = new FarseerPhysicalWorld(Vector2.Zero);
            _game.GameWorld.AddScene(_scene);
            _game.GameWorld.SetActiveScene(_scene);
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

        private void OnMouseClickHandler(
            System.Drawing.Point pointInGamePanel, 
            System.Drawing.Point gamePanelDimensions)
        {
            //TODO: create a specific actor for this
            Actor actor = new Actor();
            ITexture2D bitmap = _graphicsContext.LoadTexture2D("Bone");
            Static2DRenderableAsset asset = new Static2DRenderableAsset();
            //asset.SetBoundingBox(new Rectanglef(0, 0, 160));
            asset.Texture2D = bitmap;
            actor.RenderableAsset = asset;
            string name = "one";
            actor.Body = _scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f, name);
            float x = _scene.Camera.Viewport.Width * pointInGamePanel.X / gamePanelDimensions.X;
            float y = _scene.Camera.Viewport.Height * pointInGamePanel.Y / gamePanelDimensions.Y;
            Vector2 position = new Vector2(x, y);
            actor.Body.Position = position;
            //ActorWidgetAdapter adapter = new ActorWidgetAdapter(/*this,*/ actor, _graphicsContext);
            //_scene.AddActor(adapter);
            _scene.AddActor(actor);
        }

        private void MoveActor(BaseActor actor, Vector2 fromPosition, Vector2 toPosition)
        {

        }

        private void RotateActor(BaseActor actor, float previousRotation, float rotation)
        {

        }
    }
}
