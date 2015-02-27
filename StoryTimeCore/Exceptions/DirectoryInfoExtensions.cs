using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StoryTimeCore.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static FileInfo[] GetFilesByExtension(this DirectoryInfo di, string extension)
        {
            return di.GetFilesByExtension(extension, SearchOption.TopDirectoryOnly);
        }

        public static FileInfo[] GetFilesByExtension(this DirectoryInfo di, string extension, SearchOption option)
        {
            return di.GetFiles(string.Format("*{0}", extension), option);
        }

        public static IEnumerable<FileInfo> GetFilesByExtension(this DirectoryInfo di, params string[] extension)
        {
            return extension.SelectMany(e => di.GetFilesByExtension(e));
        }
    }
}
