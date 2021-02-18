using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public Text score;
    public GameObject panel;
    public GameObject player;
    public Camera camera;
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
        camera.GetComponent<AudioSource>().Play();
        GameIsPaused = false;
        panel.SetActive(GameIsPaused);
        Time.timeScale = 1f;
    }

    void Pause() {
        camera.GetComponent<AudioSource>().Pause();
        score.text = "Current Score: " + player.GetComponent<PlayerScore>().totalScore;
        GameIsPaused = true;
        panel.SetActive(GameIsPaused);
        Time.timeScale = 0f;
    }

}
