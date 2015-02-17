using StoryTimeDevKit.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedAnimatedSkeletonFile
    {
        public SavedAnimatedSkeleton SavedAnimatedSkeleton { get; set; }
        public string FileName
        {
            get
            {
                return string.Concat(FileNameWithoutExtension, FilesExtensions.SavedAnimatedSkeleton);
            }
        }
        public string FileNameWithoutExtension { get; set; }
        public string RelativePath
        {
            get
            {
                return string.Concat(RelativePaths.BoneAnimation, "/", FileName);
            }
        }
    }
}
