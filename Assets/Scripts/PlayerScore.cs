using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public float timeLeft = 120;
    public int playerScore = 0;
    public GameObject timeLeftUI;
    public GameObject playerScoreUI;

    void Update () {
        timeLeft -= Time.deltaTime;
        timeLeftUI.gameObject.GetComponent<Text>().text = ("Time Left: " + (int)timeLeft);
        playerScoreUI.gameObject.GetComponent<Text>().text = ("Score: " + playerScore);
        if (timeLeft < 0.1f || playerScore < 0) {
            // Timer Ran Out
            GetComponent<PlayerHealth>().Die();
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Coin") {
            playerScore += 10 + (int) (timeLeft * 5);
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "SecretKey") {
            GameObject secretWall = GameObject.FindGameObjectWithTag("SecretWall");
            if (secretWall != null) {
                Destroy(secretWall);
                // Show Secret Unlock
            }
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "DeadlyKey") {
            GetComponent<PlayerHealth>().Die();
        }
        if(coll.gameObject.tag == "Key") {
            playerScore += 20 + (int) (timeLeft * 5);
            //coll.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            Destroy(coll.gameObject);
        }
    }

    public void FinishGame() {
        playerScore = playerScore + (int) (timeLeft * 5);
        if (int.Parse(PlayerPrefs.GetString("high-score")) < playerScore)
            PlayerPrefs.SetString("high-score", playerScore.ToString());
        // Show End Screen
        Debug.Log(playerScore);
    }
}
