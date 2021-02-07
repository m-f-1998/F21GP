using UnityEngine;
using System.Collections.Generic;
using System;

public class Path {
    public static List<Vector2> FindPath(Grid grid, Vector2 pos1, Vector2 pos2, ref List<Vector2> path) {
        Node startNode = grid.nodes[(int) Math.Ceiling(pos1.x), (int) Math.Ceiling(pos1.y)];
        Node targetNode = grid.nodes[(int) Math.Ceiling(pos2.x), (int) Math.Ceiling(pos2.y)];

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);    

        while (openSet.Count > 0) {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                while (currentNode != startNode) {
                    path.Add(new Vector2(currentNode.gridX, currentNode.gridY));
                    currentNode = currentNode.parent;
                }
                path.Reverse();
                break;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;
                
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

    public int gCost;
    public int hCost;
    public Node parent;
    public int heapIndex;

    public Node(float _price, int _gridX, int _gridY) {
        walkable = _price != 0.0f;
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

public class Grid {
    public Node[,] nodes;
    int gridSizeX, gridSizeY;

    public Grid(int width, int height, int[,] tiles_costs) {
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