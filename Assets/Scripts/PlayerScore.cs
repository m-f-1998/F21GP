using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public float timeLeft = 120;
    public int levelScore = 0;
    public int totalScore = 0;
    public GameObject timeLeftUI;
    public GameObject playerScoreUI;
    private int numKeysCollected = 0;
    public AudioClip coin;
    public AudioClip secretKey;
    public AudioClip key;
    public Animator animator;

    public int GetNumKeysCollected() {
        return numKeysCollected;
    }

    public void ResetKeys() {
        animator.SetBool("OpenDoor", false);
        numKeysCollected = 0;
        levelScore = 0;
    }

    void Update () {
        timeLeft -= Time.deltaTime;
        timeLeftUI.gameObject.GetComponent<Text>().text = ("Time Left: " + (int)timeLeft);
        playerScoreUI.gameObject.GetComponent<Text>().text = ("Score: " + totalScore);
        GetComponent<AudioSource> ().playOnAwake = false;
        if (totalScore < 0) GetComponent<PlayerMove>().deathReason.text = "Level Failed: A Negative Score - Really?";
        if (timeLeft < 0.1f) GetComponent<PlayerMove>().deathReason.text = "Level Failed: Timer Expired";
        if (timeLeft < 0.1f || totalScore < 0) GetComponent<PlayerHealth>().Die();
    }

    void DisableText() {
        GetComponent<PlayerMove>().alert.text = "";
    }
    
    void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.tag == "Coin") {
            GetComponent<AudioSource> ().clip = coin;
            GetComponent<AudioSource> ().Play ();
            totalScore += 10 + (int) (timeLeft * 5);
            levelScore += 10 + (int) (timeLeft * 5);
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "SecretKey") {
            GetComponent<PlayerMove>().alert.text = "Secret Area Unlocked!";
            Invoke("DisableText", 2.5f);
            GetComponent<AudioSource> ().clip = secretKey;
            GetComponent<AudioSource> ().Play ();
            foreach (GameObject secretWall in GameObject.FindGameObjectsWithTag("SecretWall")) {
                Destroy(secretWall);
            }
            // Show Secret Unlock Animation
            Destroy(coll.gameObject);
        }
        if(coll.gameObject.tag == "DeadlyKey") {
            Time.timeScale = 0f;
            GetComponent<PlayerMove>().deathReason.text = "Level Failed: Watch Out For That Deadly Key!";
            GetComponent<PlayerHealth>().Die();
        }
        if(coll.gameObject.tag == "Key") {
            GetComponent<AudioSource> ().clip = key;
            GetComponent<AudioSource> ().Play ();
            numKeysCollected += 1;
            if (GetNumKeysCollected() == Constants.keys[GetComponent<PlayerMove>().currentLevel]["NUM_NORMAL_KEYS"]) {
                animator.SetBool("OpenDoor", true);
            }
            totalScore += 20 + (int) (timeLeft * 5);
            levelScore += 20 + (int) (timeLeft * 5);
            Destroy(coll.gameObject);
        }
    }

    public void FinishGame() {
        totalScore = totalScore + (int) (timeLeft * 5);
        if (int.Parse(PlayerPrefs.GetString("high-score")) < totalScore)
            PlayerPrefs.SetString("high-score", totalScore.ToString());
    }
}
