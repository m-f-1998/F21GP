using UnityEngine;

public class BoidController : MonoBehaviour {
    public GameObject boidPrefab;

    public int spawnCount = 5;

    public float spawnRadius = 7.0f;

    [Range(0.1f, 20.0f)]
    public float velocity = 6.0f;

    [Range(0.0f, 0.9f)]
    public float velocityVariation = 0.9f;

    [Range(0.1f, 20.0f)]
    public float rotationCoeff = 4.0f;

    [Range(0.1f, 10.0f)]
    public float neighborDist = 8.0f;
    public LayerMask searchLayer;

    void Start() {
        Invoke("Spawn", Random.Range(Random.Range(1.0f, 10.0f), Random.Range(10.0f, 15.0f)));
    }

    public void Spawn() {
        for (var i = 0; i < spawnCount; i++) Spawn(transform.position + UnityEngine.Random.insideUnitSphere * spawnRadius);
        Invoke("Spawn", Random.Range(Random.Range(1.0f, 10.0f), Random.Range(10.0f, 15.0f)));
    }

    public GameObject Spawn(Vector3 position) {
        var rotation = Quaternion.Slerp(transform.rotation, UnityEngine.Random.rotation, 0.3f);
        var boid = Instantiate(boidPrefab, position, rotation) as GameObject;
        boid.GetComponent<BoidBehaviour>().controller = this;
        boid.GetComponent<BoidBehaviour>().isChild = true;
        return boid;
    }
}