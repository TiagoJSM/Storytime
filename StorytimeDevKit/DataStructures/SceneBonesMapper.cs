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
        private Dictionary<BoneActor, Bone> _boneDictionary;

        public SceneBonesMapper()
        {
            _boneDictionary = new Dictionary<BoneActor, Bone>();
        }

        public Bone Add(BoneActor actor)
        {
            Bone parent = null;
            if (actor.Parent != null)
            {
                _boneDictionary.TryGetValue(actor.Parent, out parent);
            }

            Bone bone = new Bone(parent);
            if (parent != null)
            {
                parent.AddChildren(bone);
            }

            SetBoneData(actor, bone);

            //actor.OnPositionChange += OnPositionChangeHandler;
            actor.OnParentChange += OnParentChangeHandler;

            _boneDictionary.Add(actor, bone);
            return bone;
        }

        public Bone GetFromActor(BoneActor actor)
        {
            Bone bone;
            _boneDictionary.TryGetValue(actor, out bone);
            return bone;
        }

        public void SynchronizeBoneChain(Bone bone)
        {
            BoneActor actor = _boneDictionary.GetKeyFromValue(bone);
            SetActorData(actor, bone);
            PropagateBoneChanges(bone);
        }

        private void OnPositionChangeHandler(BoneActor actor)
        {
            Bone bone = _boneDictionary[actor];
            SetBoneData(actor, bone);
            PropagateBoneChanges(bone);
        }

        private void OnParentChangeHandler(BoneActor actor)
        {
            Bone bone = _boneDictionary[actor];
            if (actor.Parent == null)
            {
                if (bone.Parent != null)
                {
                    bone.Parent.RemoveChildren(bone);
                }
                return;
            }

            Bone parentBone = _boneDictionary[actor.Parent];
            if (bone.Parent != null)
            {
                bone.Parent.RemoveChildren(bone);
            }
            parentBone.AddChildren(bone);
        }

        private void SetBoneData(BoneActor actor, Bone bone)
        {
            bone.AbsolutePosition = actor.Body.Position;
            bone.AbsoluteEnd = actor.BoneEnd;
        }

        private void PropagateBoneChanges(Bone bone)
        {
            IEnumerable<Bone> children = bone.Children;
            foreach (Bone child in children)
            {
                BoneActor actor = _boneDictionary.GetKeyFromValue(child);
                SetActorData(actor, child);
                PropagateBoneChanges(child);
            }
        }

        private void SetActorData(BoneActor actor, Bone bone)
        {
            actor.Body.Position = bone.AbsolutePosition;
            actor.BoneEnd = bone.AbsoluteEnd;
        }
    }
}
