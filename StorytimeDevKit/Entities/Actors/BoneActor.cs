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
using StoryTimeDevKit.Entities.SceneWidgets.Interfaces.Puppeteer;

namespace StoryTimeDevKit.Entities.Actors
{
    public delegate void OnPositionChange(BoneActor bone);
    public delegate void OnParentChange(BoneActor bone);

    public class BoneActor : BaseActor
    {
        private List<BoneActor> _children;
        private BoneActor _parent;
        private Vector2 _parentBoneEndReference;
        private float _parentRotationReference;

        public event OnPositionChange OnPositionChange;
        public event OnParentChange OnParentChange;
        
        public Vector2 BoneEnd
        {
            get 
            {
                Vector2 position = Body.Position;
                position.Y += RenderableAsset.AABoundingBox.Height;
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
                /*if(_parent != null)
                    _parent.OnPositionChange -= OnParentPositionChangeHandler;*/
                _parent = value;
                if (_parent != null)
                {
                    _parentBoneEndReference = _parent.BoneEnd;
                    _parentRotationReference = _parent.Body.Rotation;
                    //_parent.OnPositionChange += OnParentPositionChangeHandler;
                }
                if (OnParentChange != null)
                    OnParentChange(this);
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
            RenderableAsset = new BoneRenderableAsset(Scene.GraphicsContext);
        }
        private void OnParentPositionChangeHandler(BoneActor boneActor)
        {
            Vector2 translationFromReference = Body.Position - _parentBoneEndReference;
            float rotation = boneActor.Body.Rotation - _parentRotationReference;
            _parentBoneEndReference = boneActor.BoneEnd;
            _parentRotationReference = boneActor.Body.Rotation;
            Body.Position = (boneActor.BoneEnd + translationFromReference);
            Body.Rotation = Body.Rotation + rotation;
            // TODO: fix bug
            //RenderableAsset.Origin = -(Body.Position - _parent.BoneEnd) + (new Vector2(this.RenderableAsset.BoundingBoxWithoutOrigin.Width / 2, 0.0f));

        }

        private void OnBoundingBoxChangesHandler(BaseActor actor)
        {
            if (OnPositionChange != null)
                OnPositionChange(this);
        }
    }
}
