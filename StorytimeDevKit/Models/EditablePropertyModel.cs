using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Models
{
    public class EditablePropertyModel
    {
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        public string PropertyGroup { get; set; }
        public object Data { get; set; }
        public Type DataType { get; set; }
    }
}
