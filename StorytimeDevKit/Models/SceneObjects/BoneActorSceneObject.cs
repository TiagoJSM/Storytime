using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Delegates;
using StoryTimeDevKit.Entities.Actors;
using StoryTimeCore.Physics;
using StoryTimeDevKit.DataStructures;
using Puppeteer.Armature;
using StoryTimeDevKit.DataStructures.Factories;

namespace StoryTimeDevKit.Models.SceneObjects
{
    public class BoneActorSceneObject : ISceneObject
    {
        private BoneActor _boneActor;
        private IPuppeteerSceneOjectActionContext _context;
        private Bone _bone;

        public event OnPositionChanges OnPositionChanges;
        public event OnRotationChanges OnRotationChanges;

        public object Object
        {
            get { return _boneActor; }
        }

        public Vector2 Position
        {
            get
            {
                return _boneActor.Body.Position;
            }
        }

        public float Rotation
        {
            get
            {
                return _boneActor.Body.Rotation;
            }
        }

        public BoneActorSceneObject(BoneActor boneActor, IPuppeteerSceneOjectActionContext context)
        {
            _boneActor = boneActor;
            _context = context;
            _bone = _boneActor.AssignedBone;
        }

        private void SynchronizeBones()
        {
            _context.SynchronizeBoneChain(_bone);
        }

        private void AddAnimationFrame()
        {
            _context.AddAnimationFrameFor(_boneActor);
        }

        public void Translate(Vector2 translation)
        {
            _bone.AbsolutePosition = _bone.AbsolutePosition + translation;
            SynchronizeBones();
            AddAnimationFrame(); 
            if (OnPositionChanges != null)
                OnPositionChanges(_boneActor.Body.Position);
        }

        public void Rotate(float rotation)
        {
            _bone.Rotation = _bone.Rotation + rotation;
            SynchronizeBones();
            if (OnRotationChanges != null)
                OnRotationChanges(_boneActor.Body.Rotation);
        }
    }
}
