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

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for SingleTimeLineControl.xaml
    /// </summary>
    public partial class SingleTimeLineControl : UserControl
    {
        public SingleTimeLineControl()
        {
            InitializeComponent();
            ObservableCollection<ITimeLineDataItem> t2Data = new ObservableCollection<ITimeLineDataItem>();
            var tmp1 = new AnimationDataItem()
            {
                StartTime = new DateTime(1),
                EndTime = new DateTime(1).AddHours(10),
                Name = "Temp 1"
            };

            t2Data.Add(tmp1);
            TimeLine.Items = t2Data;
            TimeLine.StartDate = PuppeteerDefaults.StartDate;
        }
    }
}
