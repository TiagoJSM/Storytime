using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Exceptions;
using StoryTimeFramework.DataStructures;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.Entities.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.WorldManagement.Manageables;
using StoryTimeFramework.Entities.Controllers;
using System.Reflection;
using StoryTimeCore.DataStructures;
using StoryTimeSceneGraph;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.General;

namespace StoryTimeFramework.WorldManagement
{
    /// <summary>
    /// Singleton class that represents the world.
    /// This class is the base of the framework, and will contain all the information about the elements of the world.
    /// The purpose of this class is to process the pipeline of the game world and to contain queryable information about the world.
    /// </summary>
    public class Scene
    {
        private class OrderedAsset : IBoundingBoxable
        {
            private IRenderableAsset _asset;

            public OrderedAsset(IRenderableAsset asset)
            {
                _asset = asset;
            }

            public OrderedAsset(IRenderableAsset asset, int zOrder)
                :this(asset)
            {
                _asset = asset;
                ZOrder = zOrder;
            }

            public Rectanglef BoundingBox { get { return _asset.BoundingBox; } }
            public IRenderableAsset RenderableAsset { get { return _asset; } }
            public int ZOrder { get; set; }

        }

        // The list of the many World Entities in the scene.
        private List<BaseActor> _baseActors;
        private Quadtree<IRenderableAsset> _renderables;
        private ICamera _activeCamera;
        private Dictionary<IRenderableAsset, OrderedAsset> _assetDictionary;
        private int _nextIndex;

        private WorldTime _currentTime;

        public string SceneName { get; set; }

        public Scene()
        {
            _baseActors = new List<BaseActor>();
            _renderables = new Quadtree<IRenderableAsset>();
            _assetDictionary = new Dictionary<IRenderableAsset, OrderedAsset>();
            _nextIndex = 0;
            _activeCamera = new Camera() { Viewport = new Viewport(0, 0, 1280, 720) };
        }

        public void Render(IGraphicsContext graphicsContext)
        {
            //clear graphics device
            if (_activeCamera == null) return;
            //set viewport
            Viewport vp = _activeCamera.Viewport;
            graphicsContext.SetSceneDimensions(vp.Width, vp.Height);
            Rectanglef renderingViewport = new Rectanglef(vp.X, vp.Y, vp.Height, vp.Width);
            IEnumerable<IRenderableAsset> enumRA = GetRenderablesIn(renderingViewport);
            IRenderer renderer = graphicsContext.GetRenderer();
            foreach (IRenderableAsset ba in enumRA)
                ba.Render(renderer);
        }

        public void Update(WorldTime WTime)
        {
            _currentTime = WTime;
            foreach (BaseActor ba in _baseActors)
            {
                ba.TimeElapse(_currentTime);
            }
        }

        public void AddActor(BaseActor ba)
        {
            if (_baseActors.Contains(ba)) return;

            _baseActors.Add(ba);
            OrderedAsset oa = new OrderedAsset(ba.RenderableActor, _nextIndex);
            _nextIndex++;
            AddOrderedAsset(oa);
        }

        public void RemoveWorldEntity(BaseActor ba)
        {
            if (!_baseActors.Contains(ba)) return;

            _baseActors.Remove(ba);
            OrderedAsset oa;
            if (_assetDictionary.TryGetValue(ba.RenderableActor, out oa))
            {
                RemoveOrderedAsset(oa);
                ReOrderAssets();
            }
        }

        private void AddOrderedAsset(OrderedAsset orderedAsset)
        {
            _assetDictionary.Add(orderedAsset.RenderableAsset, orderedAsset);
            orderedAsset.RenderableAsset.OnBoundingBoxChanges += RenderableAssetBoundsChange;
            _renderables.Add(orderedAsset.RenderableAsset);
        }

        private void RemoveOrderedAsset(OrderedAsset orderedAsset)
        {
            bool removed = _renderables.Remove(orderedAsset.RenderableAsset);
            if (removed)
            {
                _assetDictionary.Remove(orderedAsset.RenderableAsset);
                orderedAsset.RenderableAsset.OnBoundingBoxChanges -= RenderableAssetBoundsChange;
            }
        }

        private IEnumerable<IRenderableAsset> GetRenderablesIn(Rectanglef renderingViewport)
        {
            List<IRenderableAsset> ras = new List<IRenderableAsset>();
            Action<IRenderableAsset> renderHitAction = (ra) => ras.Add(ra);
            _renderables.Query(renderingViewport, renderHitAction);
            IEnumerable<IRenderableAsset> enumRA = ras.OrderBy(ra=>ZIndexOrder(ra));
            return enumRA;
        }

        private int ZIndexOrder(IRenderableAsset ra)
        {
            return _assetDictionary[ra].ZOrder;
        }

        private void RenderableAssetBoundsChange(IRenderableAsset asset)
        {
            OrderedAsset oa;
            if (_assetDictionary.TryGetValue(asset, out oa))
            {
                RemoveOrderedAsset(oa);
                AddOrderedAsset(oa);
            }
        }

        private void ReOrderAssets()
        {
            _nextIndex = 0;
            foreach (OrderedAsset oa in _assetDictionary.Values)
            {
                oa.ZOrder = _nextIndex;
                _nextIndex++;
            }

        }
    }
}
