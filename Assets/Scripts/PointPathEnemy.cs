using System.Collections.Generic;
using UnityEngine;
using System;

public class PointPathEnemy : MonoBehaviour { //It patrols between 4 locations. It might calculate it's path each time it reaches each location.

    public Vector3[] targets;
    public int speed = 10;

    private bool[,] tilesmap;
    private Grid grid;
    private bool active;
    private int index = 0;
    private int currentTarget = -1;

    void SetBlockable() {
        foreach (String tag in Constants.blockable_tags) {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag(tag)) {
                Bounds bounds = g.GetComponent<BoxCollider2D>().bounds;
                int top = (int) Math.Round(bounds.max.y), btm = (int) Math.Round(bounds.min.y);
                int left = (int) Math.Round(bounds.min.x), right = (int) Math.Round(bounds.max.x);

                for (int j = left; j >= 0 && j <= Constants.WIDTH-1 && j < right; j++) {
                    if (btm >= 0 && btm <= Constants.HEIGHT-1) tilesmap[j, btm] = false;
                    if (top >= 0 && top <= Constants.HEIGHT-1) tilesmap[j, top] = false;
                }

                for (int j = btm; j >= 0 && j <= Constants.HEIGHT-1 && j < top; j++) {
                    if (left >= 0 && left <= Constants.WIDTH-1) tilesmap[left, j] = false;
                    if (right >= 0 && right <= Constants.WIDTH-1) tilesmap[right, j] = false;
                }
            }
        }
    }
    
    void Start() {
        tilesmap = new bool[Constants.WIDTH, Constants.HEIGHT];
        for (int i = 0; i < Constants.WIDTH; i ++)
            for (int j = 0; j < Constants.HEIGHT; j ++)
                tilesmap[i, j] = true; // true if walkable, false if not
        SetBlockable();
        grid = new Grid(Constants.WIDTH, Constants.HEIGHT, tilesmap);
    }

    void Update() {
        if (targets.Length > 0) {
            List<Point> path = new List<Point>(); // from (0, 0) to (Constants.WIDTH-1, Constants.HEIGHT-1)
            if (active && index == 0) {
                currentTarget = currentTarget == targets.Length - 1 ? 0 : currentTarget + 1; 
                path = Path.FindPath(grid, point(transform.position), point(targets[currentTarget]));
            }
            if (path.Count > 0) { // path will either be a list of Points (x, y), or an empty list if no path is found.
                var p = new Vector3(path[index].x, path[index].y, 0);
                if (transform.position != p) transform.position = Vector3.MoveTowards(transform.position, p, Time.deltaTime * speed);
                else index = index < path.Count - 1 ? index + 1 : 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") active = true;
    }

    void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.tag == "Player") active = false;
    }

    private Point point(Vector3 pos) {
        return new Point((int) Math.Round(pos.x, 0), (int) Math.Round(pos.y, 0));
    }

}