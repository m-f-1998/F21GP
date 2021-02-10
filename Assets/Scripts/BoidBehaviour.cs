using UnityEngine;

public class BoidBehaviour : MonoBehaviour
{
    // Reference to the controller.
    public BoidController controller;

    public CameraSystem cameraTest;

    // Random seed.
    float noiseOffset;
    
    public bool isChild = false;
    public bool isMin = false;

    // Caluculates the separation vector with a target.
    Vector3 GetSeparationVector(Transform target)
    {
        var diff = transform.position - target.transform.position;
        var diffLen = diff.magnitude;
        var scaler = Mathf.Clamp01(1.0f - diffLen / controller.neighborDist);
        return diff * (scaler / diffLen);
    }

    void Start()
    {
        noiseOffset = Random.value * 10.0f;
    }

    void Update()
    {
        if (isChild) {
            var currentPosition = transform.position;
            var currentRotation = transform.rotation;

            if (!isMin && currentPosition.x > cameraTest.xMax || isMin && currentPosition.x < cameraTest.xMin) {
                Destroy(gameObject);
            } else {
                // Current velocity randomized with noise.
                var noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
                var velocity = controller.velocity * (1.0f + noise * controller.velocityVariation);

                // Initializes the vectors.
                var separation = Vector3.zero;
                var alignment = controller.transform.forward;
                var cohesion = controller.transform.position;

                // Looks up nearby boids.
                var nearbyBoids = Physics.OverlapSphere(currentPosition, controller.neighborDist, controller.searchLayer);

                // Accumulates the vectors.
                foreach (var boid in nearbyBoids) {
                    if (boid.gameObject == gameObject) continue;
                    var t = boid.transform;
                    separation += GetSeparationVector(t);
                    alignment += t.forward;
                    cohesion += t.position;
                }

                var avg = 1.0f / nearbyBoids.Length;
                alignment *= avg;
                cohesion *= avg;
                cohesion = (cohesion - currentPosition).normalized;

                // Calculates a rotation from the vectors.
                var direction = separation + alignment + cohesion;
                var rotation = Quaternion.FromToRotation(Vector3.up, direction.normalized); // Where to look - needs updated when sprite changed

                // Applys the rotation with interpolation.
                if (rotation != currentRotation) {
                    var ip = Mathf.Exp(-controller.rotationCoeff * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(rotation, currentRotation, ip);
                }

                var test = transform.right;
                if (isMin) {
                    test = transform.right * -1;
                }

                // Moves forawrd.
                Vector3 newPosition = currentPosition + test * (velocity * Time.deltaTime); // Change transform.right
                transform.position = newPosition; // Change transform.right
            }
        }
    }
}