using UnityEngine;

public class Spring : MonoBehaviour {
    public bool x_axis = false;

    public float ySpringSpeed = 100f;
    public float xSpringSpeed = 100000f;
    
    void OnCollisionEnter2D(Collision2D coll){
        if (coll.gameObject.tag == "Player") {
            Vector3 imp = coll.gameObject.transform.position - transform.position;
            if (Mathf.Abs(imp.x) > Mathf.Abs(imp.y)) {
                if (x_axis) {
                    if (imp.x>0) {
                        coll.collider.GetComponent<Rigidbody2D>().AddForce(Vector2.right * xSpringSpeed, ForceMode2D.Force);
                    } else {
                        coll.collider.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * xSpringSpeed, ForceMode2D.Force);
                    }
                }
            } else {
                if (!x_axis && imp.y > 0) {
                    coll.collider.GetComponent<PlayerMove>().isGrounded = false;
                    coll.collider.GetComponent<PlayerMove>().canDoubleJump = false;
                    coll.collider.GetComponent<Rigidbody2D>().AddForce(Vector2.up * ySpringSpeed, ForceMode2D.Impulse);
                }
            }
        }
    }

}