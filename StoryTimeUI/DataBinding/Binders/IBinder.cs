using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StoryTimeUI.DataBinding.Binders
{
    public enum BinderTrigger
    {
        Source,
        Destination
    }
    public interface IBinder
    {
        void Bind(BinderTrigger trigger);
    }
}
