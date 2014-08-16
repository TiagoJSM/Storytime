using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puppeteer.Armature
{
    public class Skeleton
    {
        private List<Bone> rootBones;

        public IEnumerable<Bone> RootBones { get { return rootBones; } }

        public Skeleton()
        {
            rootBones = new List<Bone>();
        }
    }
}
