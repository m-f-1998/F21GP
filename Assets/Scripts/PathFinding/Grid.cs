/**
 * @author Matthew Frankland
 * @email [developer@matthewfrankland.co.uk]
 * @create date 10-02-2021 18:41:10
 * @modify date 19-02-2021 09:26:22
 * @desc [A* Pathfinding - Grid]
 */

using System.Collections.Generic;

public class Grid {
    private Node[,] nodes;
    private int x, y;

    public Grid(int width, int height, bool[,] walkable) {
        x = width;
        y = height;
        nodes = new Node[width, height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                nodes[i, j] = new Node(walkable[i, j] ? 1.0f : 0.0f, i, j);
    }

    public int GetSize() {
		return x * y;
	}

    public Node GetNode(int x, int y) {
        return nodes[x, y];
    }

    public List<Node> GetNeighbours(Node node) { 
        List<Node> neighbours = new List<Node>();
        CheckNeighbours(-1, -1, node, ref neighbours); // bottom-left
        CheckNeighbours(-1, 1, node, ref neighbours); // top-left
        CheckNeighbours(1, -1, node, ref neighbours); // bottom-right
        CheckNeighbours(1, 1, node, ref neighbours); // bottom-left
        return neighbours;
    }

    private void CheckNeighbours(int i, int j, Node node, ref List<Node> neighbours) {
        int checkX = node.GetXPos() + i, checkY = node.GetYPos() + j;
        if (checkX >= 0 && checkX < x && checkY >= 0 && checkY < y) {
            neighbours.Add(nodes[checkX, checkY]);
        }
    }
}