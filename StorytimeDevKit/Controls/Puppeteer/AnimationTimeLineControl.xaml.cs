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
using TimeLineTool;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    /// <summary>
    /// Interaction logic for AnimationTimeLine.xaml
    /// </summary>
    public partial class AnimationTimeLineControl : UserControl, IAnimationTimeLine
    {
        public ObservableCollection<SingleTimeLineControl> TimeLines { get; private set; }

        public AnimationTimeLineControl()
        {
            InitializeComponent();

            ObservableCollection<ITimeLineDataItem> t2Data = new ObservableCollection<ITimeLineDataItem>();
            var tmp1 = new TempDataType()
            {
                StartTime = DateTime.Now.AddHours(3),
                EndTime = DateTime.Now.AddHours(18),
                Name = "Temp 1"
            };

            t2Data.Add(tmp1);
            //TimeLine2.Items = t2Data;
            //TimeLine2.StartDate = DateTime.Now;
            //var res = this.FindResource("UsedTemplateProperty") as DataTemplate;
            TimeLines = new ObservableCollection<SingleTimeLineControl>() 
            { 
                new SingleTimeLineControl(),
                new SingleTimeLineControl()
                //new TimeLineControl(){ SynchedWithSiblings = true, Items = t2Data, StartDate = DateTime.Now, ItemTemplate = res },
                //new TimeLineControl(){ SynchedWithSiblings = true } 
            };
        }
    }
}
