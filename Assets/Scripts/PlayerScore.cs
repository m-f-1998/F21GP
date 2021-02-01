using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    private float timeLeft = 120;
    public int playerScore = 0;
    public GameObject timeLeftUI;
    public GameObject playerScoreUI;

    void Update () {
        timeLeft -= Time.deltaTime;
        timeLeftUI.gameObject.GetComponent<Text>().text = ("Time Left: " + (int)timeLeft);
        playerScoreUI.gameObject.GetComponent<Text>().text = ("Score: " + playerScore);
        if (timeLeft < 0.1f) {
            // Timer Ran Out
            SceneManager.LoadScene ("Level One");
            FinishGame();
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Coin") {
            playerScore += 10 + (int) (timeLeft * 5);
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "Key") {
            playerScore += 20 + (int) (timeLeft * 5);
            //coll.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            Destroy(coll.gameObject);
        }
        if (coll.gameObject.tag == "EndLevel") {
            // Doorway Reached
            FinishGame();
        }
    }

    public void FinishGame() {
        playerScore = playerScore + (int) (timeLeft * 5);
        // Show End Screen
        Debug.Log(playerScore);
    }
}
