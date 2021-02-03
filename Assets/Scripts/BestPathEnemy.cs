using System.Collections.Generic;
using UnityEngine;
using System;

public class BestPathEnemy : MonoBehaviour {

    //Lets say I have a mob that patrols. It patrols between 4 locations. It might calculate it's path each time it reaches each location.

    private bool[,] tilesmap;
    private Grid grid;
    public GameObject target;
    private bool active;
    private long time = -1;
    private int index = 0;
    private List<Point> currentBestPath;
    private float Timer = 0;
    Point currentPositionHolder;
    private Vector3 startPosition;
    
    void Start() {
        tilesmap = new bool[Constants.WIDTH, Constants.HEIGHT];
        Timer = 0;
        startPosition = transform.position;

        for (int i = 0; i < Constants.WIDTH; i ++) {
            for (int j = 0; j < Constants.HEIGHT; j ++) {
                tilesmap[i, j] = true; // true if walkable, false if not
            }
        }

        grid = new Grid(Constants.WIDTH, Constants.HEIGHT, tilesmap);
    }

    void Update() {
        if (active && (time == -1 || (time + 20) < DateTimeOffset.Now.ToUnixTimeSeconds())) {
            index = 0;
            Timer = 0;
            RunBestPath(); // get path
            currentPositionHolder = currentBestPath[index];
        }
        if (currentBestPath != null && currentBestPath.Count > 0) {
            // path will either be a list of Points (x, y), or an empty list if no path is found.
            Timer += Time.deltaTime * 100f;
            if (transform.position != new Vector3(currentPositionHolder.x, currentPositionHolder.y, 0)) {
                Time.timeScale = .5f;
                transform.position = Vector3.Lerp(startPosition, new Vector3(currentPositionHolder.x, currentPositionHolder.y, 0), Timer);
                Time.timeScale = 1f;
            } else {
                if (index < currentBestPath.Count - 1) {
                    index++;
                    currentPositionHolder = currentBestPath[index];
                    Timer = 0;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            active = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.tag == "Player") {
            active = false;
            time = -1;
        }
    }

    void RunBestPath() {
        int _startX, _startY, _endX, _endY;
        _startX = (int) Math.Round(gameObject.transform.position.x, 0);
        _endX = (int) Math.Round(target.transform.position.x, 0);
        _startY = (int) Math.Round(gameObject.transform.position.y, 0);
        _endY = (int) Math.Round(target.transform.position.y, 0);

        if (gameObject.transform.position.x < 0 || target.transform.position.x < 0 || gameObject.transform.position.y < 0 || target.transform.position.y < 0) {
            // TO DO: Throw alert, kill game
            Debug.Log("Best Path Enemy Outside Minimum Target Graph!!");
        }
        
        if (gameObject.transform.position.x > Constants.HEIGHT - 1 || target.transform.position.x > Constants.HEIGHT - 1 || gameObject.transform.position.y > Constants.HEIGHT - 1 || target.transform.position.y > Constants.HEIGHT - 1) {
            // TO DO: Throw alert, kill game
            Debug.Log("Best Path Enemy Outside Target Graph!!");
        }

        // from (0, 0) to (Constants.WIDTH-1, Constants.HEIGHT-1)
        Point _from = new Point(_startX, _startY);
        Point _to = new Point(_endX, _endY);
        
        time = DateTimeOffset.Now.ToUnixTimeSeconds();

        currentBestPath = Pathfinding.FindPath(grid, _from, _to);
    }

}