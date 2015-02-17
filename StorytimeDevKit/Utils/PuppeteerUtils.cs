using Puppeteer.Armature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Extensions;
using StoryTimeDevKit.Models.SavedData.Bones;
using StoryTimeDevKit.Configurations;

namespace StoryTimeDevKit.Utils
{
    public static class PuppeteerUtils
    {
        public static void SaveSkeleton(SavedSkeletonFile skeletonFile)
        {
            XMLSerializerUtils
                .SerializeToXML<SavedSkeleton>(
                    skeletonFile.SavedSkeleton,
                    skeletonFile.RelativePath);
        }

        public static void SaveAnimatedSkeleton(SavedAnimatedSkeletonFile skeletonFile)
        {
            XMLSerializerUtils
                .SerializeToXML<SavedAnimatedSkeleton>(
                    skeletonFile.SavedAnimatedSkeleton,
                    skeletonFile.RelativePath);
        }

        public static SavedSkeleton LoadSkeleton(string filePath)
        {
            return
                XMLSerializerUtils
                    .DeserializeFromXML<SavedSkeleton>(
                        filePath);
        }

        public static SavedAnimatedSkeleton LoadAnimatedSkeleton(string filePath)
        {
            return
                XMLSerializerUtils
                    .DeserializeFromXML<SavedAnimatedSkeleton>(
                        filePath);
        }
    }
}
