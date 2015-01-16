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
//using StoryTimeSceneGraph;
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
        private class OrderedWorldEntity : IBoundingBoxable
        {
            private WorldEntity _entity;

            public OrderedWorldEntity(WorldEntity entity)
            {
                _entity = entity;
            }

            public OrderedWorldEntity(WorldEntity entity, int zOrder)
                : this(entity)
            {
                ZOrder = zOrder;
            }

            public AxisAlignedBoundingBox2D AABoundingBox { get { return _entity.AABoundingBox; } }
            public BoundingBox2D BoundingBox { get { return _entity.BoundingBox; } }
            public WorldEntity WorldEntity { get { return _entity; } }
            public int ZOrder { get; set; }
        }

        // The list of the many World Entities in the scene.
        private List<WorldEntity> _worldEntities;
        private Quadtree<WorldEntity> _quadTree;
        private ICamera _activeCamera;
        private Dictionary<WorldEntity, OrderedWorldEntity> _actorsDictionary;
        private int _nextIndex;
        private bool _initialized;
        private IGraphicsContext _graphicsContext;
        private GUIManager _gui;

        private WorldTime _currentTime;

        public string SceneName { get; set; }
        public ICamera Camera { get { return _activeCamera; } }
        public IEnumerable<WorldEntity> WorldEntities { get { return _worldEntities; } }
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
            _worldEntities = new List<WorldEntity>();
            _quadTree = new Quadtree<WorldEntity>();
            _actorsDictionary = new Dictionary<WorldEntity, OrderedWorldEntity>();
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
            var vp = _activeCamera.Viewport;
            graphicsContext.SetCamera(_activeCamera);
            var renderingViewport = new AxisAlignedBoundingBox2D(vp.X, vp.Y, vp.Height, vp.Width);
            var enumActors = GetRenderablesIn(renderingViewport);
            var renderer = graphicsContext.GetRenderer();
            foreach (var ba in enumActors)
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
            foreach (var ba in _worldEntities)
            {
                ba.TimeElapse(_currentTime);
            }
        }

        public TEntity AddWorldEntity<TEntity>(Action<TEntity> initializer = null) where TEntity : WorldEntity
        {
            var entity =  AddWorldEntity(typeof(TEntity), null) as TEntity;
            if (initializer != null)
                initializer(entity);
            return entity;
        }

        public WorldEntity AddWorldEntity(Type entityType, Action<WorldEntity> initializer = null)
        {
            var entity = Activator.CreateInstance(entityType) as WorldEntity;

            entity.Scene = this;
            if (initializer != null)
                initializer(entity);
            entity.Initialize();

            _worldEntities.Add(entity);
            var oa = new OrderedWorldEntity(entity, _nextIndex);
            _nextIndex++;
            AddOrderedEntity(oa);

            return entity;
        }

        public void RemoveWorldEntity(WorldEntity entity)
        {
            if (!_worldEntities.Contains(entity)) return;

            _worldEntities.Remove(entity);
            OrderedWorldEntity orderedEntity;
            if (_actorsDictionary.TryGetValue(entity, out orderedEntity))
            {
                RemoveOrderedEntity(orderedEntity);
                ReOrderEntities();
            }
        }

        public List<WorldEntity> AxisAlignedIntersect(Vector2 point)
        {
            var entities = _quadTree.Intersect(point);
            return EntitiesInOrder(entities).ToList();
        }

        public List<WorldEntity> Intersect(Vector2 point)
        {
            var actors = _quadTree.Intersect(point);
            var filteredEntities = actors.Where(a => a.BoundingBox.Contains(point));
            return EntitiesInOrder(filteredEntities);
        }

        private List<WorldEntity> EntitiesInOrder(IEnumerable<WorldEntity> entities)
        {
            var orderedEntities = new List<OrderedWorldEntity>();
            foreach (var entity in entities)
            {
                orderedEntities.Add(_actorsDictionary[entity]);
            }
            return
                orderedEntities
                .OrderByDescending((oActor) => oActor.ZOrder)
                .Select((oActor) => oActor.WorldEntity)
                .ToList();
        }

        private void AddOrderedEntity(OrderedWorldEntity orderedAsset)
        {
            _actorsDictionary.Add(orderedAsset.WorldEntity, orderedAsset);
            orderedAsset.WorldEntity.OnBoundingBoxChanges += RenderableAssetBoundsChange;
            _quadTree.Add(orderedAsset.WorldEntity);
        }

        private void RemoveOrderedEntity(OrderedWorldEntity orderedAsset)
        {
            var removed = _quadTree.Remove(orderedAsset.WorldEntity);
            if (removed)
            {
                _actorsDictionary.Remove(orderedAsset.WorldEntity);
                orderedAsset.WorldEntity.OnBoundingBoxChanges -= RenderableAssetBoundsChange;
            }
        }

        private IEnumerable<BaseActor> GetRenderablesIn(AxisAlignedBoundingBox2D renderingViewport)
        {
            var actors = new List<BaseActor>();
            Action<WorldEntity> renderHitAction = (entity) =>
            {
                if (entity is BaseActor)
                    actors.Add(entity as BaseActor);
            };
            _quadTree.Query(renderingViewport, renderHitAction);
            IEnumerable<BaseActor> orderedEntities = actors.OrderBy(actor => ZIndexOrder(actor));
            return orderedEntities;
        }

        private int ZIndexOrder(BaseActor actor)
        {
            return _actorsDictionary[actor].ZOrder;
        }

        private void RenderableAssetBoundsChange(WorldEntity entity)
        {
            OrderedWorldEntity orderedEntity;
            if (_actorsDictionary.TryGetValue(entity, out orderedEntity))
            {
                RemoveOrderedEntity(orderedEntity);
                AddOrderedEntity(orderedEntity);
            }
        }

        private void ReOrderEntities()
        {
            _nextIndex = 0;
            foreach (var oa in _actorsDictionary.Values)
            {
                oa.ZOrder = _nextIndex;
                _nextIndex++;
            }
        }
    }
}
