using System;
using System.Linq;
using System.Windows.Controls;
using StoryTimeFramework.WorldManagement;
using StoryTime;
using System.ComponentModel;
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.SceneViewer;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeDevKit.Extensions;
using Ninject;
using StoryTimeDevKit.Configurations;
using Ninject.Parameters;
using StoryTimeDevKit.Utils;
using FarseerPhysicsWrapper;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;
using StoryTimeDevKit.Models;
using StoryTimeUI.DataBinding.Engines;
using StoryTimeDevKit.Delegates;

namespace StoryTimeDevKit.Controls.SceneViewer
{
    /// <summary>
    /// Interaction logic for SceneViewerControl.xaml
    /// </summary>
    public partial class SceneViewerControl : UserControl, ISceneViewerControl
    {
        private MyGame _game;
        private ISceneViewerController _controller;
        private IGraphicsContext _context;
        private Vector2 _clickPosition;

        public event OnDropActor OnDropActor;
        public event OnMouseMove OnMouseMove;
        public event OnMouseClick OnMouseClick;
        public event OnMouseDown OnMouseDown;
        public event OnMouseUp OnMouseUp;
        public event OnSceneAdded OnSceneAdded;

        public event Action<ActorViewModel> OnActorAdded;
        public event Action<BaseActor> OnSelectedActorChange;

        public event OnSceneChanged OnSceneChanged;

        private ObservableCollection<SceneTabViewModel> Tabs { get; set; }
        public RelayCommand RemoveTab { get; set; }
        public SceneTabViewModel SelectedScene
        {
            get
            {
                return ScenesControl.SelectedItem as SceneTabViewModel;
            }
        }

        public SceneViewerControl()
        {
            Tabs = new ObservableCollection<SceneTabViewModel>();

            RemoveTab = new RelayCommand((obj) =>
            {
                Tabs.Remove(obj as SceneTabViewModel);
                //MessageBox.Show("New Folder!");
            });

            InitializeComponent();
            ScenesControl.ItemsSource = Tabs;

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _game = new MyGame(xna.Handle);
            _context = _game.GraphicsContext;

            ConstructorArgument gameWorldArg = 
                new ConstructorArgument(
                    ApplicationProperties.ISceneViewerGameWorldArgName,
                    _game.GameWorld);
            _controller = DependencyInjectorHelper
                            .Kernel
                            .Get<ISceneViewerController>(gameWorldArg);
            _controller.Control = this;
        }

        public void AddScene(SceneViewModel s)
        {
            //build scene according to model
            Scene scene = new Scene();
            scene.SceneName = s.SceneName;
            scene.PhysicalWorld = new FarseerPhysicalWorld(Vector2.Zero);
            SceneTabViewModel sceneVM = new SceneTabViewModel(scene);
            Tabs.Add(sceneVM);

            _game.GameWorld.AddScene(scene);
            _game.GameWorld.SetActiveScene(scene);

            if (OnSceneAdded != null)
                OnSceneAdded(sceneVM);

            ScenesControl.SelectedItem = sceneVM;
        }

        public void SaveSelectedScene()
        {
            SceneTabViewModel model = SelectedScene;
            if(model == null) return;
            _controller.SaveScene(model);
        }

        public void Undo()
        {
            _controller.Undo();
        }

        public void Redo()
        {
            _controller.Redo();
        }

        public int CommandCount
        {
            get { return _controller.CommandCount; }
        }

        public int? CommandIndex
        {
            get { return _controller.CommandIndex; }
        }

        public bool CanUndo
        {
            get { return _controller.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _controller.CanRedo; }
        }

        private void ScenesControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _game.GameWorld.SetActiveScene(SelectedScene.Scene);
            if (OnSceneChanged != null)
                OnSceneChanged(SelectedScene);
        }

        private void xna_OnDropActorHandler(
            ActorViewModel model, 
            System.Drawing.Point pointInGamePanel, 
            System.Drawing.Point gamePanelDimensions
        )
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if(sceneVM == null) return;

            Vector2 position = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnDropActor != null)
                OnDropActor(model, sceneVM, position);

            if (OnActorAdded != null)
                OnActorAdded(model);
        }

        private void xna_OnMouseClick(
            System.Drawing.Point pointInGamePanel,
            System.Drawing.Point gamePanelDimensions)
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            _clickPosition = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseClick != null)
                OnMouseClick(sceneVM, _clickPosition);
        }

        private void xna_OnMouseDown(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            Vector2 clickPosition = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseDown != null)
                OnMouseDown(sceneVM, clickPosition);
        }

        private void xna_OnMouseMove(
            System.Drawing.Point pointInGamePanel, 
            System.Drawing.Point gamePanelDimensions, 
            System.Windows.Forms.MouseButtons buttons)
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            Vector2 point = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseMove != null)
                OnMouseMove(sceneVM, point);
        }

        private void xna_OnMouseUp(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            Vector2 mouseDownPosition = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);

            if (OnMouseUp != null)
                OnMouseUp(sceneVM, mouseDownPosition);
        }
    }
}
