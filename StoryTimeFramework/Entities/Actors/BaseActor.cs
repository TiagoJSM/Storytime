using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Physics;
using StoryTimeCore.Resources.Graphic;
using StoryTimeFramework.Entities.Components;
using StoryTimeFramework.WorldManagement.Manageables;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.DataStructures;
using StoryTimeCore.DataStructures;
using StoryTimeCore.CustomAttributes.Editor;
using FarseerPhysics.Dynamics;
using StoryTimeCore.General;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeFramework.Entities.Actors
{
    /// <summary>
    /// The base class for scene actors, this class defines the drawable components for the scene.
    /// </summary>
    public abstract class BaseActor : WorldEntity, ITimeUpdatable
    {
        private IRenderableAsset _renderableAsset;
        private IBody _body;

        [Editable(EditorGroup = "Physics", EditorName = "Body")]
        public IBody Body 
        {
            get 
            {
                return _body;
            }
            set
            {
                if (value == _body)
                    return;
                if (_body != null)
                {
                    _body.OnPositionChanges -= BodyPositionChangesHandler;
                    _body.OnRotationChanges -= BodyRotationChangesHandler;
                }
                if (value != null)
                {
                    value.OnPositionChanges += BodyPositionChangesHandler;
                    value.OnRotationChanges += BodyRotationChangesHandler;
                }
                _body = value;
            }
        }
        [Editable(EditorGroup = "Renderable", EditorName = "Actor")]
        public IRenderableAsset RenderableAsset 
        {
            get
            {
                return _renderableAsset;
            }
            set
            {
                if (value == _renderableAsset)
                    return;
                if (_renderableAsset != null)
                    _renderableAsset.OnBoundingBoxChanges -= RenderableActorBoundingBoxChangesHandler;
                if(value != null)
                    value.OnBoundingBoxChanges += RenderableActorBoundingBoxChangesHandler;
                _renderableAsset = value;
            }
        }
        
        public override AxisAlignedBoundingBox2D AABoundingBox
        {
            get 
            {
                var position = Vector2.Zero;
                float rotation = 0;

                if (Body != null)
                {
                    position = Body.Position;
                    rotation = Body.Rotation;
                }
                if (RenderableAsset == null)
                    return new AxisAlignedBoundingBox2D(position);

                var box = RenderableAsset.AABoundingBox;
                box.Translate(position);
                return
                    box
                    .GetRotated(rotation, position);
            }
        }
        public override BoundingBox2D BoundingBox
        {
            get 
            {
                var position = Vector2.Zero;
                float rotation = 0;

                if (Body != null)
                {
                    position = Body.Position;
                    rotation = Body.Rotation;
                }
                if (RenderableAsset == null)
                    return new BoundingBox2D(position);

                var box = RenderableAsset.BoundingBox;
                box.Translate(position);
                return
                    box
                    .GetRotated(rotation, position);
            }
        }

        public ComponentCollection Components { get; private set; }

        public BaseActor()
        {
            Components = new ComponentCollection(this);
        }

        private void RenderableActorBoundingBoxChangesHandler(IRenderableAsset asset)
        {
            RaiseBoundingBoxChanges();
        }

        private void BodyPositionChangesHandler(IBody body)
        {
            RaiseBoundingBoxChanges();
        }

        private void BodyRotationChangesHandler(IBody body)
        {
            RaiseBoundingBoxChanges();
        }

        private void BodyScaleChangesHandler(IBody body)
        {
            RaiseBoundingBoxChanges();
        }
    }
}
