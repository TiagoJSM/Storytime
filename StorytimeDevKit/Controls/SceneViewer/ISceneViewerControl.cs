using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeDevKit.Controls.SceneViewer
{
    public interface ISceneViewerControl
    {
        void AddScene(Scene s);
    }
}
