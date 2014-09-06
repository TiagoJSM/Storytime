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
using XNAControl;
using StoryTime;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeDevKit.Controls.Dialogs;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Models.SavedData;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Controls.Puppeteer;
using StoryTimeDevKit.SceneWidgets.Interfaces;

namespace StoryTimeDevKit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageViewerDialog _imageViewer;
        private PuppeteerEditorDialog _puppeteerWindow;
        private WidgetMode _mode;

        public RelayCommand SaveScene { get; private set; }
        public RelayCommand SaveAllScenes { get; private set; }

        public RelayCommand OpenImageViewer { get; private set; }
        public RelayCommand Puppeteer { get; private set; }

        public RelayCommand Undo { get; private set; }
        public RelayCommand Redo { get; private set; }

        public RelayCommand TransformMode { get; private set; }

        public MainWindow()
        {
            #region Command Setup
            OpenImageViewer = new RelayCommand(
                (o) =>
                {
                    _imageViewer = new ImageViewerDialog();
                    _imageViewer.Owner = this;
                    _imageViewer.Closed += ImageViewer_Closed;
                    _imageViewer.Show();
                },
                (o) =>
                {
                    return _imageViewer == null;
                }
            );

            SaveScene = new RelayCommand(
                (o) => { SceneViewControl.SaveSelectedScene(); },
                (o) => { return SceneViewControl.SelectedScene != null; }
            );

            SaveAllScenes = new RelayCommand(
                (o) => { },
                (o) => { return true; }
            );

            Undo = new RelayCommand(
                (o) => { SceneViewControl.Undo(); },
                (o) => { return SceneViewControl.CanUndo; }
            );

            Redo = new RelayCommand(
                (o) => { SceneViewControl.Redo(); },
                (o) => { return SceneViewControl.CanRedo; }
            );

            Puppeteer = new RelayCommand(
                (o) =>
                {
                    _puppeteerWindow = new PuppeteerEditorDialog();
                    _puppeteerWindow.Owner = this;
                    _puppeteerWindow.Closed += PuppeteerDialog_Closed;
                    _puppeteerWindow.Show();
                },
                (o) =>
                {
                    return _puppeteerWindow == null;
                }
            );

            TransformMode = new RelayCommand(
                (o) =>
                {
                    _mode = (WidgetMode)o;
                    (SceneViewControl.SelectedActor as ITransformableWidget).WidgetMode = _mode;
                },
                (o) =>
                {
                    if (!(o is WidgetMode)) return false;
                    return SceneViewControl.SelectedActor != null && SceneViewControl.SelectedActor is ITransformableWidget;
                });
            
            #endregion

            InitializeComponent();

            SceneViewControl.OnActorAdded += OnActorAddedHandler;
            
            ApplicationUtils.SetupApplicationFolders();
        }

        private void gameObjectsControl1_OnSceneDoubleClicked(SceneViewModel obj)
        {
            SceneViewControl.AddScene(obj);
        }

        private void ImageViewer_Closed(object sender, EventArgs e)
        {
            _imageViewer = null;
        }

        private void OnActorAddedHandler(ActorViewModel model)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private void PuppeteerDialog_Closed(object sender, EventArgs e)
        {
            _puppeteerWindow = null;
        }

        private void SceneViewControl_OnSelectedActorChange(BaseActor actor)
        {
            ITransformableWidget transformable = actor as ITransformableWidget;
            if (transformable != null)
                transformable.WidgetMode = _mode;
        }
    }
}
