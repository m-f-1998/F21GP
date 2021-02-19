/**
 * @author Matthew Frankland
 * @email [developer@matthewfrankland.co.uk]
 * @create date 10-02-2021 18:41:10
 * @modify date 19-02-2021 09:26:22
 * @desc [A* Pathfinding - Best Path]
 */

using UnityEngine;
using System.Collections.Generic;
using System;

public class Path {
    public static List<Vector2> FindPath(Grid grid, Vector2 pos1, Vector2 pos2, ref List<Vector2> path) {
        Node startNode = grid.GetNode((int) Math.Ceiling(pos1.x), (int) Math.Ceiling(pos1.y));
        Node targetNode = grid.GetNode((int) Math.Ceiling(pos2.x), (int) Math.Ceiling(pos2.y));

        Heap openSet = new Heap(grid.GetSize());
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);    

        while (openSet.GetCount() > 0) {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                while (currentNode != startNode) {
                    path.Add(new Vector2(currentNode.GetXPos(), currentNode.GetYPos()));
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.IsWalkable() || closedSet.Contains(neighbour)) continue;
                
                int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return path;
    }

    private static int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.GetXPos() - nodeB.GetXPos());
        int dstY = Mathf.Abs(nodeA.GetYPos() - nodeB.GetYPos());
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}