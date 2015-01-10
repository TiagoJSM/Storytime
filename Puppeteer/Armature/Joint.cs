using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppeteer.Armature
{
    public class Joint
    {
        private List<Bone> _children;

        public Bone Parent { get; set; }
        public IEnumerable<Bone> Children { get { return _children; } }

        public void AddChildren(Bone children)
        {
            if (_children.Contains(children)) return;
            children.BoneStartJoint = this;
        }

        public void RemoveChildren(Bone children)
        {
            if (!_children.Contains(children)) return;
            children.BoneStartJoint = null;
        }
    }
}
