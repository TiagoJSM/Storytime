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
    public class BoneMapper
    {
        private Dictionary<BoneActor, Bone> _boneDictionary;

        public BoneMapper()
        {
            _boneDictionary = new Dictionary<BoneActor, Bone>();
        }

        public void Add(BoneActor actor)
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

            actor.OnPositionChange += OnPositionChangeHandler;
            actor.OnParentChange += OnParentChangeHandler;

            _boneDictionary.Add(actor, bone);
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
            bone.RelativePosition = actor.Body.Position;
            bone.RelativeEnd = actor.BoneEnd;
        }

        private void PropagateBoneChanges(Bone bone)
        {
            IEnumerable<Bone> children = bone.Children;
            foreach (Bone child in children)
            {
                BoneActor actor = _boneDictionary.GetKeyFromValue(child);
                SetActorData(actor, child);
            }
        }

        private void SetActorData(BoneActor actor, Bone bone)
        {
            actor.Body.Position = bone.RelativePosition;
            actor.Body.Rotation = bone.Rotation;
        }
    }
}
