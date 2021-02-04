using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public GameObject sp1, sp2; // Player in level 1 and player starting location in level 2
    private bool canTransport;

    // Start is called before the first frame update
    void Start() {
        canTransport = true;
        sp1 = this.gameObject;
    }

    void OnTriggerEnter2D(Collider2D trig) {
        // TODO: sp2 is random area selected in level 2 - invisible box
        if (trig.tag == "Player" && canTransport) {
            trig.gameObject.transform.position = sp2.gameObject.transform.position;
            canTransport = false;
        }
    }

    void OnTriggerExit2D(Collider2D trig) {
        canTransport = true;
    }
}
