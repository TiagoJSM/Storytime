using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Physics;
using StoryTimeCore.Resources.Graphic;
using StoryTimeFramework.WorldManagement.Manageables;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.DataStructures;
using StoryTimeFramework.Entities.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.CustomAttributes.Editor;
using FarseerPhysics.Dynamics;
using StoryTimeCore.General;
using Microsoft.Xna.Framework;

namespace StoryTimeFramework.Entities.Actors
{
    /// <summary>
    /// The base class for scene actors, this class defines the drawable components for the scene.
    /// </summary>
    public abstract class BaseActor : WorldEntity, ITimeUpdatable, IBoundingBoxable
    {
        private IRenderableAsset _renderableActor;
        private IBody _body;

        public event Action<BaseActor> OnBoundingBoxChanges;

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
                    _body.OnPositionChanges -= BodyPositionChangesHandler;
                if (value != null)
                    value.OnPositionChanges += BodyPositionChangesHandler;
                _body = value;
            }
        }
        [Editable(EditorGroup = "Renderable", EditorName = "Actor")]
        public IRenderableAsset RenderableAsset 
        {
            get
            {
                return _renderableActor;
            }
            set
            {
                if (value == _renderableActor)
                    return;
                if (_renderableActor != null)
                    _renderableActor.OnBoundingBoxChanges -= RenderableActorBoundingBoxChangesHandler;
                if(value != null)
                    value.OnBoundingBoxChanges += RenderableActorBoundingBoxChangesHandler;
                _renderableActor = value;
            }
        }
        public abstract void TimeElapse(WorldTime WTime);
        public Rectanglef BoundingBox
        {
            get 
            {
                Vector2 position;
                if (Body == null)
                    position = Vector2.Zero;
                else
                    position = Body.Position;
                if (RenderableAsset == null)
                    return new Rectanglef(position);
                Rectanglef box = RenderableAsset.BoundingBox;
                box.Translate(position);
                return box;
            }
        }

        private void RenderableActorBoundingBoxChangesHandler(IRenderableAsset asset)
        {
            if (OnBoundingBoxChanges != null)
                OnBoundingBoxChanges(this);
        }

        private void BodyPositionChangesHandler(IBody body)
        {
            if (OnBoundingBoxChanges != null)
                OnBoundingBoxChanges(this);
        }
    }
}
