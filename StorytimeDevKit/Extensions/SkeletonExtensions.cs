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
                Length = bone.Length,
                Name = bone.Name,
                Rotation = bone.Rotation,
                Translation = new SavedVector2(bone.Translation.X, bone.Translation.Y),
                Children = bone.Children.ToSavedBones()
            };
        }
    }
}
