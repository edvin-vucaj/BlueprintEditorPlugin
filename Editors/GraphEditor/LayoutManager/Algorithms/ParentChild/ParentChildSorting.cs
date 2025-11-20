using BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes;
using BlueprintEditorPlugin.Editors.BlueprintEditor.Nodes.Ports;
using BlueprintEditorPlugin.Editors.GraphEditor.LayoutManager.Algorithms.Sugiyama;
using BlueprintEditorPlugin.Models.Connections;
using BlueprintEditorPlugin.Models.Nodes;
using BlueprintEditorPlugin.Models.Nodes.Ports;
using BlueprintEditorPlugin.Views.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlueprintEditorPlugin.Editors.GraphEditor.LayoutManager.Algorithms.ParentChild
{
    public class ParentChildSorting : SugiyamaMethod
    {
        public double horizontalParentChildSpacing = 120d;
        public double verticalParentChildSpacing = 50d;

        public override void SortGraph()
        {
            base.SortGraph();

            // Rearrange nodes to reflect parent-children relationships

            // Arrange each node's parent and children nodes aligned vertically centered
            // around the node, with specified horizontal and vertical spacing
            foreach (IVertex vertex in _vertices)
            {
                if (vertex is EntityNode node)
                {
                    if (node.Inputs.Count > 0)
                    {
                        Point referencePoint = InputsReferenceMarginPoint(node);

                        double YOffset = referencePoint.Y - (totalInputParentsHeight(node) / 2d);

                        AssignNewInputsPositions(node, referencePoint.X, YOffset);
                    }
                    if (node.Outputs.Count > 0)
                    {
                        Point referencePoint = OutputsReferenceMarginPoint(node);

                        double YOffset = referencePoint.Y - (totalOutputChildrensHeight(node) / 2d);

                        AssignNewOutputsPositions(node, referencePoint.X, YOffset);
                    }
                }
            }

            // remove overlapping nodes
            RemoveOverlaps();
        }

        public Point InputsReferenceMarginPoint(EntityNode _node)
        {
            Point point = new Point(_node.Location.X - (horizontalParentChildSpacing + (totalInputParentsHeight(_node) / (double)_node.Inputs.Count)), _node.Location.Y + (_node.Size.Height / 2d) + 20d);
            return point;
        }
        public Point OutputsReferenceMarginPoint(EntityNode _node)
        {
            Point point = new Point(_node.Location.X + _node.Size.Width + (horizontalParentChildSpacing + (totalOutputChildrensHeight(_node) / (double)_node.Outputs.Count)), _node.Location.Y + (_node.Size.Height / 2d) + 20d);
            return point;
        }

        public double totalInputParentsHeight(EntityNode _node)
        {
            double totalHeight = 0d;
            foreach (IConnection connection in _node.NodeWrangler.Connections)
            {
                if (connection.Target.Node != _node)
                    continue;
                BaseConnection bconnection = connection as BaseConnection;
                INode parentNode = bconnection.Source.Node;
                totalHeight += parentNode.Size.Height + verticalParentChildSpacing;
            }
            return totalHeight;
        }

        public double totalOutputChildrensHeight(EntityNode _node)
        {
            double totalHeight = 0d;
            foreach (IConnection connection in _node.NodeWrangler.Connections)
            {
                if (connection.Source.Node != _node)
                    continue;
                BaseConnection bconnection = connection as BaseConnection;
                INode childNode = bconnection.Target.Node;
                totalHeight += childNode.Size.Height + verticalParentChildSpacing;
            }
            return totalHeight;
        }

        public void AssignNewInputsPositions(EntityNode _node, double _xOffset, double _yOffset)
        {
            foreach (IConnection connection in _node.NodeWrangler.Connections)
            {
                if (connection.Target.Node != _node)
                    continue;
                BaseConnection bconnection = connection as BaseConnection;
                INode parentNode = bconnection.Source.Node;
                Point newPosition = new Point(_xOffset - parentNode.Size.Width, _yOffset);
                parentNode.Location = newPosition;
                _yOffset += parentNode.Size.Height + verticalParentChildSpacing;
            }
        }

        public void AssignNewOutputsPositions(EntityNode _node, double _xOffset, double _yOffset)
        {
            foreach (IConnection connection in _node.NodeWrangler.Connections)
            {
                if (connection.Source.Node != _node)
                    continue;
                BaseConnection bconnection = connection as BaseConnection;
                INode childNode = bconnection.Target.Node;
                Point newPosition = new Point(_xOffset, _yOffset);
                childNode.Location = newPosition;
                _yOffset += childNode.Size.Height + verticalParentChildSpacing;
            }
        }

        public bool PortHasMultipleConnections(IPort _port)
        {
            int connectionCount = 0;
            foreach (IConnection connection in _port.Node.NodeWrangler.Connections)
            {
                BaseConnection bconnection = connection as BaseConnection;
                if (bconnection.Source == _port || bconnection.Target == _port)
                {
                    connectionCount++;
                }
            }
            return connectionCount > 1;
        }

        public void RemoveOverlaps()
        {
            // Simple overlap removal by shifting nodes downwards
            HashSet<IVertex> processedNodes = new HashSet<IVertex>();
            foreach (IVertex vertexA in _vertices)
            {
                if (processedNodes.Contains(vertexA))
                    continue;
                Rect rectA = new Rect(vertexA.Location, vertexA.Size);
                foreach (IVertex vertexB in _vertices)
                {
                    if (vertexA == vertexB || processedNodes.Contains(vertexB))
                        continue;
                    Rect rectB = new Rect(vertexB.Location, vertexB.Size);
                    if (rectA.IntersectsWith(rectB))
                    {
                        double overlapHeight = (rectA.Bottom - rectB.Top) + verticalParentChildSpacing; // Add some padding
                        vertexB.Location = new Point(vertexB.Location.X, vertexB.Location.Y + overlapHeight);
                        rectB = new Rect(vertexB.Location, vertexB.Size); // Update rectB after moving
                    }
                }
                processedNodes.Add(vertexA);
            }
        }

        public double GetMiddleYPos_Interface(InterfaceNode _interfaceNode)
        {
            // get the first element Y position to begin with
            IConnection firstConnection = _interfaceNode.NodeWrangler.Connections.FirstOrDefault();
            BaseConnection bconnection = firstConnection as BaseConnection;
            // determine the direction of the connected node to the interface node
            INode connectedNode = (bconnection.Source.Node == _interfaceNode) ? bconnection.Target.Node : bconnection.Source.Node;
            double currentY = connectedNode.Location.Y;
            // set both the upper and lower Y position to current Y position
            double upperY = currentY;
            double lowerY = currentY;

            // if there are more connections, iterate through them to find the uppermost and lowermost Y positions
            for (int i = 1; i < _interfaceNode.NodeWrangler.Connections.Count(); i++)
            {
                IConnection connection = _interfaceNode.NodeWrangler.Connections.ElementAt(i);
                bconnection = connection as BaseConnection;
                // determine the direction of the connected node
                connectedNode = (bconnection.Source.Node == _interfaceNode) ? bconnection.Target.Node : bconnection.Source.Node;
                currentY = connectedNode.Location.Y;
                
                if (currentY < upperY)
                    upperY = currentY;
                else if (currentY > lowerY)
                    lowerY = currentY;
            }

            // calculate the middle Y position
            double middleY = (upperY + lowerY) / 2d;

            return middleY;
        }

        public ParentChildSorting(List<IConnection> connections, List<IVertex> vertices)
        {
            _connections = connections;
            _vertices = vertices;
        }
    }
}
