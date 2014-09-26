﻿using System;
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
using StoryTimeDevKit.Models.Puppeteer;
using System.ComponentModel;
using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Utils;
using Ninject;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for SkeletonTreeView.xaml
    /// </summary>
    public partial class SkeletonTreeViewControl : UserControl, INodeAddedCallback, ISkeletonTreeViewControl
    {
        private ISkeletonViewerController _skeletonController;

        public event Action<ISkeletonTreeViewControl> OnLoaded;
        public event Action<ISkeletonTreeViewControl> OnUnloaded;

        public SkeletonTreeViewControl()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        public void NodeAddedCallback(Models.GameObjectsTreeViewModels.TreeViewItemViewModel parent, IEnumerable<Models.GameObjectsTreeViewModels.TreeViewItemViewModel> newModels)
        {
            
        }

        public void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _skeletonController =
                DependencyInjectorHelper
                            .Kernel
                            .Get<ISkeletonViewerController>();

            _skeletonController.SkeletonTreeViewControl = this;

            if (OnLoaded != null)
                OnLoaded(this);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (OnUnloaded != null)
                OnUnloaded(this);
        }
    }
}