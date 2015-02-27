using StoryTimeDevKit.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Configurations;

namespace StoryTimeDevKit.Models.SavedData.Bones
{
    public class SavedSkeletonFile
    {
        public SavedSkeleton SavedSkeleton { get; set; }
        public string FileName
        {
            get
            {
                return string.Concat(FileNameWithoutExtension, FilesExtensions.SavedSkeleton);
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
