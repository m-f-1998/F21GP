using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {
    public int playerSpeed = 10;
    private float tempY = -1;
    public int currentLevel = 1;
    private bool facingRight = false;
    public int playerJumpPower = 1250;
    public bool isGrounded = false;
    public bool canDoubleJump = false;
    private System.Random rnd = new System.Random();
    public GameObject key;
    public GameObject secretKey;
    public GameObject deadlyKey;
    private Color color;
    public Text goal;

    void Start() {
        // TODO: Dog
        //Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Dog").GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

     void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
         
    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        List<List<int>> res = new List<List<int>>(); // min x, max x, y
        foreach (string tag in Constants.spawnable) {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag(tag)) {
                var temp_res = new List<int>();
                temp_res.Add((int) Math.Ceiling(g.transform.position.x - g.transform.localScale.x / 2));
                temp_res.Add((int) Math.Ceiling(g.transform.position.x + g.transform.localScale.x / 2));
                temp_res.Add((int) Math.Ceiling(g.transform.position.y + g.transform.localScale.y / 2) + 2);
                res.Add(temp_res);
            }
        }

        int pickObject  = rnd.Next(0, res.Count);

        if (res[pickObject][0] < res[pickObject][1]) {
            // MARK: Player Spawn Position
            int pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
            transform.position = new Vector2(pickX, res[pickObject][2]);
            res.Remove(res[pickObject]);

            if (currentLevel != 2) {

                goal.text = "Goal: Collect the keys that make the colour '" + Constants.colorsStandard[currentLevel][0] + "'";
            
            }

            // MARK: NORMAL KEY

            for (int i = 0; i < Constants.keys[currentLevel]["NUM_NORMAL_KEYS"]; i++) {
                ColorUtility.TryParseHtmlString(Constants.colorsStandard[currentLevel][i + 1], out color);
                pickObject  = rnd.Next(0, res.Count);
                pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
                var prefab = Instantiate(key, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);
                prefab.GetComponent<SpriteRenderer>().color = color;
                res.Remove(res[pickObject]);
            }

            // MARK: DEADLY KEYS
            var test = (int) Math.Floor(UnityEngine.Random.Range(0f, 3f));
            ColorUtility.TryParseHtmlString(Constants.colorsDeadly[test], out color);
            for (int i = 0; i < Constants.keys[currentLevel]["NUM_DEADLY_KEYS"]; i++) {
                pickObject  = rnd.Next(0, res.Count);
                pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
                var prefab = Instantiate(deadlyKey, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);
                prefab.GetComponent<SpriteRenderer>().color = color;
                res.Remove(res[pickObject]);
            }

            //MARK: SECRET KEYS

            for (int i = 0; i < Constants.keys[currentLevel]["NUM_SECRET_KEYS"]; i++) {
                pickObject  = rnd.Next(0, res.Count);
                pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);            
                var prefab = Instantiate(secretKey, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);
                prefab.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }

    void Update() {
        if (isGrounded && Input.GetButtonDown("Jump")) {
            Jump();
            canDoubleJump = true;
        } else if (canDoubleJump && Input.GetButtonDown("Jump")) {
            Jump();
            canDoubleJump = false;
        }

        if (Input.GetAxis("Horizontal") < 0.0f && !facingRight) {
            FlipPlayer();
        } else if (Input.GetAxis("Horizontal") > 0.0f && facingRight) {
            FlipPlayer();
        }
        
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (Input.GetAxis("Horizontal") * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    void Jump() {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpPower);
        isGrounded = false;
    }

    void FlipPlayer() {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D coll){
        var temp = tempY;
        if (temp != -1) tempY = -1;
        if (temp != -1 && gameObject.transform.position.y + 25 < temp) {
            GetComponent<PlayerHealth>().Die();
        } else {
            if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "SafeGround" || coll.gameObject.tag == "SecretGround")
                isGrounded = true;
            else if (coll.gameObject.tag == "Platform") {
                transform.SetParent(coll.transform);
                isGrounded = true;
            } else if (coll.gameObject.tag == "Door") {
                if (GetComponent<PlayerScore>().GetNumKeysCollected() == Constants.keys[currentLevel]["NUM_NORMAL_KEYS"]) {
                    if (currentLevel == 3) {
                        // TODO: Show Ending Screen
                    } else {
                        GetComponent<PlayerScore>().ResetKeys();
                        currentLevel += 1;
                        SceneManager.LoadScene("Level " + currentLevel);
                    }
                } else {
                    // TODO: Show Alert - Keys Not Collected
                }
            } else if (coll.gameObject.tag == "Enemy") {
                if (coll.gameObject.GetComponent<EnemyMove>() != null && !coll.gameObject.GetComponent<EnemyMove>().bounce) {
                    Vector3 imp = coll.gameObject.transform.position - transform.position;
                    if (Mathf.Abs(imp.x) <= Mathf.Abs(imp.y)) {
                        if (imp.y <= 0) {
                            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000);
                            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200);
                            coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
                            coll.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                            coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                            coll.gameObject.GetComponent<EnemyMove>().enabled = false;
                        }
                    } else {
                        if (imp.x <= 0) {
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 120), ForceMode2D.Impulse);
                            GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10000);
                            GetComponent<PlayerHealth>().reduceHealthByOne();
                        } else {
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 120), ForceMode2D.Impulse);
                            GetComponent<Rigidbody2D>().AddForce(Vector2.left * 10000);
                            GetComponent<PlayerHealth>().reduceHealthByOne();
                        }
                    }
                } else {
                    GetComponent<PlayerHealth>().health = 0f;
                }
            }
        }
    }
    
    void OnCollisionExit2D(Collision2D coll){
        if (coll.gameObject.tag == "Platform") {
            isGrounded = false;
            tempY = gameObject.transform.position.y;
            transform.SetParent(null);
        } else if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "SafeGround" || coll.gameObject.tag == "SecretGround") {
            tempY = gameObject.transform.position.y;
            isGrounded = false;
        }
    }

}
