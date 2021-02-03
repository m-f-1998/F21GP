using UnityEngine;
using System.Collections.Generic;

public class Pathfinding {
    public static List<Point> FindPath(Grid grid, Point startPos, Point targetPos) {
        List<Node> nodes_path = _ImpFindPath(grid, startPos, targetPos);

        List<Point> ret = new List<Point>();
        if (nodes_path != null) {
            foreach (Node node in nodes_path) {
                ret.Add(new Point(node.gridX, node.gridY));
            }
        }
        return ret;
    }

    private static List<Node> _ImpFindPath(Grid grid, Point startPos, Point targetPos) {
        Node startNode = grid.nodes[startPos.x, startPos.y];
        Node targetNode = grid.nodes[targetPos.x, targetPos.y];

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                return RetracePath(grid, startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) * (int)(10.0f * neighbour.penalty);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    private static List<Node> RetracePath(Grid grid, Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    private static int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}

public class Node {
    public bool walkable;
    public int gridX;
    public int gridY;
    public float penalty;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(float _price, int _gridX, int _gridY) {
        walkable = _price != 0.0f;
        penalty = _price;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}

public class Point {
    public int x;
    public int y;

    public Point() {
        x = 0;
        y = 0;
    }

    public Point(int iX, int iY) {
        this.x = iX;
        this.y = iY;
    }

    public Point(Point b) {
        x = b.x;
        y = b.y;
    }

    public override int GetHashCode() {
        return x ^ y;
    }

    public override bool Equals(System.Object obj) {
        Point p = (Point)obj;

        if (ReferenceEquals(null, p)) {
            return false;
        }

        return (x == p.x) && (y == p.y);
    }

    public bool Equals(Point p) {
        if (ReferenceEquals(null, p)) {
            return false;
        }
        return (x == p.x) && (y == p.y);
    }

    public static bool operator ==(Point a, Point b) {
        if (System.Object.ReferenceEquals(a, b)) {
            return true;
        }
        if (ReferenceEquals(null, a)) {
            return false;
        }
        if (ReferenceEquals(null, b)) {
            return false;
        }
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Point a, Point b) {
        return !(a == b);
    }

    public Point Set(int iX, int iY) {
        this.x = iX;
        this.y = iY;
        return this;
    }
}

public class Grid {
    public Node[,] nodes;
    int gridSizeX, gridSizeY;

    public Grid(int width, int height, float[,] tiles_costs) {
        gridSizeX = width;
        gridSizeY = height;
        nodes = new Node[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                nodes[x, y] = new Node(tiles_costs[x, y], x, y);

            }
        }
    }


    public Grid(int width, int height, bool[,] walkable_tiles) {
        gridSizeX = width;
        gridSizeY = height;
        nodes = new Node[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                nodes[x, y] = new Node(walkable_tiles[x, y] ? 1.0f : 0.0f, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(nodes[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}