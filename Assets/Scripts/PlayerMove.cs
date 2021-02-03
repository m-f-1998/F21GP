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
        PlayerRaycast();
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
    }
    
    void OnCollisionExit2D(Collision2D coll){
        if (coll.gameObject.tag == "Platform") {
            transform.SetParent(null);
        }
    }

    void PlayerRaycast() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit.collider.GetComponent<EnemyMove>() != null && !hit.collider.GetComponent<EnemyMove>().bounce)
            if (hit.collider != null && hit.distance < 0.9f && hit.collider.tag == "Enemy") {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * 1000);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 200);
                hit.collider.gameObject.GetComponent<Rigidbody2D>().gravityScale = 20;
                hit.collider.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
                hit.collider.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                hit.collider.gameObject.GetComponent<EnemyMove>().enabled = false;
            }
    }
}
