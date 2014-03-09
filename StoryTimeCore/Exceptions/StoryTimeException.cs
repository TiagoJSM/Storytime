using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Exceptions
{
    /// <summary>
    /// The base class for the exceptions of the StoryTime engine, this class mainly exists to diferenciate between other exception types.
    /// </summary>
    public class StoryTimeException : Exception
    {
        public StoryTimeException() { }
        public StoryTimeException(string message) : base(message) { }
    }
}
