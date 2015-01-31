using Puppeteer.Armature;
using StoryTimeDevKit.Configurations;
using StoryTimeDevKit.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Extensions;
using StoryTimeDevKit.Models.SavedData.Bones;

namespace StoryTimeDevKit.DataStructures.Factories
{
    public class SavedPuppeteerLoadedContent
    {
        public SavedSkeleton SavedSkeleton { get; set; }
        public SavedAnimations SavedAnimations { get; set; }
    }

    public class SavedPuppeteerLoadFactory
    {
        private Dictionary<string, Func<FileInfo, SavedPuppeteerLoadedContent>> _fileParsers;

        public SavedPuppeteerLoadFactory()
        {
            _fileParsers = new Dictionary<string, Func<FileInfo, SavedPuppeteerLoadedContent>>()
            {
                {FilesExtensions.SavedSkeleton, LoadSavedSkeleton},
                {FilesExtensions.SavedAnimatedSkeleton, LoadSavedAnimatedSkeleton}
            };
        }

        public SavedPuppeteerLoadedContent Load(FileInfo file)
        {
            if (!_fileParsers.ContainsKey(file.Extension)) return null;
            return _fileParsers[file.Extension](file);
        }

        private SavedPuppeteerLoadedContent LoadSavedSkeleton(FileInfo file)
        {
            var savedSkeleton = PuppeteerUtils.LoadSkeleton(file.FullName);
            return new SavedPuppeteerLoadedContent()
            {
                SavedSkeleton = savedSkeleton
            };
        }

        private SavedPuppeteerLoadedContent LoadSavedAnimatedSkeleton(FileInfo file)
        {
            var savedSkeleton = PuppeteerUtils.LoadAnimatedSkeleton(file.FullName);
            return new SavedPuppeteerLoadedContent()
            {
                SavedSkeleton = savedSkeleton.SavedSkeleton,
                SavedAnimations = savedSkeleton.SavedAnimations
            };
        }
    }
}
