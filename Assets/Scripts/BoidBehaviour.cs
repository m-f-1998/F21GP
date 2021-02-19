/**
 * @author Matthew Frankland
 * @email [developer@matthewfrankland.co.uk]
 * @create date 07-02-2021 20:36:53
 * @modify date 19-02-2021 09:26:22
 * @desc [Boid birds - controlls individual bird behaviour]
 */

using UnityEngine;

public class BoidBehaviour : MonoBehaviour {

    public BoidController controller;
    public CameraSystem cameraTest;
    public bool isChild = false, isMin = false;

    private float noiseOffset;

    void Start() {
        noiseOffset = Random.value * 10.0f;
    }

    void Update() {
        if (isChild) {
            var curPos = transform.position;
            var curRot = transform.rotation;

            if (!isMin && curPos.x > cameraTest.xMax || isMin && curPos.x < cameraTest.xMin) Destroy(gameObject);
            else {
                var noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
                var velocity = controller.velocity * (1.0f + noise * controller.velocityVariation);
                var separation = Vector3.zero;
                var alignment = controller.transform.forward;
                var cohesion = controller.transform.position;
                var nearbyBoids = Physics.OverlapSphere(curPos, controller.neighborDist, controller.searchLayer);

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
                cohesion = (cohesion - curPos).normalized;

                var direction = separation + alignment + cohesion;
                var rotation = Quaternion.FromToRotation(Vector3.up, direction.normalized);

                if (rotation != curRot) {
                    var ip = Mathf.Exp(-controller.rotationCoeff * Time.deltaTime);
                    transform.rotation = Quaternion.Slerp(rotation, curRot, ip);
                }

                transform.position = curPos + (isMin ? transform.right * -1 : transform.right) * (velocity * Time.deltaTime);
            }
        }
    }

    Vector3 GetSeparationVector(Transform target) {
        var diff = transform.position - target.transform.position;
        var diffLen = diff.magnitude;
        var scaler = Mathf.Clamp01(1.0f - diffLen / controller.neighborDist);
        return diff * (scaler / diffLen);
    }
}