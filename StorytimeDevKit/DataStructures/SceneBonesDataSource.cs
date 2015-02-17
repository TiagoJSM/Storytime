using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Actors;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeDevKit.DataStructures
{
    public class SceneBonesDataSource
    {
        private const float FirstBoneDefaultLenght = 100;

        private Dictionary<Bone, BoneActor> _boneDictionary;
        private Skeleton _skeleton;
        private Scene _scene;

        public SceneBonesDataSource(Skeleton skeleton, Scene scene)
        {
            _boneDictionary = new Dictionary<Bone, BoneActor>();
            _skeleton = skeleton;
            _scene = scene;
        }

        public Bone Add(BoneActor actor)
        {
            Bone bone = null;
            if (actor.Parent != null)
            {
                var parent = actor.Parent.AssignedBone;
                bone = new Bone(parent);
            }
            else 
            {
                bone = new Bone()
                {
                    Length = FirstBoneDefaultLenght
                };
                _skeleton.AddBone(bone);
            }

            actor.AssignedBone = bone;
            SetBoneData(actor);

            actor.OnParentChange += OnParentChangeHandler;

            _boneDictionary.Add(bone, actor);
            return bone;
        }

        public BoneActor Add(Vector2 startPosition, Bone parent = null)
        {
            Bone bone = new Bone(parent)
            {
                AbsolutePosition = startPosition,
                Length = FirstBoneDefaultLenght
            };
            if (parent == null)
            {
                _skeleton.AddBone(bone);
            }
            return CreateBoneActorWith(bone);
        }

        public BoneActor Add(Vector2 startPosition, Vector2 endPosition, Bone parent = null)
        {
            Bone bone = new Bone(parent)
            {
                AbsolutePosition = startPosition,
                AbsoluteEnd = endPosition
            };
            if (parent == null)
            {
                _skeleton.AddBone(bone);
            }
            return CreateBoneActorWith(bone);
        }

        public void SynchronizeBoneChain(Bone bone)
        {
            var actor = _boneDictionary[bone];
            SetActorData(actor);
            PropagateBoneChanges(bone);
        }

        public void SynchronizeFullBoneChain()
        {
            foreach (var bone in _skeleton)
            {
                var actor = _boneDictionary[bone];
                SynchronizeBoneChain(actor.AssignedBone);
            }
        }

        public BoneActor GetBoneActorByName(string boneName)
        {
            return _boneDictionary.Where(kvp => kvp.Key.Name == boneName).First().Value;
        }

        public void Clear()
        {
            _boneDictionary = new Dictionary<Bone, BoneActor>();
        }

        private void OnPositionChangeHandler(BoneActor actor)
        {
            SetBoneData(actor);
            PropagateBoneChanges(actor.AssignedBone);
        }

        private void OnParentChangeHandler(BoneActor actor)
        {
            var bone = actor.AssignedBone;
            if (actor.Parent == null)
            {
                if (bone.Parent != null)
                {
                    bone.Parent.RemoveChildren(bone);
                }
                return;
            }

            var parentBone = actor.Parent.AssignedBone;
            if (bone.Parent != null)
            {
                bone.Parent.RemoveChildren(bone);
            }
            parentBone.AddChildren(bone);
        }

        private void SetBoneData(BoneActor actor)
        {
            actor.AssignedBone.AbsolutePosition = actor.Body.Position;
            actor.AssignedBone.AbsoluteEnd = actor.BoneEnd;
        }

        private void PropagateBoneChanges(Bone bone)
        {
            var children = bone.Children;
            foreach (var child in children)
            {
                var actor = _boneDictionary[child];
                SetActorData(actor);
                PropagateBoneChanges(child);
            }
        }

        private void SetActorData(BoneActor actor)
        {
            actor.Body.Position = actor.AssignedBone.AbsolutePosition;
            actor.BoneEnd = actor.AssignedBone.AbsoluteEnd;
        }

        private BoneActor CreateBoneActorWith(Bone bone)
        {
            var actor = _scene.AddWorldEntity<BoneActor>();
            actor.AssignedBone = bone;

            actor.OnParentChange += OnParentChangeHandler;

            _boneDictionary.Add(bone, actor);
            return actor;
        }
    }
}
