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
        GetComponent<PlayerScore>().totalScore -= 5 + (int) (GetComponent<PlayerScore>().timeLeft * 2.5);
        GetComponent<PlayerScore>().levelScore -= 5 + (int) (GetComponent<PlayerScore>().timeLeft * 2.5);
    }

    public void Die () {
        // TODO: Show Death Animation
        // Show Score Screen Over Top
        Debug.Log("Player Died");
        SceneManager.LoadScene("Level 1");
    }
}
