using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Exceptions;
using StoryTimeFramework.DataStructures;
using StoryTimeFramework.Entities.Actors;
using Microsoft.Xna.Framework.Graphics;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.WorldManagement.Manageables;
using StoryTimeFramework.Entities.Controllers;
using System.Reflection;
using StoryTimeCore.DataStructures;
using StoryTimeSceneGraph;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.General;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using StoryTimeCore.Physics;
using StoryTimeUI;
using StoryTimeCore.Entities;

namespace StoryTimeFramework.WorldManagement
{
    public class Scene
    {
        private class OrderedActor : IBoundingBoxable
        {
            private BaseActor _actor;

            public OrderedActor(BaseActor actor)
            {
                _actor = actor;
            }

            public OrderedActor(BaseActor actor, int zOrder)
                : this(actor)
            {
                ZOrder = zOrder;
            }

            public AxisAlignedBoundingBox2D AABoundingBox { get { return _actor.AABoundingBox; } }
            public BoundingBox2D BoundingBox { get { return _actor.BoundingBox; } }
            public BaseActor Actor { get { return _actor; } }
            public int ZOrder { get; set; }
        }

        // The list of the many World Entities in the scene.
        private List<BaseActor> _baseActors;
        private Quadtree<BaseActor> _actorsTree;
        private ICamera _activeCamera;
        private Dictionary<BaseActor, OrderedActor> _actorsDictionary;
        private int _nextIndex;
        private bool _initialized;
        private IGraphicsContext _graphicsContext;
        private GUIManager _gui;

        private WorldTime _currentTime;

        public string SceneName { get; set; }
        public ICamera Camera { get { return _activeCamera; } }
        public IEnumerable<BaseActor> Actors { get { return _baseActors; } }
        public IPhysicalWorld PhysicalWorld { get; set; }
        public IGraphicsContext GraphicsContext
        {  
            get 
            {
                return _graphicsContext;
            }
            set 
            {
                if(_graphicsContext == null)
                    _graphicsContext = value;
            }
        }
        public GUIManager GUI { get { return _gui; } }

        public Scene()
        {
            _baseActors = new List<BaseActor>();
            _actorsTree = new Quadtree<BaseActor>();
            _actorsDictionary = new Dictionary<BaseActor, OrderedActor>();
            _nextIndex = 0;
            _activeCamera = new Camera() { Viewport = new Viewport(0, 0, 1280, 720) }; //1280
        }

        public Scene(Vector2 gravity)
            : this()
        {
            PhysicalWorld.Gravity = gravity;
        }

        public void Initialize()
        {
            if (_initialized) return;
            _gui = new GUIManager(_graphicsContext);
            //TODO: other required logic
            _initialized = true;
        }

        public void Render(IGraphicsContext graphicsContext)
        {
            //clear graphics device
            if (_activeCamera == null) return;
            //set viewport
            Viewport vp = _activeCamera.Viewport;
            graphicsContext.SetCamera(_activeCamera);
            AxisAlignedBoundingBox2D renderingViewport = new AxisAlignedBoundingBox2D(vp.X, vp.Y, vp.Height, vp.Width);
            IEnumerable<BaseActor> enumActors = GetRenderablesIn(renderingViewport);
            IRenderer renderer = graphicsContext.GetRenderer();
            foreach (BaseActor ba in enumActors)
            {
                renderer.TranslationTransformation += ba.Body.Position;
                renderer.RotationTransformation += ba.Body.Rotation;
                ba.RenderableAsset.Render(renderer);
                renderer.RotationTransformation -= ba.Body.Rotation;
                renderer.TranslationTransformation -= ba.Body.Position;

                //renderer.RenderBoundingBox(ba.BoundingBox, Color.Red);
            }
            //TODO: should reset renderer here!
            _gui.Render(renderer);
        }

        public void Update(WorldTime WTime)
        {
            _currentTime = WTime;
            foreach (BaseActor ba in _baseActors)
            {
                ba.TimeElapse(_currentTime);
            }
        }

        public TActor AddActor<TActor>() where TActor : BaseActor
        {
            return AddActor(typeof(TActor)) as TActor;
        }

        public BaseActor AddActor(Type actorType)
        {
            BaseActor actor = Activator.CreateInstance(actorType) as BaseActor;

            actor.Scene = this;
            actor.Initialize();

            _baseActors.Add(actor);
            OrderedActor oa = new OrderedActor(actor, _nextIndex);
            _nextIndex++;
            AddOrderedAsset(oa);

            return actor;
        }

        public void RemoveActor(BaseActor ba)
        {
            if (!_baseActors.Contains(ba)) return;

            _baseActors.Remove(ba);
            OrderedActor oa;
            if (_actorsDictionary.TryGetValue(ba, out oa))
            {
                RemoveOrderedActor(oa);
                ReOrderActors();
            }
        }

        public List<BaseActor> AxisAlignedIntersect(Vector2 point)
        {
            List<BaseActor> actors = _actorsTree.Intersect(point);
            return ActorsInOrder(actors);
        }

        public List<BaseActor> Intersect(Vector2 point)
        {
            List<BaseActor> actors = _actorsTree.Intersect(point);
            IEnumerable<BaseActor> filteredActors = actors.Where(a => a.BoundingBox.Contains(point));
            return ActorsInOrder(filteredActors);
        }

        private List<BaseActor> ActorsInOrder(IEnumerable<BaseActor> actors)
        {
            List<OrderedActor> orderActors = new List<OrderedActor>();
            foreach (BaseActor actor in actors)
            {
                orderActors.Add(_actorsDictionary[actor]);
            }
            return
                orderActors
                .OrderByDescending((oActor) => oActor.ZOrder)
                .Select((oActor) => oActor.Actor)
                .ToList();
        }

        private void AddOrderedAsset(OrderedActor orderedAsset)
        {
            _actorsDictionary.Add(orderedAsset.Actor, orderedAsset);
            orderedAsset.Actor.OnBoundingBoxChanges += RenderableAssetBoundsChange;
            _actorsTree.Add(orderedAsset.Actor);
        }

        private void RemoveOrderedActor(OrderedActor orderedAsset)
        {
            bool removed = _actorsTree.Remove(orderedAsset.Actor);
            if (removed)
            {
                _actorsDictionary.Remove(orderedAsset.Actor);
                orderedAsset.Actor.OnBoundingBoxChanges -= RenderableAssetBoundsChange;
            }
        }

        private IEnumerable<BaseActor> GetRenderablesIn(AxisAlignedBoundingBox2D renderingViewport)
        {
            List<BaseActor> actors = new List<BaseActor>();
            Action<BaseActor> renderHitAction = (actor) => actors.Add(actor);
            _actorsTree.Query(renderingViewport, renderHitAction);
            IEnumerable<BaseActor> enumActors = actors.OrderBy(actor => ZIndexOrder(actor));
            return enumActors;
        }

        private int ZIndexOrder(BaseActor actor)
        {
            return _actorsDictionary[actor].ZOrder;
        }

        private void RenderableAssetBoundsChange(BaseActor actor)
        {
            OrderedActor oa;
            if (_actorsDictionary.TryGetValue(actor, out oa))
            {
                RemoveOrderedActor(oa);
                AddOrderedAsset(oa);
            }
        }

        private void ReOrderActors()
        {
            _nextIndex = 0;
            foreach (OrderedActor oa in _actorsDictionary.Values)
            {
                oa.ZOrder = _nextIndex;
                _nextIndex++;
            }
        }
    }
}
