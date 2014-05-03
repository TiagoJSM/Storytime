using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.GameObjects;
using System.Reflection;
using StoryTimeFramework.Entities.Actors;
using StoryTimeDevKit.Models;
using StoryTimeDevKit.Utils;
using StoryTimeDevKit.Configurations;
using System.IO;

namespace StoryTimeDevKit.Controllers.GameObjects
{
    public class GameObjectsController : IGameObjectsController
    {
        private IGameObjectsControl _control;
        private GameObjectsPathConfiguration _goPathConfig;

        public GameObjectsController()
        {
            _goPathConfig = XMLSerializerUtils
                .DeserializeFromXML<GameObjectsPathConfiguration>(
                    RootConfigFiles.GameObjectsPathName
                );
        }

        public IGameObjectsControl Control
        {
            set { _control = value; }
        }

        public List<GameObjectsActorModel> LoadActors()
        {
            Assembly assembly = Assembly.Load("StoryTimeFramework");
            Type baseType = typeof(BaseActor);
            List<GameObjectsActorModel> actorTypes = assembly.GetTypes()
                .Where(t => t != baseType && baseType.IsAssignableFrom(t))
                .Select(t =>
                {
                    return new GameObjectsActorModel
                    {
                        ActorName = t.Name,
                        AssemblyName = assembly.GetName().Name
                    };
                })
                .ToList();

            return actorTypes;
        }

        public List<GameObjectsTextureModel> LoadTextures()
        {
            List<GameObjectsTextureModel> models = new List<GameObjectsTextureModel>();

            foreach(string path in _goPathConfig.TexturesPaths)
            {
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fis = di.GetFiles("*.jpg", SearchOption.AllDirectories);
                models.AddRange(ConvertIntoTextureModels(fis, di));
            }

            return models;
        }

        private IEnumerable<GameObjectsTextureModel> ConvertIntoTextureModels(FileInfo[] fis, DirectoryInfo di)
        {
            return 
                fis.Select(fi =>
                {
                    return new GameObjectsTextureModel()
                    {
                         Name = fi.Name,
                         RelativePath = fi.DirectoryName.Replace(di.FullName, di.Name)
                    };
                });
        }
    }
}
