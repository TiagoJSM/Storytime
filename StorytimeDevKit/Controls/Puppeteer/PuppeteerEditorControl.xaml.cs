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

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for PupeteerEditor.xaml
    /// </summary>
    public partial class PuppeteerEditorControl : BaseSceneUserControl, IPuppeteerEditorControl
    {
        private MyGame _game;
        private IPuppeteerController _puppeteerController;
        private TransformModeViewModel transformModeModel;

        public event Action<IPuppeteerEditorControl> OnLoaded;
        public event Action<IPuppeteerEditorControl> OnUnloaded;
        public event Action<PuppeteerWorkingModeType> OnWorkingModeChanges;
        public event OnAssetListItemViewModelDrop OnAssetListItemViewModelDrop;

        public PuppeteerEditorControl()
        {
            InitializeComponent();
            AssignPanelEventHandling(PuppeteerEditor);
            PuppeteerEditor.OnDropData += OnDropDataHandler;
            Loaded += LoadedHandler;
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

            _puppeteerController =
                DependencyInjectorHelper
                            .PuppeteerKernel
                            .Get<IPuppeteerController>();
            _puppeteerController.GameWorld = _game.GameWorld;

            _puppeteerController.PuppeteerControl = this;

            if (OnLoaded != null)
                OnLoaded(this);

            SelectBoneMode.IsChecked = true;

            transformModeModel =
                DependencyInjectorHelper
                    .PuppeteerKernel
                    .Get<TransformModeViewModel>();

            TranslateButton.DataContext = transformModeModel;
            RotateButton.DataContext = transformModeModel;
            ScaleButton.DataContext = transformModeModel;
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
        }

        private void RadioButton_AddBone_Checked(object sender, RoutedEventArgs e)
        {
            TransformationButtonEnabled(false);
            if (OnWorkingModeChanges != null)
                OnWorkingModeChanges(PuppeteerWorkingModeType.AddBoneMode);
        }

        private void OnDropDataHandler(object data, System.Drawing.Point positionGameWorld, System.Drawing.Point gamePanelDimensions)
        {
            AssetListItemViewModel model = data as AssetListItemViewModel;
            Scene scene = GetScene();
            if (scene == null) return;

            Vector2 dropPosition = scene.GetPointInGameWorld(positionGameWorld, gamePanelDimensions);

            if (OnAssetListItemViewModelDrop != null && model != null)
                OnAssetListItemViewModelDrop(model, dropPosition);
        }
    }
}
