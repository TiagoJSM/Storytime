using StoryTimeDevKit.Controllers.Puppeteer;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Models.SavedData.Bones;
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
using System.Windows.Shapes;

namespace StoryTimeDevKit.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateSkeletonDialog.xaml
    /// </summary>
    public partial class CreateSkeletonDialog : Window
    {
        public SaveSkeletonDialogModel Model { get; set; }

        public CreateSkeletonDialog()
            : this(new SaveSkeletonDialogModel())
        {
        }

        public CreateSkeletonDialog(SaveSkeletonDialogModel model)
        {
            Model = model;
            DataContext = Model;
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
