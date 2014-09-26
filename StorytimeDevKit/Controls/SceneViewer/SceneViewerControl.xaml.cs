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
using StoryTimeDevKit.Controllers.Scenes;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.SceneViewer;
using StoryTime.Contexts;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Commands.UICommands;
using FarseerPhysics.Dynamics;
using StoryTimeFramework.Entities.Interfaces;
using StoryTimeDevKit.Extensions;
using Ninject;
using StoryTimeDevKit.Configurations;
using Ninject.Parameters;
using StoryTimeDevKit.Utils;
using StoryTimeFramework.Extensions;
using FarseerPhysicsWrapper;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;
using StoryTimeUI.DataBinding;
using StoryTimeDevKit.Models;
using StoryTimeUI.DataBinding.Engines;

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
        private ActorWidgetAdapter _intersectedActor;
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
        public BaseActor SelectedActor
        {
            get { return _intersectedActor; }
        }

        public event Action<ActorViewModel> OnActorAdded;
        public event Action<BaseActor> OnSelectedActorChange;

        private SceneWidgets.Transformation.RotateSceneWidget _rotateWidg;
        private SceneWidgets.Transformation.TranslateSceneWidget _translateWidg;
        private BindingEngine<SceneWidgets.Transformation.RotateSceneWidget, TransformActorViewModel> _bindingEngine;
        private BindingEngine<SceneWidgets.Transformation.TranslateSceneWidget, TransformActorViewModel> _translateBindingEngine;

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

            ConstructorArgument graphicsContextArg = 
                new ConstructorArgument(
                    ApplicationProperties.ISceneViewerGraphicsContextArgName, 
                    _context);
            _controller = DependencyInjectorHelper
                            .Kernel
                            .Get<ISceneViewerController>(graphicsContextArg);
            //_rotateWidg = new SceneWidgets.Transformation.RotateSceneWidget();
            //_game.GameWorld.GUI.Children.Add(_rotateWidg);
            _translateWidg = new SceneWidgets.Transformation.TranslateSceneWidget();
            _game.GameWorld.GUI.Children.Add(_translateWidg);
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

            _clickPosition = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);
            ActorWidgetAdapter newIntersectedActor = sceneVM.Scene.Intersect(_clickPosition).FirstOrDefault() as ActorWidgetAdapter;

            _game.GameWorld.GUI.Intersect(_clickPosition);

            if (newIntersectedActor == null) return;
            if (newIntersectedActor == _intersectedActor) return;

            _controller.SelectWidget(_intersectedActor, newIntersectedActor);
            _intersectedActor = newIntersectedActor;

            TransformActorViewModel model = new TransformActorViewModel(_intersectedActor);
            //_bindingEngine= 
            //    new BindingEngine<SceneWidgets.Transformation.RotateSceneWidget, TransformActorViewModel>(
            //        _rotateWidg, model);
            //_bindingEngine.Bind(rw => rw.Position, a => a.Position);

            _translateBindingEngine=
                new BindingEngine<SceneWidgets.Transformation.TranslateSceneWidget, TransformActorViewModel>(
                    _translateWidg, model);
            _translateBindingEngine.Bind(tw => tw.Position, a => a.Position);

            if (OnSelectedActorChange != null)
                OnSelectedActorChange(_intersectedActor);
        }

        private void xna_OnMouseDown(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            if(_intersectedActor == null) return;
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            Vector2 clickPosition = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);
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

            Vector2 point = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);
            _intersectedActorChild.Drag(Vector2.Zero, point);
        }

        private void xna_OnMouseUp(System.Drawing.Point pointInGamePanel, System.Drawing.Point gamePanelDimensions)
        {
            if (_intersectedActorChild == null) return;
            SceneTabViewModel sceneVM = SelectedScene;
            if (sceneVM == null) return;

            Vector2 mouseDownPosition = sceneVM.Scene.GetPointInGameWorld(pointInGamePanel, gamePanelDimensions);
            _intersectedActorChild.StopDrag(mouseDownPosition);
            _intersectedActorChild = null;
        }
    }
}
