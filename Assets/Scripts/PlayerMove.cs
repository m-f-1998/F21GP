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

    public Button nextLevel;
    public GameObject nextLevelPanel;
    public GameObject[] stars;
    public Text score;
    public Text alert;

    public Text deathReason;
    public Button restartLevel;

    public Animator animator;

    public Camera camera;
    public AudioClip deathJump;

    void Start() {
        nextLevelPanel.SetActive(false);
        nextLevel.onClick.AddListener(NextLevel);
        restartLevel.onClick.AddListener(RestartLevel);
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
                temp_res.Add((int) Math.Ceiling(g.transform.position.y + g.transform.localScale.y / 2) + 1);
                res.Add(temp_res);
            }
        }

        int pickObject  = rnd.Next(0, res.Count);

        if (res[pickObject][0] < res[pickObject][1]) {
            int pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
            //transform.position = new Vector2(pickX, res[pickObject][2]);
            //res.Remove(res[pickObject]);
            int goalColor = rnd.Next(0, Constants.colorsStandard.Count);

            if (currentLevel != 2) {
                goal.text = "Goal: Collect the keys that make the colour '" + Constants.colorsStandard[goalColor][0] + "'";
            }

            for (int i = 0; i < Constants.keys[currentLevel]["NUM_NORMAL_KEYS"]; i++) {
                ColorUtility.TryParseHtmlString(Constants.colorsStandard[goalColor][i + 1], out color);
                pickObject  = rnd.Next(0, res.Count);
                pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
                var prefab = Instantiate(key, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);
                prefab.GetComponent<SpriteRenderer>().color = color;
                res.Remove(res[pickObject]);
            }

            var test = (int) Math.Floor(UnityEngine.Random.Range(0f, 3f));
            ColorUtility.TryParseHtmlString(Constants.colorsDeadly[test], out color);
            for (int i = 0; i < Constants.keys[currentLevel]["NUM_DEADLY_KEYS"]; i++) {
                pickObject  = rnd.Next(0, res.Count);
                pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);
                var prefab = Instantiate(deadlyKey, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);
                prefab.GetComponent<SpriteRenderer>().color = color;
                res.Remove(res[pickObject]);
            }

            for (int i = 0; i < Constants.keys[currentLevel]["NUM_SECRET_KEYS"]; i++) {
                pickObject  = rnd.Next(0, res.Count);
                pickX  = rnd.Next(res[pickObject][0], res[pickObject][1]);            
                var prefab = Instantiate(secretKey, new Vector2(pickX, res[pickObject][2]), Quaternion.identity);
                prefab.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
    }

    void Update() {
        animator.SetFloat("Speed", Math.Abs(Input.GetAxis("Horizontal") * playerSpeed));
        if (isGrounded && Input.GetButtonDown("Jump")) {
            Jump();
            animator.SetBool("IsJumping", true);
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

    void RestartLevel() {
        GetComponent<PlayerScore>().ResetKeys();
        SceneManager.LoadScene("Level " + currentLevel);
        camera.GetComponent<AudioSource>().Play();
    }
    
    void NextLevel() {
        if (currentLevel == 3) {
            GetComponent<PlayerScore>().FinishGame();
            SceneManager.LoadScene("Main Menu");
        } else {
            GetComponent<PlayerScore>().ResetKeys();
            currentLevel += 1;
            SceneManager.LoadScene("Level " + currentLevel);
            camera.GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Exit") {
            if (GetComponent<PlayerScore>().GetNumKeysCollected() == Constants.keys[currentLevel]["NUM_NORMAL_KEYS"]) {
                if (int.Parse(PlayerPrefs.GetString("score-level-" + currentLevel)) < GetComponent<PlayerScore>().levelScore)
                    PlayerPrefs.SetString("score-level-" + currentLevel, GetComponent<PlayerScore>().levelScore.ToString());
                nextLevelPanel.SetActive(true);
                camera.GetComponent<AudioSource>().Pause();
                score.text = "Score: " + GetComponent<PlayerScore>().levelScore.ToString();
                Time.timeScale = 0f;
                for (int i = 0; i < 2 /* num starts */; i++) {
                    stars[i].SetActive(true);
                }
            } else {
                alert.text = "You're Missing Some Keys!";
                Invoke("DisableText", 2.5f);
            }
        }
    }

    void DisableText() {
        alert.text = "";
    }

    void OnCollisionEnter2D(Collision2D coll){
        var temp = tempY;
        if (temp != -1) tempY = -1;
        if (temp != -1 && gameObject.transform.position.y + 25 < temp) {
            deathReason.text = "You Died: Fell From A Great Hight";
            GetComponent<PlayerHealth>().Die();
        } else {
            if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "SafeGround" || coll.gameObject.tag == "SecretGround") {
                isGrounded = true;
                animator.SetBool("IsJumping", false);
            } else if (coll.gameObject.tag == "Platform") {
                transform.SetParent(coll.transform);
                isGrounded = true;
                animator.SetBool("IsJumping", false);
            } else if (coll.gameObject.tag == "Enemy") {
                Vector3 imp = coll.gameObject.transform.position - transform.position;
                var destroyEnemy = coll.gameObject.GetComponent<StandardEnemy>() != null && !coll.gameObject.GetComponent<StandardEnemy>().yAxis && Mathf.Abs(imp.x) <= Mathf.Abs(imp.y) && imp.y <= 0;
                if (destroyEnemy) {
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000);
                    coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200);
                    coll.gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
                    coll.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                    coll.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    coll.gameObject.GetComponent<StandardEnemy>().enabled = false;
                    GetComponent<AudioSource> ().clip = deathJump;
                    GetComponent<AudioSource> ().Play ();
                } else {
                    Vector2 forceDir = imp.x <= 0 ? Vector2.right : Vector2.left;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 120), ForceMode2D.Impulse);
                    GetComponent<Rigidbody2D>().AddForce(forceDir * 10000);
                    GetComponent<PlayerHealth>().reduceHealthByOne();
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
