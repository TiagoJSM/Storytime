using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;
using StoryTimeCore.DataStructures;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;

namespace StoryTimeFramework.Entities.Components
{
    public abstract class Component : WorldEntity, IRenderable
    {
        public BaseActor OwnerActor { get; set; }
        public bool RenderInGame { get; protected set; }
        public override AxisAlignedBoundingBox2D AABoundingBox
        {
            get
            {
                return BoundingBox.GetAABoundingBox();
            }
        }

        public override BoundingBox2D BoundingBox
        {
            get 
            {
                if (OwnerActor == null || OwnerActor.Body == null)
                    return RawAABoundingBox.GetBoundingBox2D();

                var position = OwnerActor.Body.Position;
                var rotation = OwnerActor.Body.Rotation;

                var box = RawAABoundingBox.GetBoundingBox2D();
                box.Translate(position);
                return
                    box
                    .GetRotated(rotation, position);
            }
        }
        
        protected abstract AxisAlignedBoundingBox2D RawAABoundingBox { get; }

        public abstract void Render(IRenderer renderer);
    }
}
