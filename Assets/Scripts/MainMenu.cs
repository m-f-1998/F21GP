using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button start;
    public Button score;

    void Start() {
		start.onClick.AddListener(StartGame);
        score.onClick.AddListener(HighScore);

        if (!(PlayerPrefs.HasKey("high-score"))) {
            PlayerPrefs.SetString("high-score", "0");
        }
        if (!(PlayerPrefs.HasKey("score-level-1"))) {
            PlayerPrefs.SetString("score-level-1", "0");
        }
        if (!(PlayerPrefs.HasKey("score-level-2"))) {
            PlayerPrefs.SetString("score-level-2", "0");
        }
        if (!(PlayerPrefs.HasKey("score-level-3"))) {
            PlayerPrefs.SetString("score-level-3", "0");
        }
    }

    void StartGame() {
        SceneManager.LoadScene("Level 1");
    }

    void HighScore() {
        SceneManager.LoadScene("HighScore");
    }

}
