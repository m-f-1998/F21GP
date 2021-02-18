using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour {
    public Text levelOneScore;
    public Text levelTwoScore;
    public Text levelThreeScore;
    public Text totalScore;
    public Button back;

    void Start() {
        back.onClick.AddListener(BackButton);
        totalScore.text = PlayerPrefs.GetString("high-score");
        levelOneScore.text = PlayerPrefs.GetString("score-level-1");
        levelTwoScore.text = PlayerPrefs.GetString("score-level-2");
        levelThreeScore.text = PlayerPrefs.GetString("score-level-3");
    }

    public void BackButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
