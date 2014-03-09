using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.GenericExceptions;
using StoryTimeFramework.Entities.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeCore.General;

namespace StoryTimeFramework.DataStructures
{
    /// <summary>
    /// Enums to define the QuadtreeNode position, this is used to help in terms of code legibility.
    /// </summary>
    public enum NodesPosition
    {
        BottomLeft = 0,
        BottomRight = 1,
        TopLeft = 2,
        TopRight = 3,
        TotalNodes = 4
    }

    public interface IQuadtreeNode<TData> where TData : IBoundingBoxable
    {
        IQuadtreeNode<TData> Parent { get; }
        bool IsLeaf { get; }
        bool Remove(TData data);
    }

    /// <summary>
    /// The node that contains the data to be searched.
    /// </summary>
    public class QuadtreeNode<TData> : IQuadtreeNode<TData> where TData : IBoundingBoxable
    {
        private List<TData> _dataSet;
        private QuadtreeNode<TData>[] _childNodes;
        private Rectanglef _nodeCoordinates;
        private Quadtree<TData> _quadtree;
        private IQuadtreeNode<TData> _parent;

        public QuadtreeNode(Rectanglef nodeCoordinates, Quadtree<TData> quadtree, IQuadtreeNode<TData> parent = null)
        {
            _childNodes = new QuadtreeNode<TData>[(int)NodesPosition.TotalNodes];
            _nodeCoordinates = nodeCoordinates;
            _quadtree = quadtree;
            _parent = parent;
        }

        public List<TData> DataSet
        {
            get
            {
                if (_dataSet == null)
                    _dataSet = new List<TData>();
                return _dataSet;
            }
        }
        public Quadtree<TData> Quadtree { get { return _quadtree; } }
        public IQuadtreeNode<TData> Parent { get { return _parent; } }
        public bool IsLeaf
        {
            get
            {
                foreach (QuadtreeNode<TData> node in _childNodes)
                    if (node != null)
                        return false;
                return true;
            }
        }
        public Rectanglef NodeCoordinates { get { return _nodeCoordinates; } }

        public QuadtreeNode<TData> GetChildAt(NodesPosition position)
        {
            return GetChildAt((int)position);
        }

        public QuadtreeNode<TData> GetChildAt(int position)
        {
            if (position < 0 || position >= (int)NodesPosition.TotalNodes)
                throw new InvalidIndexException(0, 3);

            return _childNodes[position];
        }

        private void CreateChildrenAt(int position)
        {
            Rectanglef childNodeBounds = GetChildrenCoordinatesAt(position);
            _childNodes[position] = new QuadtreeNode<TData>(childNodeBounds, Quadtree, this);
        }

        private Rectanglef GetChildrenCoordinatesAt(int position)
        {
            float childSize = _nodeCoordinates.Width / 2;
            float X = _nodeCoordinates.X;
            float Y = _nodeCoordinates.Y;

            switch (position)
            {
                case (int)NodesPosition.BottomLeft:
                    //nothing to do here
                    break;
                case (int)NodesPosition.BottomRight:
                    X += childSize;
                    break;
                case (int)NodesPosition.TopLeft:
                    Y += childSize;
                    break;
                case (int)NodesPosition.TopRight:
                    X += childSize;
                    Y += childSize;
                    break;
            }

            Rectanglef childNodeBounds = new Rectanglef(X, Y, childSize);
            return childNodeBounds;
        }


        private bool CanCreateChildren()
        {
            float childSize = _nodeCoordinates.Width / 2;
            if (childSize < _quadtree.NodeMinimumSideSize)
                return false;
            return true;
        }

        public bool CanAdd(TData data)
        {
            return _nodeCoordinates.Contains(data.BoundingBox);
        }

        public IQuadtreeNode<TData> Add(TData data)
        {
            if (!CanAdd(data)) return null;

            if (!CanCreateChildren())
            {
                DataSet.Add(data);
                return this;
            }

            for (int idx = 0; idx < _childNodes.Length; idx++)
            {
                if (_childNodes[idx] == null)
                {
                    Rectanglef nodeCoords = GetChildrenCoordinatesAt(idx);
                    bool contains = nodeCoords.Contains(data.BoundingBox);
                    if (contains)
                    {
                        CreateChildrenAt(idx);
                        return _childNodes[idx].Add(data);
                    }
                }
            }
            DataSet.Add(data);
            return this;
        }
        public bool Remove(TData data)
        {
            if (DataSet.Contains(data))
            {
                DataSet.Remove(data);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// The data structure that holds all the children nodes and the data contained.
    /// This class is used to manage the childen and understand where to place the data
    /// </summary>
    public class Quadtree<TData> : IQuadtreeNode<TData>  where TData : IBoundingBoxable
    {
        private List<TData> _dataSet;
        private QuadtreeNode<TData> _childNode;
        private float _nodeMinSideSize;
        private Dictionary<TData, IQuadtreeNode<TData>> _dataHolding;

        public Quadtree(float xOrigin = 0, float yOrigin = 0, float sideSize = 800, float nodeMinSideSize = 10) 
        {
            _childNode = new QuadtreeNode<TData>(
                new Rectanglef(xOrigin, yOrigin, sideSize, sideSize),
                this,
                this
            );
            _nodeMinSideSize = nodeMinSideSize;
            _dataHolding = new Dictionary<TData, IQuadtreeNode<TData>>();
        }

        public float NodeMinimumSideSize { get { return _nodeMinSideSize; } }
        
        private List<TData> DataSet
        {
            get
            {
                if (_dataSet == null)
                    _dataSet = new List<TData>();
                return _dataSet;
            }
        }

        public void Add(TData data)
        {
            if (!_childNode.CanAdd(data))
            {
                DataSet.Add(data);
                _dataHolding.Add(data, this);
            }

            IQuadtreeNode<TData> bucket = _childNode.Add(data);
            _dataHolding.Add(data, bucket);
        }

        public bool Remove(TData data)
        {
            IQuadtreeNode<TData> bucket;
            if (_dataHolding.TryGetValue(data, out bucket))
            {
                bool removed = bucket.Remove(data);
                if (removed)
                    _dataHolding.Remove(data);
                return removed;
            }
            return false;
        }

        public void Query(Action<TData> HitAction)
        {
            Query(_childNode.NodeCoordinates, HitAction);
        }

        public void Query(Rectanglef boundingBox, Action<TData> HitAction)
        {
            DataSet
                .ForEach( 
                    TData =>
                    {
                        if(boundingBox.Intersects(TData.BoundingBox))
                            HitAction(TData);
                    }
                );

            QueryAux(boundingBox, _childNode, HitAction);
        }

        private void QueryAux(Rectanglef boundingBox, QuadtreeNode<TData> node, Action<TData> HitAction)
        {
            if (node == null)
                return;
            if (!boundingBox.Intersects(node.NodeCoordinates))
                return;

            if (node.DataSet.Count > 0)
            {
                node.DataSet
                    .ForEach(
                        TData =>
                        {
                            if (boundingBox.Intersects(TData.BoundingBox))
                                HitAction(TData);
                        }
                    );
            }

            int numOfChilds = (int)NodesPosition.TotalNodes;
            for(int idx = 0; idx < numOfChilds; idx++)
            {
                QueryAux(boundingBox, node.GetChildAt(idx), HitAction);
            }
        }

        public IQuadtreeNode<TData> Parent
        {
            get
            {
                return null;
            }
        }

        public bool IsLeaf
        {
            get 
            {
                if (_childNode == null)
                    return true;
                return false;
            }
        }
    }
}