using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Entities.Actors;
using Puppeteer.Armature;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeDevKit.DataStructures
{
    public class SceneBonesMapper
    {
        private Dictionary<Bone, BoneActor> _boneDictionary;

        public SceneBonesMapper()
        {
            _boneDictionary = new Dictionary<Bone, BoneActor>();
        }

        public Bone Add(BoneActor actor)
        {
            Bone bone = null;
            if (actor.Parent != null)
            {
                Bone parent = actor.Parent.AssignedBone;
                bone = new Bone(parent);
            }
            else 
            {
                bone = new Bone();
            }

            actor.AssignedBone = bone;
            SetBoneData(actor);

            actor.OnParentChange += OnParentChangeHandler;

            _boneDictionary.Add(bone, actor);
            return bone;
        }

        public void SynchronizeBoneChain(Bone bone)
        {
            BoneActor actor = _boneDictionary[bone];
            SetActorData(actor);
            PropagateBoneChanges(bone);
        }

        private void OnPositionChangeHandler(BoneActor actor)
        {
            SetBoneData(actor);
            PropagateBoneChanges(actor.AssignedBone);
        }

        private void OnParentChangeHandler(BoneActor actor)
        {
            Bone bone = actor.AssignedBone;
            if (actor.Parent == null)
            {
                if (bone.Parent != null)
                {
                    bone.Parent.RemoveChildren(bone);
                }
                return;
            }

            Bone parentBone = actor.Parent.AssignedBone;
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
            IEnumerable<Bone> children = bone.Children;
            foreach (Bone child in children)
            {
                BoneActor actor = _boneDictionary[child];
                SetActorData(actor);
                PropagateBoneChanges(child);
            }
        }

        private void SetActorData(BoneActor actor)
        {
            actor.Body.Position = actor.AssignedBone.AbsolutePosition;
            actor.BoneEnd = actor.AssignedBone.AbsoluteEnd;
        }
    }
}
