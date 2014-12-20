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
using TimeLineTool;
using System.Collections.ObjectModel;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Models.Puppeteer;
using StoryTimeDevKit.Extensions;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for SingleTimeLineControl.xaml
    /// </summary>
    public partial class SingleTimeLineControl : UserControl
    {
        public ObservableCollection<ITimeLineDataItem> DataItems 
        {
            get
            {
                return TimeLine.Items;
            }
            set
            {
                if (TimeLine.Items == value) return;
                TimeLine.Items = value;
            }
        }

        public SingleTimeLineControl()
        {
            InitializeComponent();
            /*DataItems = new ObservableCollection<ITimeLineDataItem>();
            var tmp1 = new AnimationDataItem()
            {
                StartTime = PuppeteerDefaults.StartDate,
                EndTime = PuppeteerDefaults.StartDate.AddHours(10),
                Name = "Temp 1"
            };

            DataItems.Add(tmp1);
            TimeLine.Items = DataItems;*/
            TimeLine.StartDate = PuppeteerDefaults.StartDate;
        }

        public AnimationDataItem GetDataItemAt(DateTime time)
        {
            foreach (ITimeLineDataItem item in DataItems)
            {
                if (item.IsIntervalIntesected(time))
                    return item as AnimationDataItem;
            }
            return null;
        }
    }
}
