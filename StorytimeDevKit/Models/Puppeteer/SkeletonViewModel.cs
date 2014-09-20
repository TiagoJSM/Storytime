using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace StoryTimeDevKit.Models.Puppeteer
{
    public class SkeletonViewModel : BaseViewModel
    {
        public ObservableCollection<BoneViewModel> RootBones { get; set; }

        public SkeletonViewModel()
        {
            RootBones = new ObservableCollection<BoneViewModel>();
        }
    }
}
