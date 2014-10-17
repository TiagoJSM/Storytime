using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeCore.DataStructures;

namespace StoryTimeDevKit.Entities.Actors
{
    public delegate void OnPositionChange(BoneActor bone);

    public class BoneActor : BaseActor
    {
        private List<BoneActor> _children;
        private BoneActor _parent;

        public event OnPositionChange OnPositionChange;
        
        public Vector2 BoneEnd
        {
            get 
            {
                Vector2 position = Body.Position;
                position.Y += RenderableAsset.BoundingBox.Height;
                return position.Rotate(Body.Rotation, Body.Position);
            }
            set
            {
                AxisAlignedBoundingBox2D originalBounds = 
                    RenderableAsset
                    .BoundingBoxWithoutOrigin
                    .GetScaled(RenderableAsset.Scale.Inverse(), Vector2.Zero);

                float distance = Vector2.Distance(Body.Position, value);
                float angle = value.AngleWithCenterIn(Body.Position) - 90.0f;
                float yScale = distance / originalBounds.Height;
                Body.Rotation = angle;
                RenderableAsset.Scale = new Vector2(1.0f, yScale);
            }
        }
        public BoneActor Parent 
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent == value) return;
                if(_parent != null)
                    _parent.OnPositionChange -= OnParentPositionChangeHandler;
                _parent = value;
                if(_parent != null)
                    _parent.OnPositionChange += OnParentPositionChangeHandler;
            }
        }
        public IEnumerable<BoneActor> Children { get { return _children; } }

        public BoneActor()
        {
            _children = new List<BoneActor>();
            OnCreated += OnCreatedHandler;
            OnBoundingBoxChanges += OnBoundingBoxChangesHandler;
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        public void AddChildren(BoneActor bone)
        {
            bone.Parent = this;
            _children.Add(bone);
        }

        private void OnCreatedHandler()
        {
            ITexture2D bitmap = Scene.GraphicsContext.LoadTexture2D("Bone");
            Static2DRenderableAsset asset = new Static2DRenderableAsset();
            asset.Texture2D = bitmap;
            asset.Origin = new Vector2(bitmap.Width / 2, 0.0f);
            RenderableAsset = asset;
        }
        private void OnParentPositionChangeHandler(BoneActor boneActor)
        {
            Body.Position = boneActor.BoneEnd;
        }

        private void OnBoundingBoxChangesHandler(BaseActor actor)
        {
            if (OnPositionChange != null)
                OnPositionChange(this);
        }
    }
}
