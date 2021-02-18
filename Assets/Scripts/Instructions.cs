using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour {
    public Button back;

    void Start() {
        back.onClick.AddListener(BackButton);
    }

    public void BackButton() {
        SceneManager.LoadScene("MainMenu");
    }
}
