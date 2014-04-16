using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.CustomAttributes.Editor
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited=true)]
    public class EditableAttribute : Attribute
    {
        public EditableAttribute() { }

        public string EditorGroup { get; set; }
        public string EditorName { get; set; }
    }
}
