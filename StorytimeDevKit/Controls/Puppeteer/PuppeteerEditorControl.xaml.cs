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
        public event Action<PuppeteerWorkingMode> OnWorkingModeChanges;

        public PuppeteerEditorControl()
        {
            InitializeComponent();
            AssignPanelEventHandling(PuppeteerEditor);
            Loaded += LoadedHandler;
        }

        public void LoadedHandler(object sender, RoutedEventArgs e)
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
            SelectionMode.IsChecked = true;

            transformModeModel =
                DependencyInjectorHelper
                    .PuppeteerKernel
                    .Get<TransformModeViewModel>();

            TranslateButton.DataContext = transformModeModel;
            RotateButton.DataContext = transformModeModel;
            ScaleButton.DataContext = transformModeModel;
        }

        protected override Scene GetScene()
        {
            return _game.GameWorld.ActiveScene;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (OnUnloaded != null)
                OnUnloaded(this);
        }

        private void RadioButton_Selection_Checked(object sender, RoutedEventArgs e)
        {
            if (OnWorkingModeChanges != null)
                OnWorkingModeChanges(PuppeteerWorkingMode.SelectionMode);
            TranslateButton.IsEnabled = true;
            RotateButton.IsEnabled = true;
            ScaleButton.IsEnabled = true;
        }

        private void SelectionMode_Unchecked(object sender, RoutedEventArgs e)
        {
            TranslateButton.IsEnabled = false;
            RotateButton.IsEnabled = false;
            ScaleButton.IsEnabled = false;
        }

        private void RadioButton_AddBone_Checked(object sender, RoutedEventArgs e)
        {
            if (OnWorkingModeChanges != null)
                OnWorkingModeChanges(PuppeteerWorkingMode.AddBoneMode);
        }
    }
}
