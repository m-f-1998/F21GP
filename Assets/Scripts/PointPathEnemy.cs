using System.Collections.Generic;
using UnityEngine;

public class PointPathEnemy : MonoBehaviour {

    public int speed = 10;
    public LayerMask unwalkableMask;
    public Vector3[] directions = new Vector3[4];

    private bool[,] tilesmap;
    private Grid grid;
    private bool newPath = true;
    private int index = 0;

    void Start() {
        tilesmap = new bool[Constants.WIDTH, Constants.HEIGHT];
        for (int i = 0; i < Constants.WIDTH; i ++)
            for (int j = 0; j < Constants.HEIGHT; j ++) {
                tilesmap[i, j] = !(Physics2D.OverlapCircle(new Vector2(i, j), 0.5f, unwalkableMask)); // true if walkable, false if not
            }
        grid = new Grid(Constants.WIDTH, Constants.HEIGHT, tilesmap);
    }

    void Update() {
        List<Vector2> path = new List<Vector2>(); // from (0, 0) to (Constants.WIDTH-1, Constants.HEIGHT-1)
        if (newPath) {
            Path.FindPath(grid, transform.position, directions[index], ref path);
            newPath = false;
        }
        if (path.Count > 0) { // path will either be a list of Points (x, y), or an empty list if no path is found.
            var p = new Vector3(path[index].x, path[index].y, 0);
            if (transform.position != p) transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
            else {
                index = index == 3 ? 0 : index + 1;
                newPath = true;
            }
        } else {
            newPath = true;
        }
    }

}