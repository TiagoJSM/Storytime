using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataVirtualization;
using StoryTimeDevKit.Models.ImageViewer;
using System.IO;

namespace StoryTimeDevKit.DataStructures.Virtualization
{
    public class TextureItemsProvider : IItemsProvider<TextureViewModel>
    {
        private FileInfo[] _fis;
        public TextureItemsProvider(string path)
        {
            var di = new DirectoryInfo(path);
            _fis = di.GetFiles("*.jpg", SearchOption.AllDirectories);
        }

        public int FetchCount()
        {
            return _fis.Length;
        }

        public IList<TextureViewModel> FetchRange(int startIndex, int count)
        {
            if (count >= FetchCount())
                count = FetchCount();

            IList<TextureViewModel> data = new List<TextureViewModel>();
            for (var i = startIndex; i < startIndex + count; i++)
            {
                var fi = _fis[i];
                data.Add(new TextureViewModel(fi.Name, fi.FullName));
            }
            return data;
        }
    }
}
