using StoryTimeCore.Extensions;
using StoryTimeDevKit.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StoryTimeDevKit.Extensions;
using StoryTimeFramework.Configurations;

namespace StoryTimeDevKit.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for LoadSavedPuppeteerItemsDialog.xaml
    /// </summary>
    public partial class LoadSavedPuppeteerItemsDialog : Window
    {
        private IEnumerable<FileInfo> _fileInfos;

        public FileInfo FileInfo { get; private set; }

        public LoadSavedPuppeteerItemsDialog()
        {
            InitializeComponent();
            //RelativePaths
            var dirInfo = new DirectoryInfo(RelativePaths.BoneAnimation);
            _fileInfos = dirInfo.GetFilesByExtension(FilesExtensions.SavedSkeleton, FilesExtensions.SavedAnimatedSkeleton);
            SavedFiles.ItemsSource = _fileInfos;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SavedFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileInfo = SavedFiles.SelectedItem as FileInfo;
        }
    }
}
