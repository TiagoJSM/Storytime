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
using StoryTimeDevKit.Models;
using System.ComponentModel;

namespace StoryTimeDevKit.Controls.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateSceneDialog.xaml
    /// </summary>
    public partial class CreateSceneDialog : Window
    {
        public CreateSceneViewModel Model { get; set; }

        public CreateSceneDialog()
        {
            Model = new CreateSceneViewModel();
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
