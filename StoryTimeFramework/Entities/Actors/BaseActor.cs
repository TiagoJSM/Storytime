﻿using System;
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
using StoryTimeCore.Delegates;
using StoryTimeCore.Entities;
using StoryTimeCore.Utils;

namespace StoryTimeFramework.Entities.Actors
{
    /// <summary>
    /// The base class for scene actors, this class defines the drawable components for the scene.
    /// </summary>
    public abstract class BaseActor : WorldEntity, ITimeUpdatable, IComponentOwner
    {
        private IRenderableAsset _renderableAsset;
        private IBody _body;

        public OnTimeElapse OnTimeElapse;
        public OnBodyChanges OnBodyChanges;

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
                if (OnBodyChanges != null)
                    OnBodyChanges(_body);
            }
        }
        [Editable(EditorGroup = "Renderable", EditorName = "Asset")]
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

        public override BoundingBox2D BoundingBox
        {
            get { return Components.BoundingBox; }
        }

        public override AxisAlignedBoundingBox2D AABoundingBox
        {
            get 
            {
                return Components.AABoundingBox;
            }
        }

        public Matrix Transformation
        {
            get 
            {
                if (_body == null)
                {
                    return Matrix.Identity;
                }
                return MatrixUtils.CreateLocalTransformation(_body.Position, Body.Rotation, Vector2.One);
            }
        }

        public ActorComponentCollection Components { get; private set; }

        public BaseActor()
        {
            Components = new ActorComponentCollection(this);
            Components.OnBoundingBoxChanges += OnComponentCollectionBoundingBoxChanges;
        }

        public sealed override void TimeElapse(WorldTime WTime)
        {
            //Components.TimeElapse(WTime);
            if (_renderableAsset != null)
                _renderableAsset.TimeElapse(WTime);
            if (OnTimeElapse != null)
                OnTimeElapse(WTime);
        }

        private void RenderableActorBoundingBoxChangesHandler(IBoundingBoxable boxable)
        {
            RaiseBoundingBoxChanges();
        }

        private void BodyPositionChangesHandler(IBody body)
        {
            RaiseBoundingBoxChanges();
            if (OnBodyChanges != null)
                OnBodyChanges(body);
        }

        private void BodyRotationChangesHandler(IBody body)
        {
            RaiseBoundingBoxChanges();
            if (OnBodyChanges != null)
                OnBodyChanges(body);
        }

        private void BodyScaleChangesHandler(IBody body)
        {
            RaiseBoundingBoxChanges();
            if (OnBodyChanges != null)
                OnBodyChanges(body);
        }

        private void OnComponentCollectionBoundingBoxChanges(IBoundingBoxable boundingBoxable)
        {
            RaiseBoundingBoxChanges();
        }
    }
}
