using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public Button start;
    public Text score;

    void Start() {
		start.onClick.AddListener(StartGame);

        if (!(PlayerPrefs.HasKey("high-score"))) {
            PlayerPrefs.SetString("high-score", "0");
        }
        
        score.text = "High Score: " + PlayerPrefs.GetString("high-score");
    }

    void StartGame() {
        SceneManager.LoadScene("Level One");
    }

}
