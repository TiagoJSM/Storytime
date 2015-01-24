using Puppeteer.Armature;
using StoryTimeDevKit.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.DataStructures.Factories
{
    public class SavedPuppeteerLoadedContent
    {
        public Skeleton Skeleton { get; set; }
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
            return null;
        }

        private SavedPuppeteerLoadedContent LoadSavedAnimatedSkeleton(FileInfo file)
        {
            return null;
        }
    }
}
