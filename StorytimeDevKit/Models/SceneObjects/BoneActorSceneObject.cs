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

namespace StoryTimeDevKit.Models.SceneObjects
{
    public class BoneActorSceneObject : ISceneObject
    {
        private BoneActor _boneActor;
        private BoneMapper _boneMapper;
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

        public BoneActorSceneObject(BoneActor boneActor, BoneMapper boneMapper)
        {
            _boneActor = boneActor;
            _boneMapper = boneMapper;
            _bone = _boneMapper.GetFromActor(_boneActor);
        }

        private void SynchronizeBones()
        {
            _boneMapper.SynchronizeBoneChain(_bone);
        }


        public void Translate(Vector2 translation)
        {
            _bone.RelativePosition = _bone.RelativePosition + translation;
            SynchronizeBones();
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
