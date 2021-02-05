using UnityEngine;
using System.Collections.Generic;
using System;

public class Path {
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

        //Debug.Log(startPos.x + " " + startPos.y);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                return RetracePath(grid, startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour))continue;
                
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
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

public class Node : IHeapItem<Node> {
    public bool walkable;
    public int gridX;
    public int gridY;
    public float penalty;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

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

    public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare) {
		int compare = fCost.CompareTo(nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
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

    public int MaxSize {
		get {
			return gridSizeX * gridSizeY;
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

public class Heap<T> where T : IHeapItem<T> {
	
	T[] items;
	int currentItemCount;
	
	public Heap(int maxHeapSize) {
		items = new T[maxHeapSize];
	}
	
	public void Add(T item) {
		item.HeapIndex = currentItemCount;
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}

	public T RemoveFirst() {
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
		items[0].HeapIndex = 0;
		SortDown(items[0]);
		return firstItem;
	}

	public void UpdateItem(T item) {
		SortUp(item);
	}

	public int Count {
		get {
			return currentItemCount;
		}
	}

	public bool Contains(T item) {
		return Equals(items[item.HeapIndex], item);
	}

	void SortDown(T item) {
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;
			int swapIndex = 0;

			if (childIndexLeft < currentItemCount) {
				swapIndex = childIndexLeft;

				if (childIndexRight < currentItemCount) {
					if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				if (item.CompareTo(items[swapIndex]) < 0) {
					Swap (item,items[swapIndex]);
				}
				else {
					return;
				}

			}
			else {
				return;
			}

		}
	}
	
	void SortUp(T item) {
		int parentIndex = (item.HeapIndex-1)/2;
		
		while (true) {
			T parentItem = items[parentIndex];
			if (item.CompareTo(parentItem) > 0) {
				Swap (item,parentItem);
			}
			else {
				break;
			}

			parentIndex = (item.HeapIndex-1)/2;
		}
	}
	
	void Swap(T itemA, T itemB) {
		items[itemA.HeapIndex] = itemB;
		items[itemB.HeapIndex] = itemA;
		int itemAIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get;
		set;
	}
}