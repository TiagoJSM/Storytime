using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StoryTimeDevKit.Extensions
{
    public static class FileExtensions
    {
        private static string[] _imageExtensions = new [] { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        public static bool IsFileImage(this FileInfo fi)
        {
            return _imageExtensions.Contains(fi.Extension, StringComparer.InvariantCultureIgnoreCase);
        }
    }
}
