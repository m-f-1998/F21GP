 using UnityEngine;
 
 public class EnemyMove : MonoBehaviour {
 
    public int EnemySpeed;
    public int XMoveDirection;

    public bool bounce = false;
    private float speed = 10f;
    private float distance = 10f;
    private Vector3 startPosition;

    void Start () {
        startPosition = transform.position;
    }
    
    void Update(){
        if (bounce) {
            if ((speed < 0 && transform.position.y < startPosition.y) || (speed > 0 && transform.position.y > startPosition.y + distance)) speed *= -1;
            transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
        } else {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(XMoveDirection, 0));
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection, 0) * EnemySpeed;
            if (hit.distance < 1.7f) {
                Flip();
            }
        }
    }

    void Flip() {
        if (XMoveDirection > 0) {
            XMoveDirection = -1;
        } else {
            XMoveDirection = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D coll){
        if (coll.gameObject.tag == "Player") {
            coll.collider.GetComponent<PlayerHealth>().health = 0f;
        }
    }
 
 }