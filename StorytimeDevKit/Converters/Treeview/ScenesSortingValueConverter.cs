using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Collections;
using StoryTimeDevKit.Models.GameObjectsTreeViewModels;

namespace StoryTimeDevKit.Converters.Treeview
{
    [ValueConversion(typeof(IList), typeof(IEnumerable))]
    public class ScenesSortingValueConverter : IValueConverter
    {
        private class SortByTypeComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x.GetType() == typeof(FolderViewModel) && y.GetType() == typeof(SceneViewModel))
                    return -1;
                else if (x.GetType() == typeof(SceneViewModel) && y.GetType() == typeof(FolderViewModel))
                    return 1;
                else if (x.GetType() == typeof(FolderViewModel) && y.GetType() == typeof(FolderViewModel))
                {
                    var folderX = x as FolderViewModel;
                    var folderY = y as FolderViewModel;
                    return String.Compare(folderX.FolderName, folderY.FolderName);
                }
                else if (x.GetType() == typeof(SceneViewModel) && y.GetType() == typeof(SceneViewModel))
                {
                    var sceneX = x as SceneViewModel;
                    var sceneY = y as SceneViewModel;
                    return String.Compare(sceneX.SceneName, sceneY.SceneName);
                }
                return 0;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as IList;
            var view = new ListCollectionView(collection);
            view.CustomSort = new SortByTypeComparer();
            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
