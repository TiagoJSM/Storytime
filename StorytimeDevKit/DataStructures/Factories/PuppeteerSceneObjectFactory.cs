using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeDevKit.Models.SceneObjects;
using Microsoft.Xna.Framework;
using Puppeteer.Armature;
using Puppeteer.Resources;

namespace StoryTimeDevKit.DataStructures.Factories
{
    public class PuppeteerSceneObjectFactory : TypeConfigurableSceneObjectFactory
    {
        private BoneMapper _boneMapper;

        public PuppeteerSceneObjectFactory(BoneMapper boneMapper)
            : base()
        {
            _boneMapper = boneMapper;
            SceneObjectMapper.Add(typeof(BoneActor), o => new BoneActorSceneObject((BoneActor)o, _boneMapper));
            SceneObjectMapper.Add(typeof(BoneAttachedRenderableAsset), o => new BoneAttachedAssetSceneObject((BoneAttachedRenderableAsset)o));
        }
    }
}
