using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Exceptions;

namespace StoryTimeFramework.GenericExceptions
{
    public class InvalidIndexException : StoryTimeException
    {
        private static string _betweenIndexesMessageTemplate = "The index must be between {0} and {1}.";
        
        public InvalidIndexException(int minIndex, int maxIndex)
            : base(string.Format(_betweenIndexesMessageTemplate, minIndex, maxIndex))
        {
        }

    }
}
