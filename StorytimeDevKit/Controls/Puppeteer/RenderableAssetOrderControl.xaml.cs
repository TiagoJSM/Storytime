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
using StoryTimeDevKit.Extensions;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Models.Puppeteer;
using System.ComponentModel;
using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Utils;
using Ninject;
using StoryTimeDevKit.Delegates.Puppeteer;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for RenderableAssetOrderControl.xaml
    /// </summary>
    public partial class RenderableAssetOrderControl : UserControl, IRenderableAssetOrderControl
    {
        private const string _format = "myformat";
        private Point _startPosition;
        private ListViewItem _draggedItem;
        private ListViewItem _overTarget;

        private ObservableCollection<AssetViewModel> _data;

        private ISkeletonViewerController _skeletonViewerController;

        public event OnAssetOrderChange OnAssetOrderChange;

        public RenderableAssetOrderControl()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            _skeletonViewerController =
                DependencyInjectorHelper
                    .PuppeteerKernel
                    .Get<ISkeletonViewerController>();

            _skeletonViewerController.RenderableAssetOrderControl = this;
            _data = _skeletonViewerController.RenderableAssetOrderModels;
            base.DataContext = _data;
        }

        private void List_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPosition = e.GetPosition(null);
        }

        private void List_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPosition - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                ListView listView = sender as ListView;
                _draggedItem = ((DependencyObject)e.OriginalSource).FindAnchestor<ListViewItem>();

                var data = listView.ItemContainerGenerator.ItemFromContainer(_draggedItem);

                DataObject dragData = new DataObject(_format, data);
                DragDrop.DoDragDrop(_draggedItem, dragData, DragDropEffects.Move);
            }
        }

        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(_format) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            var dropTarget = ((DependencyObject)e.OriginalSource).FindAnchestor<ListViewItem>();
            if (dropTarget == null) return;
            if (_draggedItem == _overTarget) return;
            if (!e.Data.GetDataPresent(_format)) return;
            if(!_data.Contains(dropTarget.Content)) return;
            var insertIndex = _data.IndexOf(dropTarget.Content as AssetViewModel);
            if(insertIndex < 0) return;

            var asset = e.Data.GetData(_format) as AssetViewModel;
            _data.Remove(asset);
            _data.Insert(insertIndex, asset);
            (_overTarget.Content as AssetViewModel).DragOverTarget = false;
            _overTarget = null;
            if (OnAssetOrderChange != null)
                OnAssetOrderChange(asset, insertIndex);
        }

        private void DragList_DragOver(object sender, DragEventArgs e)
        {
            _overTarget = ((DependencyObject)e.OriginalSource).FindAnchestor<ListViewItem>();
            if (_overTarget == null) return;
            if (_draggedItem == _overTarget) return;
            (_overTarget.Content as AssetViewModel).DragOverTarget = true;
        }

        private void DragList_DragLeave(object sender, DragEventArgs e)
        {
            var dropTarget = ((DependencyObject)e.OriginalSource).FindAnchestor<ListViewItem>();
            if (dropTarget == null) return;
            if (_draggedItem == dropTarget) return;
            (_overTarget.Content as AssetViewModel).DragOverTarget = false;
        }
 
    }
}
