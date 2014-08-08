using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StoryTimeFramework.WorldManagement;
using StoryTime;
using System.ComponentModel;
using StoryTimeCore.WorldManagement;
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.SceneViewer;
using StoryTime.Contexts;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Entities.Actors;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeFramework.Entities.Actors;
using FarseerPhysics.Dynamics;
using StoryTimeFramework.Entities.Interfaces;
using StoryTimeDevKit.SceneWidgets;
using StoryTimeDevKit.Extensions;

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
        private ISceneWidget _intersectedActor;
        private ISceneWidget _intersectedActorChild;

        private ObservableCollection<SceneTabViewModel> Tabs { get; set; }
        public RelayCommand RemoveTab { get; set; }
        public SceneTabViewModel SelectedScene
        {
            get
            {
                return ScenesControl.SelectedItem as SceneTabViewModel;
            }
        }

        public event Action<ActorViewModel> OnActorAdded;

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
            _controller = new SceneViewerController(_context);
        }

        public void AddScene(SceneViewModel s)
        {
            //build scene according to model
            Scene scene = new Scene();
            scene.SceneName = s.SceneName;
            SceneTabViewModel sceneVM = new SceneTabViewModel(scene);
            Tabs.Add(sceneVM);

            GameWorld.Singleton.AddScene(scene);
            GameWorld.Singleton.SetActiveScene(scene);
            ScenesControl.SelectedItem = sceneVM;
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

        }

        private void xna_OnDropActorHandler(
            ActorViewModel model, 
            System.Drawing.Point pointInGamePanel, 
            System.Drawing.Point gamePanelDimensions
        )
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if(sceneVM == null) return;

            float x = sceneVM.Scene.Camera.Viewport.Width * pointInGamePanel.X / gamePanelDimensions.X;
            float y = sceneVM.Scene.Camera.Viewport.Height * pointInGamePanel.Y / gamePanelDimensions.Y;
            Vector2 position = new Vector2(x, y);

            _controller.AddActor(sceneVM, model, position);

            if (OnActorAdded != null)
                OnActorAdded(model);
        }

        private void xna_OnMouseClick(
            System.Drawing.Point pointInGamePanel,
            System.Drawing.Point gamePanelDimensions)
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            _clickPosition = GetPointInGameWorld(sceneVM.Scene.Camera, pointInGamePanel, gamePanelDimensions);
            ISceneWidget newIntersectedActor = sceneVM.Scene.Intersect(_clickPosition).FirstOrDefault() as ISceneWidget;
            
            if (newIntersectedActor == null) return;
            if (newIntersectedActor == _intersectedActor) return;

            _controller.SelectWidget(_intersectedActor, newIntersectedActor);
            _intersectedActor = newIntersectedActor;
        }

        private void xna_OnMouseDown(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            if(_intersectedActor == null) return;
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            Vector2 clickPosition = GetPointInGameWorld(sceneVM.Scene.Camera, pointInGamePanel, gamePanelDimensions);
            List<ISceneWidget> children = _intersectedActor.GetAllIntersectedLeafChildren(clickPosition);
            _intersectedActorChild = children.FirstOrDefault();
            if (_intersectedActorChild == null) return;
            _intersectedActorChild.StartDrag(clickPosition);
        }

        private void xna_OnMouseMove(
            System.Drawing.Point pointInGamePanel, 
            System.Drawing.Point gamePanelDimensions, 
            System.Windows.Forms.MouseButtons buttons)
        {
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;
            if (_intersectedActorChild == null) return;

            Vector2 point = GetPointInGameWorld(sceneVM.Scene.Camera, pointInGamePanel, gamePanelDimensions);
            _intersectedActorChild.Drag(Vector2.Zero, point);
        }

        private void xna_OnMouseUp(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            if (_intersectedActorChild == null) return;
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            Vector2 mouseDownPosition = GetPointInGameWorld(sceneVM.Scene.Camera, pointInGamePanel, gamePanelDimensions);
            _intersectedActorChild.StopDrag(mouseDownPosition);
            _intersectedActorChild = null;
        } 

        private Vector2 GetPointInGameWorld(
            ICamera cam,
            System.Drawing.Point pointInGamePanel,
            System.Drawing.Point gamePanelDimensions)
        {
            float x = cam.Viewport.Width * pointInGamePanel.X / gamePanelDimensions.X;
            float y = cam.Viewport.Height * pointInGamePanel.Y / gamePanelDimensions.Y;
            Vector2 position = new Vector2(x, y);
            return position;
        }
    }
}
