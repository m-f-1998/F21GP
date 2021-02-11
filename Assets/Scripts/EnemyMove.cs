 using UnityEngine;
 
 public class EnemyMove : MonoBehaviour {
 
    public int EnemySpeed = 10;
    public int YMoveDirection = 10;
    public bool bounce = false;

    private int XMoveDirection = 1;
    private Vector3 startPosition;

    void Start () {
        startPosition = transform.position;
    }
    
    void Update(){
        if (bounce) {
            if ((EnemySpeed < 0 && transform.position.y < startPosition.y) || (EnemySpeed > 0 && transform.position.y > startPosition.y + YMoveDirection)) EnemySpeed *= -1;
            transform.position = new Vector2(transform.position.x, transform.position.y + EnemySpeed * Time.deltaTime);
        } else {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(XMoveDirection, 0));
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(XMoveDirection, 0) * EnemySpeed;
            if (hit.distance < 1.7f && hit.collider.gameObject.tag != "Player") {
                Flip();
            }
        }
    }

    void Flip() {
        XMoveDirection = XMoveDirection > 0 ? -1 : 1;
    }
 
 }