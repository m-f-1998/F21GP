using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    public float health;
    public GameObject deathPanel;
    public Camera camera;
    public AudioClip died;
    public AudioClip hit;

    void Start () {
        health = 3f;
        deathPanel.SetActive(false);
    }

    public void reduceHealthByOne () {
        health--;
        if (health == 0f) {
            GetComponent<PlayerMove>().deathReason.text = "You Died: No Health Left";
            Time.timeScale = 0f;
            Die();
        } else {
            GetComponent<AudioSource> ().clip = hit;
            GetComponent<AudioSource> ().Play ();
            if (GetComponent<PlayerScore>().totalScore - 5 + (int) (GetComponent<PlayerScore>().timeLeft * 2.5) < 0) {
                GetComponent<PlayerScore>().totalScore -= 5 + (int) (GetComponent<PlayerScore>().timeLeft * 2.5);
                GetComponent<PlayerScore>().levelScore -= 5 + (int) (GetComponent<PlayerScore>().timeLeft * 2.5);
            }
        }
    }

    public void Die () {
        GetComponent<AudioSource>().clip = died;
        GetComponent<AudioSource>().Play ();
        deathPanel.SetActive(true);
        camera.GetComponent<AudioSource>().Pause();
        Debug.Log("Player Died");
    }
}
