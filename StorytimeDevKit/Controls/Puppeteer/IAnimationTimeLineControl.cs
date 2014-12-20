using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Models.Puppeteer;
using System.Collections.ObjectModel;
using TimeLineTool;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    public interface IAnimationTimeLineControl
    {
        event Action<double> OnTimeMarkerChange;

        void AddTimeLine(BoneViewModel bone, ObservableCollection<ITimeLineDataItem> items);
        void AddFrame(BoneViewModel bone, float rotation, Vector2 position);
    }
}
