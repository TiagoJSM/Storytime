using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Exceptions
{
    public class ApplicationBaseException : Exception
    {
        public string UserText { get; private set; }
        public string LogText { get; private set; }

        public ApplicationBaseException(string userText, string logText)
        {
            UserText = userText;
            LogText = logText;
        }
    }
}
