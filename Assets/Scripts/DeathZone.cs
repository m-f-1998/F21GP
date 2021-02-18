using UnityEngine;

public class DeathZone : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.tag == "Player") {
            coll.GetComponent<PlayerMove>().deathReason.text = "You Died: It's Difficult To Swim";
            Time.timeScale = 0f;
            coll.GetComponent<PlayerHealth>().Die();
        }
    }
}
