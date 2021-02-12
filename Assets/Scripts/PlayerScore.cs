using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public float timeLeft = 120;
    public int levelScore = 0;
    public int totalScore = 0;
    public GameObject timeLeftUI;
    public GameObject playerScoreUI;
    private int numKeysCollected = 0;

    public int GetNumKeysCollected() {
        return numKeysCollected;
    }

    public void ResetKeys() {
        numKeysCollected = 0;
    }

    void Update () {
        timeLeft -= Time.deltaTime;
        timeLeftUI.gameObject.GetComponent<Text>().text = ("Time Left: " + (int)timeLeft);
        playerScoreUI.gameObject.GetComponent<Text>().text = ("Score: " + totalScore);
        if (timeLeft < 0.1f || totalScore < 0) {
            // Timer Ran Out
            GetComponent<PlayerHealth>().Die();
            // TODO: Show Alert Screen
        }
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Coin") {
            totalScore += 10 + (int) (timeLeft * 5);
            levelScore += 10 + (int) (timeLeft * 5);
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "SecretKey") {
            foreach (GameObject secretWall in GameObject.FindGameObjectsWithTag("SecretWall")) {
                Destroy(secretWall);
            }
            // Show Secret Unlock Animation
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "DeadlyKey") {
            GetComponent<PlayerHealth>().Die();
        }
        if(coll.gameObject.tag == "Key") {
            totalScore += 20 + (int) (timeLeft * 5);
            levelScore += 20 + (int) (timeLeft * 5);
            Destroy(coll.gameObject);
        }
    }

    public void FinishGame() {
        totalScore = totalScore + (int) (timeLeft * 5);
        if (int.Parse(PlayerPrefs.GetString("high-score")) < totalScore)
            PlayerPrefs.SetString("high-score", totalScore.ToString());
        Debug.Log(totalScore);
    }
}
