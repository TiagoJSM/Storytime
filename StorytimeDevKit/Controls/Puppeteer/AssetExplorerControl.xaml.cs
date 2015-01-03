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
using System.Collections.ObjectModel;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Extensions;
using System.IO;
using StoryTimeDevKit.Resources.Puppeteer;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for AssetExplorer.xaml
    /// </summary>
    public partial class AssetExplorerControl : UserControl, IAssetExplorerControl
    {
        private Point _startPoint;

        public ObservableCollection<AssetListItemViewModel> AssetListItems { get; private set; }

        public AssetExplorerControl()
        {
            InitializeComponent();

            AssetListItems = new ObservableCollection<AssetListItemViewModel>();
        }

        private void AssetsPanel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = false;
        }

        private void AssetsPanel_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var docPaths = (string[])e.Data.GetData(DataFormats.FileDrop);

            var allAreImageFiles = docPaths.Select(d => new FileInfo(d)).All(fi => fi.IsFileImage());

            if (!allAreImageFiles)
            {
                MessageBox.Show(
                    AssetExplorerResources.NotAllAreImageFiles,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            foreach(var docPath in docPaths)
                AssetListItems.Add(new AssetListItemViewModel(docPath));
        }

        private void ListBoxItem_PreviewMouseDown
            (object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _startPoint = e.GetPosition(AssetsPanel);
            }
        }

        private void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    var currentPosition = e.GetPosition(AssetsPanel);

                    if ((Math.Abs(currentPosition.X - _startPoint.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _startPoint.Y) > 10.0))
                    {
                        var draggedItem = AssetsPanel.SelectedItem as AssetListItemViewModel;
                        if (draggedItem != null)
                        {
                            var finalDropEffect =
                                DragDrop.DoDragDrop(
                                    AssetsPanel,
                                    AssetsPanel.SelectedItem,
                                    DragDropEffects.Move);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
