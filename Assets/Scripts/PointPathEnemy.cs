using System.Collections.Generic;
using UnityEngine;

public class PointPathEnemy : MonoBehaviour {

    public int speed = 10;
    public LayerMask unwalkableMask;
    public Vector3[] directions = new Vector3[4];
    public bool reverse = false;

    private bool[,] tilesmap;
    private Grid grid;
    private bool newPath = true;
    private int pathIndex = 0;
    private int goalIndex = 0;
    private bool flag = false;
    private List<Vector2> path = new List<Vector2>(); // from (0, 0) to (Constants.WIDTH-1, Constants.HEIGHT-1)

    void Start() {
        tilesmap = new bool[Constants.WIDTH, Constants.HEIGHT];
        for (int i = 0; i < Constants.WIDTH; i ++)
            for (int j = 0; j < Constants.HEIGHT; j ++) {
                tilesmap[i, j] = !(Physics2D.OverlapCircle(new Vector2(i, j), 0.5f, unwalkableMask)); // true if walkable, false if not
            }
        grid = new Grid(Constants.WIDTH, Constants.HEIGHT, tilesmap);
    }

    void Update() {
        if ((!reverse && goalIndex < directions.Length) || reverse) {
            if (newPath) {
                Path.FindPath(grid, transform.position, directions[goalIndex], ref path);
                newPath = false;
            }
            var p = new Vector3(path[pathIndex].x, path[pathIndex].y, 0);
            if (transform.position != p) {
                transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
            } else {
                if (pathIndex < path.Count-1) {
                    pathIndex++;
                } else {
                    if (reverse && goalIndex == directions.Length - 1) { flag = true; } else if (reverse && goalIndex == 0) { flag = false; }
                    if (!flag) { goalIndex++; } else { goalIndex--; }
                    pathIndex = 0;
                    newPath = true;
                    path = new List<Vector2>();
                }
                
            }
        }
    }

}