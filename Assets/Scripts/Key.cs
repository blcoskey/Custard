using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [SerializeField]
    private Text pickupDialogue;
    [SerializeField]
    private KeyType type;
    [SerializeField]
    private bool playerInRange = false;
    [SerializeField]
    private BasicCharacter player;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.Space)){
            player.AddKey(type);
            pickupDialogue.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            player = other.gameObject.GetComponent<BasicCharacter>();
            pickupDialogue.gameObject.SetActive(true);

            if(player.keyType == KeyType.None){
                pickupDialogue.text = "Press SPACE to pickup key";
                playerInRange = true;
            }else{
                pickupDialogue.text = "You can only carry one key at a time";
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            playerInRange = false;
            pickupDialogue.gameObject.SetActive(false);
        }
    }
}
