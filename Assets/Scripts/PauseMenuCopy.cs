using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuCopy : MonoBehaviour {

    public Button pause;
    public bool _paused = false;

    void Start() {
		pause.onClick.AddListener(pauseGame);

        if (!(PlayerPrefs.HasKey("high-score"))) {
            PlayerPrefs.SetString("high-score", "0");
        }        
    }

    void PauseGame() {
        _paused = true;
        Debug.Log("Paused Called");
        SceneManager.LoadScene ("PauseMenu", LoadSceneMode.Additive);
    }

}
