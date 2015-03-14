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
using StoryTimeDevKit.Models.Puppeteer;

namespace StoryTimeDevKit.Models.SceneObjects
{
    public class BoneActorSceneObject : ISceneObject
    {
        private BoneActor _boneActor;
        private IPuppeteerSceneOjectActionContext _context;
        private Bone _bone;
        private AnimationTransformation _animationTransform;

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

        public void StartTranslate(Vector2 position)
        {
            _animationTransform = new AnimationTransformation()
            {
                StartPosition = _bone.Translation,
                StartRotation = _bone.Rotation
            };
        }

        public void Translate(Vector2 translation)
        {
            _bone.AbsolutePosition = _bone.AbsolutePosition + translation;
            SynchronizeBones();
            if (OnPositionChanges != null)
                OnPositionChanges(_boneActor.Body.Position);
        }

        public void EndTranslation(Vector2 fromTranslation, Vector2 toTranslation)
        {
            _animationTransform.EndPosition = _bone.Translation;
            _animationTransform.EndRotation = _bone.Rotation;
            _context.AddAnimationFrameFor(_boneActor, _animationTransform);
        }

        public void StartRotation(float originalRotation)
        {
            _animationTransform = new AnimationTransformation()
            {
                StartPosition = _bone.Translation,
                StartRotation = _bone.Rotation
            };
        }

        public void Rotate(float rotation)
        {
            _bone.Rotation = _bone.Rotation + rotation;
            SynchronizeBones();
            if (OnRotationChanges != null)
                OnRotationChanges(_boneActor.Body.Rotation);
        }

        public void EndRotation(float fromRotation, float toRotation, float totalRotation)
        {
            _animationTransform.EndPosition = _bone.Translation;
            _animationTransform.EndRotation = toRotation;
            _animationTransform.ClockwiseRotation = totalRotation < 0;
            _context.AddAnimationFrameFor(_boneActor, _animationTransform);
        }
    }
}
