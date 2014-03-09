using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Contexts.Interfaces
{
    public interface ITexture2D
    {
        int Id { get; }
        int Width { get; }
        int Height { get; }
        string TextureName { get; }
    }
}
