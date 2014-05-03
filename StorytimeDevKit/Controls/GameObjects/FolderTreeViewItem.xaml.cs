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

namespace StoryTimeDevKit.Controls.GameObjects
{
    /// <summary>
    /// Interaction logic for FolderTreeViewItem.xaml
    /// </summary>
    public partial class FolderTreeViewItem : UserControl
    {
        public FolderTreeViewItem()
        {
            InitializeComponent();
        }

        public string FolderName 
        { 
            get { return this.FolderNameTB.Text; }
            set { this.FolderNameTB.Text = value; }
        }

        public ItemCollection Items { get { return TVItem.Items; } }
    }
}
