using Microsoft.Xna.Framework;
using Puppeteer.Animation;
using Puppeteer.Armature;
using StoryTimeDevKit.Models.SavedData.Bones;
using StoryTimeDevKit.Models.SavedData.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Extensions
{
    public static class SkeletonExtensions
    {
        public static SavedSkeleton ToSavedSkeleton(this Skeleton skeleton)
        {
            return new SavedSkeleton()
            {
                RootBones = skeleton.RootBones.ToSavedBones()
            };
        }

        public static SavedBone[] ToSavedBones(this IEnumerable<Bone> bones)
        {
            return bones.Select(ToSavedBone).ToArray();
        }

        public static SavedBone ToSavedBone(this Bone bone)
        {
            return new SavedBone()
            {
                Name = bone.Name,
                AbsolutePosition = new SavedVector2(bone.AbsolutePosition),
                AbsoluteEnd = new SavedVector2(bone.AbsoluteEnd),
                Children = bone.Children.ToSavedBones()
            };
        }

        public static SavedAnimation ToSavedAnimation(this Dictionary<Bone, List<BoneAnimationFrame>> frameMapping)
        {
            return new SavedAnimation()
            {
                BoneAnimationFrames = frameMapping.Select(fm =>
                {
                    return new SavedBoneAnimation()
                    {
                        BoneName = fm.Key.Name,
                        AnimationFrames = fm.Value.Select(baf =>
                        {
                            return new SavedAnimationFrame()
                            {
                                EndRotation = baf.EndRotation,
                                EndTime = baf.EndTime,
                                EndTranslation = baf.EndTranslation,
                                StartRotation = baf.StartRotation,
                                StartTime = baf.StartTime,
                                StartTranslation = baf.StartTranslation
                            };
                        })
                        .ToArray()
                    };
                })
                .ToArray()
            };
        }
    }
}
