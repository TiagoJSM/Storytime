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
        public PuppeteerControlTabs()
        {
            InitializeComponent();

        }

        private void PlayStopButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (PlayStopButton.IsChecked ?? false)
            {
                TimeLines.ResetAnimation();
                TimeLines.PlayAnimation();
            }
            else
            {
                TimeLines.PauseAnimation();
            }
        }

        private void AnimationLoopButton_Click(object sender, RoutedEventArgs e)
        {
            TimeLines.AnimationLoop = AnimationLoopButton.IsChecked ?? false;
        }
    }
}
