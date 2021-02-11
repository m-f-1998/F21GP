using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public Button resume;
    public Text score;

    void Start() {
		resume.onClick.AddListener(resumeGame);

        if (!(PlayerPrefs.HasKey("high-score"))) {
            PlayerPrefs.SetString("high-score", "0");
        }
        
        score.text = "High Score: " + PlayerPrefs.GetString("high-score");
    }

    void resumeGame() {
        GetComponent<PauseMenuCopy>()._paused = false;
        SceneManager.UnloadSceneAsync ("PauseMenu");
    }

}
