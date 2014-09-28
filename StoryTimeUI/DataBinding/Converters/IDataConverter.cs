using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeUI.DataBinding.Converters
{
    public interface IDataConverter
    {
        object ToDestinationConvertion(object data);
        object ToSourceConvertion(object data);
    }
}
