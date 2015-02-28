using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Delegates;
using Microsoft.Xna.Framework;
using Puppeteer.Resources;
using StoryTimeCore.Extensions;

namespace StoryTimeDevKit.Models.SceneObjects
{
    public class BoneAttachedAssetSceneObject : ISceneObject
    {
        private BoneAttachedRenderableAsset _boneAttachedAsset;

        public event OnPositionChanges OnPositionChanges;
        public event OnRotationChanges OnRotationChanges;

        public object Object
        {
            get { return _boneAttachedAsset; }
        }

        public Vector2 Position
        {
            get 
            { 
                /*return _boneAttachedAsset.RenderingOffset;*/
                return _boneAttachedAsset.Transformation.Translation.ToVector2();
            }
        }

        public float Rotation
        {
            get { return _boneAttachedAsset.Rotation; }
        }

        public void StartTranslate(Vector2 position)
        {
        }

        public void Translate(Vector2 translation)
        {
            _boneAttachedAsset.RenderingOffset = _boneAttachedAsset.RenderingOffset + translation;
            if (OnPositionChanges != null)
                OnPositionChanges(_boneAttachedAsset.RenderingOffset);
        }

        public void EndTranslation(Vector2 fromTranslation, Vector2 toTranslation)
        {
        }

        public void StartRotation(float originalRotation)
        {
        }

        public void Rotate(float rotation)
        {
            
            _boneAttachedAsset.Rotation = _boneAttachedAsset.Rotation + rotation;
            if (OnRotationChanges != null)
                OnRotationChanges(_boneAttachedAsset.Rotation);
        }

        public void EndRotation(float fromRotation, float toRotation)
        {
        }

        public BoneAttachedAssetSceneObject(BoneAttachedRenderableAsset boneAttachedAsset)
        {
            _boneAttachedAsset = boneAttachedAsset;
        }
    }
}
