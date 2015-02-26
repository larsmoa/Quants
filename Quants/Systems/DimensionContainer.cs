using System;
using System.Collections.Generic;
using System.Linq;
using Quants.Conversion;

namespace Quants.Systems
{
    /// <summary>
    /// Convinience class for implementing IUnitSystem. Responsible for providing units
    /// and unit converters within one IDimension.
    /// </summary>
    public class DimensionContainer: ICloneable
    {
        /// <summary>
        /// Factory function that is responsible for creating IValueConverters for units with a direct relation, i.e. where
        /// one of the units are parent of the other. The first argument is the source unit, the second is the target unit.
        /// </summary>
        /// <param name="source">Unit to convert values from.</param>
        /// <param name="target">Unit to convert values to.</param>
        /// <returns></returns>
        public delegate IValueConverter CreateAdjacentValueConverterDelegate(IUnit source, IUnit target);

        /// <summary>
        /// Tree structure that contains units and the relation between them.
        /// </summary>
        private class Node: ICloneable
        {
            private readonly IList<Node> _children = new List<Node>();

            public IUnit Unit { get; private set; }
            public Node Parent { get; private set; }

            public Node(IUnit unit, Node parent = null)
            {
                Unit = unit;
                Parent = parent;
            }

            public object Clone()
            {
                // Deep clone
                Node clone = new Node(Unit, Parent);                
                foreach (Node child in _children)
                {
                    Node childClone = (Node) child.Clone();
                    childClone.Parent = clone;
                    clone._children.Add(childClone);
                }
                return clone;
            }

            public void AddChild(IUnit child)
            {
                if (!Equals(child.Dimension, Unit.Dimension))
                    throw new InvalidOperationException("Dimensions does not match.");

                Node childNode = new Node(child, this);
                _children.Add(childNode);
            }

            /// <summary>
            /// Traverse the tree bredth first.
            /// </summary>
            /// <param name="action">When action returns true the traversal stops.</param>
            public void TraverseBredthFirst(Func<Node, bool> action)
            {
                Queue<Node> queue = new Queue<Node>();
                queue.Enqueue(this);
                do
                {
                    Node current = queue.Dequeue();
                    if (action(current))
                    {
                        // Callback returns true, stop traversal
                        break;
                    }
                    foreach (Node child in current._children)
                    {
                        queue.Enqueue(child);
                    }
                } while (queue.Count > 0);
            }

            public Node FindNode(IUnit unit)
            {
                Node node = null;
                Func<Node, bool> locator =
                    x =>
                    {
                        if (Equals(x.Unit, unit))
                        {
                            node = x;
                            return true; // Node found
                        }
                        return false; // Keep on looking
                    };
                TraverseBredthFirst(locator);
                return node;
            }

            private LinkedList<Node> BacktrackTo(Node target)
            {
                LinkedList<Node> path = new LinkedList<Node>();
                // Backtrack
                Node current = this;
                do
                {
                    path.AddFirst(current);
                    current = current.Parent;
                } while (current != target && current != null);
                if (current != null)
                    path.AddFirst(current);
                return path;
            }

            public LinkedList<Node> FindPathBetween(IUnit from, IUnit to)
            {
                // 1. Find indiviual path to from and to
                Node fromNode = FindNode(from);
                Node toNode = FindNode(to);
                LinkedList<Node> fromPath = fromNode.BacktrackTo(this);
                LinkedList<Node> toPath = toNode.BacktrackTo(this);

                // 2. Chop of common parts and find the last common node
                Node lastCommon = ChopAwayCommonPath(fromPath, toPath);

                // 3. Reverse from path
                fromPath = new LinkedList<Node>(fromPath.Reverse());

                // 4. Combine and return!
                fromPath.AddLast(lastCommon);
                LinkedListNode<Node> curr = toPath.First;
                while (curr != null)
                {
                    fromPath.AddLast(curr.Value);
                    curr = curr.Next;
                }
                return fromPath;
            }

            private static Node ChopAwayCommonPath(LinkedList<Node> fromPath, LinkedList<Node> toPath)
            {
                Node lastCommon = null;
                while (fromPath.Count > 0 && toPath.Count > 0 && 
                       ReferenceEquals(fromPath.First.Value, toPath.First.Value))
                {
                    lastCommon = fromPath.First.Value;
                    fromPath.RemoveFirst();
                    toPath.RemoveFirst();
                }
                return lastCommon;
            }
        }

        private Node _root;
        private readonly IDimension _dimension;
        private readonly CreateAdjacentValueConverterDelegate _adjacentValueConverterFactory;

        /// <summary>
        /// Creates a new container for a given dimension, e.g. "Mass".
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="adjacentValueConverterFactory">
        /// Factory function that is responsible for creating IValueConverters for units with a direct relation, i.e. where
        /// one of the units are parent of the other. The first argument is the source unit, the second is the target unit.
        /// </param>
        public DimensionContainer(IDimension dimension, CreateAdjacentValueConverterDelegate adjacentValueConverterFactory)
        {
            _dimension = dimension;
            _adjacentValueConverterFactory = adjacentValueConverterFactory;
        }

