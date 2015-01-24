using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Puppeteer.Armature
{
    public class Skeleton : IEnumerable<Bone>
    {
        private List<Bone> _rootBones;

        public IEnumerable<Bone> RootBones { get { return _rootBones; } }

        public Skeleton()
        {
            _rootBones = new List<Bone>();
        }

        public void AddBone(Bone bone)
        {
            _rootBones.Add(bone);
        }

        public void RemoveBone(Bone bone)
        {
            _rootBones.Remove(bone);
        }

        public IEnumerator<Bone> GetEnumerator()
        {
            return _rootBones.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _rootBones.GetEnumerator();
        }
    }
}
