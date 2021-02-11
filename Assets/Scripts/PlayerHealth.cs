using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public float health;

    void Start() {
        health = 3f;
    }

    void Update() {
        if (health == 0f) Die();
    }

    public void reduceHealthByOne() {
        health--;
        GetComponent<PlayerScore>().playerScore -= 5 + (int) (GetComponent<PlayerScore>().timeLeft * 2.5);
    }

    public void Die () {
        GetComponent<PlayerScore>().FinishGame();
        // Show Score Screen Over Top
        Debug.Log("Player Died");
        SceneManager.LoadScene("Level 1");
    }
}
