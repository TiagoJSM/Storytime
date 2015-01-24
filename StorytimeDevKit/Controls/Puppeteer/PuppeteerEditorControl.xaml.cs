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
using System.ComponentModel;
using StoryTime;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Utils;
using Ninject;
using Ninject.Parameters;
using StoryTimeDevKit.Configurations;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Extensions;
using StoryTimeDevKit.Models.MainWindow;
using StoryTimeDevKit.Controls.Templates;
using StoryTimeFramework.WorldManagement;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Enums;
using StoryTimeDevKit.Commands.UICommands;
using StoryTimeDevKit.Commands.UICommands.Puppeteer;
using StoryTimeDevKit.DataStructures.Factories;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for PupeteerEditor.xaml
    /// </summary>
    public partial class PuppeteerEditorControl : BaseSceneUserControl, IPuppeteerEditorControl
    {
        private MyGame _game;
        
        private TransformModeViewModel _transformModeModel;
        private PuppeteerWorkingModesModel _workingModesModel;

        public event Action<IPuppeteerEditorControl> OnLoaded;
        public event Action<IPuppeteerEditorControl> OnUnloaded;
        public event Action<PuppeteerWorkingModeType> OnWorkingModeChanges;
        public event OnAssetListItemViewModelDrop OnAssetListItemViewModelDrop;

        public IPuppeteerController PuppeteerController { get; private set; }
        public ICommand SaveSkeletonCommand { get; private set; }
        public ICommand SaveAnimatedSkeletonCommand { get; set; }
        public ICommand LoadSavedPuppeteerItemsCommand { get; set; }

        public PuppeteerEditorControl()
        {
            InitializeComponent();
            AssignPanelEventHandling(PuppeteerEditor);
            PuppeteerEditor.OnDropData += OnDropDataHandler;
            Loaded += LoadedHandler;
            
            #region Commands
            SaveSkeletonCommand = new SaveSkeletonCommand(this, Window.GetWindow(this));
            SaveAnimatedSkeletonCommand = new SaveAnimatedSkeletonCommand(this, Window.GetWindow(this));
            LoadSavedPuppeteerItemsCommand = new LoadSavedPuppeteerItemsCommand(this, Window.GetWindow(this));
            #endregion
        }

        protected override Scene GetScene()
        {
            return _game.GameWorld.ActiveScene;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _game = new MyGame(PuppeteerEditor.Handle);

            var controlArg =
                new ConstructorArgument(
                    ApplicationProperties.IPuppeteerControllerGameWorldArgName,
                    _game.GameWorld);

            PuppeteerController =
                DependencyInjectorHelper
                            .PuppeteerKernel
                            .Get<IPuppeteerController>(controlArg);
            //_puppeteerController.GameWorld = _game.GameWorld;

            PuppeteerController.PuppeteerControl = this;

            if (OnLoaded != null)
                OnLoaded(this);

            SelectBoneMode.IsChecked = true;

            _transformModeModel =
                DependencyInjectorHelper
                    .PuppeteerKernel
                    .Get<TransformModeViewModel>();

            TranslateButton.DataContext = _transformModeModel;
            FreeMovementButton.DataContext = _transformModeModel;
            RotateButton.DataContext = _transformModeModel;
            ScaleButton.DataContext = _transformModeModel;

            _workingModesModel =
                DependencyInjectorHelper
                    .PuppeteerKernel
                    .Get<PuppeteerWorkingModesModel>();

            SelectBoneMode.DataContext = _workingModesModel;
            SelectAssetMode.DataContext = _workingModesModel;
            AddBoneMode.DataContext = _workingModesModel;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (OnUnloaded != null)
                OnUnloaded(this);
        }

        private void RadioButton_SelectBone_Checked(object sender, RoutedEventArgs e)
        {
            if (OnWorkingModeChanges != null)
                OnWorkingModeChanges(PuppeteerWorkingModeType.BoneSelectionMode);
            TransformationButtonEnabled(true);
        }

        private void RadioButton_SelectAsset_Checked(object sender, RoutedEventArgs e)
        {
            if (OnWorkingModeChanges != null)
                OnWorkingModeChanges(PuppeteerWorkingModeType.AssetSelectionMode);
            TransformationButtonEnabled(true);
        }

        private void TransformationButtonEnabled(bool enabled)
        {
            TranslateButton.IsEnabled = enabled;
            RotateButton.IsEnabled = enabled;
            ScaleButton.IsEnabled = enabled;
            FreeMovementButton.IsEnabled = enabled;
        }

        private void RadioButton_AddBone_Checked(object sender, RoutedEventArgs e)
        {
            TransformationButtonEnabled(false);
            if (OnWorkingModeChanges != null)
                OnWorkingModeChanges(PuppeteerWorkingModeType.AddBoneMode);
        }

        private void OnDropDataHandler(object data, System.Drawing.Point positionGameWorld, System.Drawing.Point gamePanelDimensions)
        {
            var model = data as AssetListItemViewModel;
            var scene = GetScene();
            if (scene == null) return;

            var dropPosition = scene.GetPointInGameWorld(positionGameWorld, gamePanelDimensions);

            if (OnAssetListItemViewModelDrop != null && model != null)
                OnAssetListItemViewModelDrop(model, dropPosition);
        }
    }
}
