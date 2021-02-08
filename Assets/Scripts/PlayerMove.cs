using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class PlayerMove : MonoBehaviour {
    public int playerSpeed = 10;
    private bool facingRight = false;
    public int playerJumpPower = 1250;
    public bool isGrounded = false;
    public bool canDoubleJump = false;
    private System.Random rnd = new System.Random();
    public GameObject key;


    void Start() {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Dog").GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
                temp_res.Add((int) Math.Ceiling(g.transform.position.y + g.transform.localScale.y / 2));
                res.Add(temp_res);
            }
        }
        int pickObject  = rnd.Next(0, res.Count);
        int pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(pickX, res[pickObject][2]), new Vector2(5, 0));
        while (hit.distance < 1.7f && (hit.collider == null || hit.collider.gameObject.tag == "Enemy")) {
            pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
            hit = Physics2D.Raycast(new Vector2(pickX, res[pickObject][2]), new Vector2(10, 0));
        }
        transform.position = new Vector2(pickX, res[pickObject][2]);

        res.Remove(res[pickObject]);

        pickObject  = rnd.Next(0, res.Count);
        pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
        Instantiate(key, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);

        pickObject  = rnd.Next(0, res.Count);
        pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
        Instantiate(key, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);

        pickObject  = rnd.Next(0, res.Count);
        pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
        Instantiate(key, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);

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
        if (coll.gameObject.tag == "Ground")
            isGrounded = true;
        else if (coll.gameObject.tag == "Platform") {
            transform.SetParent(coll.transform);
            isGrounded = true;
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
                    } else {
                        GetComponent<PlayerHealth>().health = 0f;
                    }
                } else {
                    GetComponent<PlayerHealth>().health = 0f;
                }
            } else {
                GetComponent<PlayerHealth>().health = 0f;
            }
        }
    }
    
    void OnCollisionExit2D(Collision2D coll){
        if (coll.gameObject.tag == "Platform") {
            transform.SetParent(null);
        }
    }

}