        /// <summary>
        /// Returns a deep clone of the container. The tree is cloned, but the clone will reference the
        /// same units, dimension and CreateAdjacentValueConverterDelegate as the original.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            DimensionContainer clone = new DimensionContainer(_dimension, _adjacentValueConverterFactory)
                                           {
                                               _root = (_root != null) ? (Node) _root.Clone() : null
                                           };
            return clone;
        }

        /// <summary>
        /// Returns the IDmension this container is bound to. All units within the container
        /// is in this dimension.
        /// </summary>
        public IDimension Dimension { get { return _dimension; } }

        /// <summary>
        /// Returns true if SetBaseUnit() has been called with a 
        /// </summary>
        public bool HasBaseUnit
        {
            get { return _root != null; }
        }

        /// <summary>
        /// Return the unit set by SetBaseUnit().
        /// </summary>
        public IUnit BaseUnit
        {
            get { return _root != null ? _root.Unit : null; }
        }

        /// <summary>
        /// Returns all units in the container.
        /// </summary>
        public IEnumerable<IUnit> Units
        {
            get
            {
                if (HasBaseUnit)
                {
                    LinkedList<IUnit> units = new LinkedList<IUnit>();
                    _root.TraverseBredthFirst((x) =>
                                                  {
                                                      units.AddLast(x.Unit);
                                                      return false; // Continue traversal
                                                  });
                    return units;
                }
                return Enumerable.Empty<IUnit>();
            }
        }

        /// <summary>
        /// Returns true if the unit given is contained in the container.
        /// First checks if the dimension is correct and returns false right away
        /// without performing a search if not.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool ContainsUnit(IUnit unit)
        {
            if (!Equals(unit.Dimension, Dimension))
                return false;

            if (HasBaseUnit)
            {
                bool found = false;
                _root.TraverseBredthFirst((x) =>
                {
                    if (Equals(x.Unit, unit))
                    {
                        found = true;
                        return true;
                    }
                    return false; // Continue traversal
                });
                return found;
            }
            return false;
        }

        /// <summary>
        /// Specifies a base unit (the e.g. "kg" for the "Mass" dimension
        /// for the SI unit system). Clears the container.
        /// </summary>
        /// <param name="baseunit"></param>
        public void SetBaseUnit(IUnit baseunit)
        {
            _root = baseunit != null ? new Node(baseunit) : null;
        }

        /// <summary>
        /// Adds an unit to the container that has a relation (through an IValueConverter)
        /// to an unit allready registered in the container.
        /// </summary>
        /// <param name="baseunit"></param>
        /// <param name="unit"></param>
        public void AddUnit(IUnit baseunit, IUnit unit)
        {
            if (!Equals(baseunit.Dimension, Dimension) || !Equals(unit.Dimension, Dimension))
                throw new ArgumentException(string.Format("Dimension of units must be '{0}'", Dimension));

            Node parent = FindNode(baseunit);
            if (parent == null)
                throw new NullReferenceException(string.Format("Unit '{0}' not found", baseunit));
            parent.AddChild(unit);
        }

        /// <summary>
        /// Returns true if the container contains both of the units and therefore is able to convert
        /// between them.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown when base unit hasn't been set.</exception>
        /// <exception cref="ArgumentException">Thrown when source and target unit is equal.</exception>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool CanConvert(IUnit source, IUnit target)
        {
            if (!HasBaseUnit)
                throw new NullReferenceException("BaseUnit not set yet.");
            if (Equals(source, target))
                throw new ArgumentException("Source and target unit are equal.");

            Node sourceNode = _root.FindNode(source);
            Node targetNode = _root.FindNode(target);
            return (sourceNode != null && targetNode != null);
        }

        /// <summary>
        /// Creates a converter from a source unit to the target unit.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown when base unit hasn't been set.</exception>
        /// <exception cref="ArgumentException">Thrown when source and target unit is equal.</exception>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public IValueConverter CreateConverter(IUnit source, IUnit target)
        {
            if (!HasBaseUnit)
                throw new NullReferenceException("BaseUnit not set yet.");
            if (Equals(source, target))
                throw new ArgumentException("Source and target unit are equal.");

            LinkedList<Node> path = _root.FindPathBetween(source, target);
            LinkedListNode<Node> current = path.First;
            CompositeConverter compositeConverter = new CompositeConverter();
            while (current.Next != null)
            {
                IValueConverter converter = CreateAdjacentConverter(current.Value, current.Next.Value);
                compositeConverter.AddConverter(converter);
                current = current.Next;
            }
            return compositeConverter;
        }

        private Node FindNode(IUnit unit)
        {
            if (!HasBaseUnit)
                throw new NullReferenceException("BaseUnit not set yet.");
            return _root.FindNode(unit);
        }

        private IValueConverter CreateAdjacentConverter(Node source, Node target)
        {
            if (target.Parent == source || source.Parent == target)
            {
                return _adjacentValueConverterFactory(source.Unit, target.Unit);
            }
            throw new InvalidOperationException("Nodes are not adjecent.");
        }
    }
}
