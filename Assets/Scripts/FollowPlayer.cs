using UnityEngine;
using System.Collections.Generic;

public class FollowPlayer : MonoBehaviour {

    public GameObject leader; // the game object to follow - assign in inspector
    public int steps; // number of steps to stay behind - assign in inspector

    private Queue<Vector3> record = new Queue<Vector3>();
    private Vector3 lastRecord;

    void FixedUpdate() {
        float _dogMinY = transform.position.y - transform.localScale.y / 2;
        float _dogMaxY = transform.position.y + transform.localScale.y / 2;
        float _minYPos = leader.transform.position.y - leader.transform.localScale.y / 2 + (_dogMaxY - _dogMinY);
        Vector2 positionLeader = new Vector2(leader.transform.position.x - 0.4f, _minYPos);
        record.Enqueue(positionLeader);

        // remove last position from the record and use it for our own
        if (record.Count > steps) {
            this.transform.position = record.Dequeue();
        }
    }
}
