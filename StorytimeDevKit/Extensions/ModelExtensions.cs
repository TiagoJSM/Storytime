using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.SavedData;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces;

namespace StoryTimeDevKit.Extensions
{
    public static class ModelExtensions
    {
        public static SavedSceneModel ToSaveModel(this Scene scene)
        {
            var model = new SavedSceneModel();
            var sceneActors = new List<SavedSceneActor>();
            //IEnumerable<ActorWidgetAdapter> widgetActors = scene.Actors.OfType<ActorWidgetAdapter>();
            //foreach (ActorWidgetAdapter widget in widgetActors)
            var actors = scene.Actors;
            foreach (var ba in actors)
            {
                //BaseActor ba = widget.BaseActor;
                var t = ba.GetType();
                var sceneActor = new SavedSceneActor()
                {
                     Module = t.Module.Name,
                     ActorType = t.FullName
                };
                sceneActors.Add(sceneActor);
            }
            model.SceneActors = sceneActors.ToArray();
            model.SceneName = scene.SceneName;
            return model;
        }
    }
}
