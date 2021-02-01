using UnityEngine;

public class EnemyHealth: MonoBehaviour {

    void Update() {
        if (gameObject.transform.position.y < -10) {
            Destroy(gameObject);
        }
    }

}