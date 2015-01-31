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

namespace StoryTimeDevKit.Controls.Puppeteer
{
    public class Demo : BaseViewModel
    {
        bool dragOverTarget;
        string name;

        public bool DragOverTarget
        {
            get { return dragOverTarget; }
            set
            {
                dragOverTarget = value;
                base.OnPropertyChanged("DragOverTarget");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                base.OnPropertyChanged("Name");
            }
        }
    }
    /// <summary>
    /// Interaction logic for RenderableAssetOrderControl.xaml
    /// </summary>
    public partial class RenderableAssetOrderControl : UserControl
    {
        private const string _format = "myformat";
        private Point _startPosition;
        private Adorner _adorner;
        private ListViewItem _draggedItem;
        private ListViewItem _overTarget;
        private new ObservableCollection<Demo> data;

        public RenderableAssetOrderControl()
        {
            data = new ObservableCollection<Demo>()
            {
                new Demo{Name="Storm"},
                new Demo{Name="Earth"},
                new Demo{Name="Fire"}
            };
            base.DataContext = data;
            InitializeComponent();
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

                var c = listView.ItemContainerGenerator.ItemFromContainer(_draggedItem);

                DataObject dragData = new DataObject(_format, c);
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
            if(!data.Contains(dropTarget.Content)) return;
            var insertIndex = data.IndexOf(dropTarget.Content as Demo);
            if(insertIndex < 0) return;

            var contact = e.Data.GetData(_format) as Demo;
            data.Remove(contact);
            data.Insert(insertIndex, contact);
            (_overTarget.Content as Demo).DragOverTarget = false;
            _overTarget = null;
        }

        private void DragList_DragOver(object sender, DragEventArgs e)
        {
            _overTarget = ((DependencyObject)e.OriginalSource).FindAnchestor<ListViewItem>();
            if (_overTarget == null) return;
            if (_draggedItem == _overTarget) return;
            (_overTarget.Content as Demo).DragOverTarget = true;
        }

        private void DragList_DragLeave(object sender, DragEventArgs e)
        {
            var dropTarget = ((DependencyObject)e.OriginalSource).FindAnchestor<ListViewItem>();
            if (dropTarget == null) return;
            if (_draggedItem == dropTarget) return;
            (_overTarget.Content as Demo).DragOverTarget = false;
        }
 
    }
}
