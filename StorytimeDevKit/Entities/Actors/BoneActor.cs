using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeCore.DataStructures;
using StoryTimeDevKit.Entities.Renderables.Puppeteer;
using Puppeteer.Armature;

namespace StoryTimeDevKit.Entities.Actors
{
    public delegate void OnPositionChange(BoneActor bone);
    public delegate void OnParentChange(BoneActor bone);

    public class BoneActor : BaseActor
    {
        private BoneActor _parent;
        private Vector2 _parentBoneEndReference;
        private float _parentRotationReference;

        public event OnPositionChange OnPositionChange;
        public event OnParentChange OnParentChange;
        
        public Vector2 BoneEnd
        {
            get 
            {
                var position = Body.Position;
                position.Y += RenderableAsset.AABoundingBox.Height;
                return position.Rotate(Body.Rotation, Body.Position);
            }
            set
            {
                var originalBounds = 
                    RenderableAsset
                    .AABoundingBoxWithoutOrigin
                    .GetScaled(RenderableAsset.Scale.Inverse(), Vector2.Zero);

                var distance = Vector2.Distance(Body.Position, value);
                var angle = value.AngleWithCenterIn(Body.Position) - 90.0f;
                var yScale = distance / originalBounds.Height;
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
                _parent = value;
                if (_parent != null)
                {
                    _parentBoneEndReference = _parent.BoneEnd;
                    _parentRotationReference = _parent.Body.Rotation;
                }
                if (OnParentChange != null)
                    OnParentChange(this);
            }
        }
        public Bone AssignedBone { get; set; }

        public BoneActor()
        {
            OnCreated += OnCreatedHandler;
            OnBoundingBoxChanges += OnBoundingBoxChangesHandler;
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        private void OnCreatedHandler()
        {
            Body = Scene.PhysicalWorld.CreateRectangularBody(160f, 160f, 1f);
            RenderableAsset = new BoneRenderableAsset(Scene.GraphicsContext);
        }

        private void OnBoundingBoxChangesHandler(WorldEntity entity)
        {
            if (OnPositionChange != null)
                OnPositionChange(this);
        }
    }
}
