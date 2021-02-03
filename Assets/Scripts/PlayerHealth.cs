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

    void Die () {
        GetComponent<PlayerScore>().FinishGame();
        // Show Score Screen Over Top
        SceneManager.LoadScene("Level One");
        Debug.Log("Player Died");
    }
}
