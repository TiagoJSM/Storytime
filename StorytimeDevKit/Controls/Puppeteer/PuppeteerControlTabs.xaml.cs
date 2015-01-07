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
using StoryTimeDevKit.Controllers.Puppeteer;
using System.ComponentModel;
using StoryTimeDevKit.Utils;
using Ninject;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for PuppeteerControlTabs.xaml
    /// </summary>
    public partial class PuppeteerControlTabs : UserControl
    {
        private IAnimationTimeLineController _timelineController;

        public PuppeteerControlTabs()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _timelineController =
                DependencyInjectorHelper
                    .PuppeteerKernel
                    .Get<IAnimationTimeLineController>();
        }

        private void PlayStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayStopButton.IsChecked ?? false)
            {
                _timelineController.TimeLineControl.ResetAnimation();
                _timelineController.TimeLineControl.PlayAnimation();
            }
            else
            {
                _timelineController.TimeLineControl.PauseAnimation();
            }
        }
    }
}
