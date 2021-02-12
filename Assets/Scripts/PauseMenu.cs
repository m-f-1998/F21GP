using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public Text score;
    public GameObject panel;
    public GameObject player;
    public static bool GameIsPaused = false;

    void Start() {
        panel.SetActive(GameIsPaused);
        score.text = "Current Score: " + player.GetComponent<PlayerScore>().totalScore;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        GameIsPaused = false;
        panel.SetActive(GameIsPaused);
        Time.timeScale = 1f;
    }

    void Pause() {
        score.text = "Current Score: " + player.GetComponent<PlayerScore>().totalScore;
        GameIsPaused = true;
        panel.SetActive(GameIsPaused);
        Time.timeScale = 0f;
    }

}
