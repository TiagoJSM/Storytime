using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Models.SavedData;
using StoryTimeFramework.WorldManagement;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeDevKit.Extensions
{
    public static class ModelExtensions
    {
        public static SavedSceneModel From(this Scene scene)
        {
            SavedSceneModel model = new SavedSceneModel();
            List<SavedSceneActor> sceneActors = new List<SavedSceneActor>();
            foreach (BaseActor ba in scene.Actors)
            {
                Type t = ba.GetType();
                SavedSceneActor sceneActor = new SavedSceneActor()
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
