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
using StoryTimeDevKit.Models.ImageViewer;
using DataVirtualization;
using StoryTimeDevKit.DataStructures.Virtualization;
using StoryTimeDevKit.Controllers.ImageViewer;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace StoryTimeDevKit.Controls.Displayers
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : UserControl, IImageViewerControl
    {
        private IImageViewerController _controller;
        public AsyncVirtualizingCollection<TextureViewModel> Textures { get; private set; } 

        public ImageViewer()
        {
            InitializeComponent();
            
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _controller = new ImageViewerController();

            this.DataContext = _controller.LoadTexturePaths();
        }

        private void stackPanel1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Textures =
                new AsyncVirtualizingCollection<TextureViewModel>(
                    new TextureItemsProvider("C:\\Users\\Tiago\\Pictures")
                    );

            ImagesPanel.ItemsSource = Textures;
        }

    }
}
