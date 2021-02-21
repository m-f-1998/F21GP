/**
 * @author Matthew Frankland
 * @email [developer@matthewfrankland.co.uk]
 * @create date 07-02-2021 20:38:32
 * @modify date 19-02-2021 09:26:22
 * @desc [Flock behaviour]
 */

using UnityEngine;

public class Bird : MonoBehaviour {

    public GameObject bird;
    public int spawnCount = 5;
    public float spawnRadius = 7.0f;
    public LayerMask searchLayer;

    [Range(0.1f, 20.0f)]
    public float velocity = 6.0f, rotationCoeff = 4.0f;

    [Range(0.0f, 0.9f)]
    public float velocityVariation = 0.9f;

    [Range(0.1f, 10.0f)]
    public float dist = 8.0f;

    private bool coin = false;

    void Start() {
        Invoke("Spawn", Random.Range(Random.Range(10.0f, 30.0f), Random.Range(10.0f, 15.0f)));
        Invoke("Coin", 50f);
    }

    public void Spawn() {
        for (var i = 0; i < spawnCount; i++) Spawn(transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius);
        Invoke("Spawn", Random.Range(Random.Range(10.0f, 30.0f), Random.Range(10.0f, 15.0f)));
    }

    public void Coin() {
        coin = true;
        Invoke("Coin", 50f);
    }

    public GameObject Spawn(Vector3 position) {
        var rotation = Quaternion.Slerp(transform.rotation, UnityEngine.Random.rotation, 0.3f);
        var boid = Instantiate(bird, position, rotation) as GameObject;
        if (coin) {
            boid.GetComponent<BirdBehaviour>().coin = true;
            coin = false;
        }
        boid.GetComponent<BirdBehaviour>().controller = this;
        boid.GetComponent<BirdBehaviour>().setChild();
        return boid;
    }
}