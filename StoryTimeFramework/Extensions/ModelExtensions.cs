using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Models.SavedData;
using StoryTimeFramework.WorldManagement;


namespace StoryTimeFramework.Extensions
{
    public static class ModelExtensions
    {
        public static SavedSceneModel ToSaveModel(this Scene scene)
        {
            var model = new SavedSceneModel();
            var sceneActors = new List<SavedSceneActor>();
            //IEnumerable<ActorWidgetAdapter> widgetActors = scene.WorldEntities.OfType<ActorWidgetAdapter>();
            //foreach (ActorWidgetAdapter widget in widgetActors)
            var actors = scene.WorldEntities;
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
