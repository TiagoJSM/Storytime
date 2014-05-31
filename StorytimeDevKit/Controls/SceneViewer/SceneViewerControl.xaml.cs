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

namespace StoryTimeDevKit.Controls.SceneViewer
{
    /// <summary>
    /// Interaction logic for SceneViewerControl.xaml
    /// </summary>
    public partial class SceneViewerControl : UserControl, ISceneViewerControl
    {
        private MyGame _game;
        private ISceneViewerController _controller;

        private ObservableCollection<SceneTabViewModel> Tabs { get; set; }

        public SceneViewerControl()
        {
            Tabs = new ObservableCollection<SceneTabViewModel>();
            
            InitializeComponent();
            ScenesControl.ItemsSource = Tabs;

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            _game = new MyGame(xna.Handle);
            _controller = new SceneViewerController();

            xna.OnDropActor += OnDropActorHandler;
        }

        public void AddScene(SceneViewModel s)
        {
            //build scene according to model
            Scene scene = new Scene();
            scene.SceneName = s.SceneName;
            SceneTabViewModel sceneVM = new SceneTabViewModel(scene);
            Tabs.Add(sceneVM);

            World.Singleton.AddScene(scene);
            World.Singleton.SetActiveScene(scene);
            ScenesControl.SelectedItem = sceneVM;
        }

        private void ScenesControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnDropActorHandler(
            ActorViewModel model, 
            System.Drawing.Point pointInGameWorld, 
            System.Drawing.Point gamePanelDimensions
        )
        {
            SceneTabViewModel sceneVM = ScenesControl.SelectedItem as SceneTabViewModel;
            if(sceneVM == null) return;
            ITexture2D bitmap = _game.GraphicsContext.LoadTexture2D("Bitmap1");
            Static2DRenderableAsset asset = new Static2DRenderableAsset();

            float x = sceneVM.Scene.Camera.Viewport.Width * pointInGameWorld.X / gamePanelDimensions.X;
            float y = sceneVM.Scene.Camera.Viewport.Height * pointInGameWorld.Y / gamePanelDimensions.Y;

            asset.SetBoundingBox(new StoryTimeCore.DataStructures.Rectanglef(x, y, 160));
            asset.Texture2D = bitmap;
            Actor actor = new Actor()
            {
                RenderableActor = asset
            };

            _controller.AddActor(sceneVM.Scene, actor);
        }
    }
}
