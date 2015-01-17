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
    public interface IPuppeteerSceneOjectActionContext
    {
        void SynchronizeBoneChain(Bone bone);
        void AddAnimationFrameFor(BoneActor actor);
    }

    public class PuppeteerSceneObjectFactory : TypeConfigurableSceneObjectFactory
    {
        private SceneBonesDataSource _boneMapper;
        private AnimationTimeLineDataSource _animationMapper;

        private IPuppeteerSceneOjectActionContext _context;

        public PuppeteerSceneObjectFactory(IPuppeteerSceneOjectActionContext context)
            : base()
        {
            _context = context;

            SceneObjectMapper.Add(typeof(BoneActor), o => new BoneActorSceneObject((BoneActor)o, _context));
            SceneObjectMapper.Add(typeof(BoneAttachedRenderableAsset), o => new BoneAttachedAssetSceneObject((BoneAttachedRenderableAsset)o));
        }
    }
}
