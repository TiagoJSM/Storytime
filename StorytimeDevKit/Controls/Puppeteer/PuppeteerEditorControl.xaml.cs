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

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for PupeteerEditor.xaml
    /// </summary>
    public partial class PuppeteerEditorControl : UserControl, IPuppeteerEditorControl
    {
        private IPuppeteerController _puppeteerController;

        public event Action<IPuppeteerEditorControl> OnLoaded;
        public event Action<IPuppeteerEditorControl> OnUnloaded;
        public event Action<PuppeteerWorkingMode> OnWorkingModeChanges;
        public event Action<System.Drawing.Point, System.Drawing.Point> OnMouseClick;

        public PuppeteerEditorControl()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        public void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            ConstructorArgument windowHandle =
                new ConstructorArgument(
                    ApplicationProperties.IPuppeteerControllerArgName,
                    PuppeteerEditor.Handle);

            _puppeteerController =
                DependencyInjectorHelper
                            .Kernel
                            .Get<IPuppeteerController>(windowHandle);

            _puppeteerController.PuppeteerControl = this;

            if (OnLoaded != null)
                OnLoaded(this);
            SelectionMode.IsChecked = true;
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
        }

        private void RadioButton_AddBone_Checked(object sender, RoutedEventArgs e)
        {
            if (OnWorkingModeChanges != null)
                OnWorkingModeChanges(PuppeteerWorkingMode.AddBoneMode);
        }

        private void PuppeteerEditor_OnMouseClick(
            System.Drawing.Point pointInGamePanel, 
            System.Drawing.Point gamePanelDimensions)
        {
            if (OnMouseClick != null)
                OnMouseClick(pointInGamePanel, gamePanelDimensions);
        }
    }
}
