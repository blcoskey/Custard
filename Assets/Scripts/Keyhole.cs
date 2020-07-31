using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Keyhole : MonoBehaviour {
    [SerializeField]
    private Color INVIS = new Color(255, 255, 255, 0);
    [SerializeField]
    private Color WHITE = new Color(255, 255, 255, 255);

    public KeyType keyType;
    [SerializeField]
    private bool playerInRange = false;
    [SerializeField]
    private BasicCharacter player;
    [SerializeField]
    private Text useKeyDialogue;
    [SerializeField]
    private LevelManager levelManager;

    void Update () {
        if (playerInRange && player.keyType == keyType) {
            if (Input.GetKeyDown (KeyCode.Space)) {
                player.UseKey ();
                levelManager.UseKey(keyType);
                keyType = KeyType.None;
                useKeyDialogue.text = "You've inserted a key into the slot";
            }
        }
    }

    void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            player = other.gameObject.GetComponent<BasicCharacter> ();
            playerInRange = true;

            if (player.keyType == keyType) {
                useKeyDialogue.text = "Press SPACE to use key";
            }
            if (keyType == KeyType.None) {
                useKeyDialogue.text = "There is a key inserted into the slot";
            }
            if(player.keyType != keyType && player.keyType != KeyType.None)
            {
                useKeyDialogue.text = "This key doesn't fit";
            }
            if(player.keyType == KeyType.None && keyType != KeyType.None)
            {
                useKeyDialogue.text = "There seems to be a slot for a key";
            }

            useKeyDialogue.color = WHITE;
        }
    }

    private void OnTriggerExit (Collider other) {
        if (other.tag == "Player") {
            playerInRange = false;
            useKeyDialogue.color = INVIS;
        }
    }
}