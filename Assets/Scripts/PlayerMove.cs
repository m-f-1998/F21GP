using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public int playerSpeed = 10;
    private bool facingRight = false;
    public int playerJumpPower = 1250;
    private float moveX;
    public bool isGrounded = false;
    public bool canDoubleJump = false;

    void Start() {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Dog").GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
        else if (coll.gameObject.tag == "Platform")
            transform.SetParent(coll.transform);
        else if (coll.gameObject.tag == "Enemy") {
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
