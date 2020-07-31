using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [SerializeField]
    private Color INVIS = new Color(255, 255, 255, 0);
    [SerializeField]
    private Color WHITE = new Color(255, 255, 255, 255);

    [SerializeField]
    public Text pickupDialogue;
    [SerializeField]
    private KeyType type;
    [SerializeField]
    private bool playerInRange = false;
    [SerializeField]
    private BasicCharacter player;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        pickupDialogue = GameObject.Find("PickupDialogue").GetComponent<Text>();
    }

    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.Space)){
            player.AddKey(type);
            pickupDialogue.color = INVIS;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            player = other.gameObject.GetComponent<BasicCharacter>();
            pickupDialogue.color = WHITE;

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
            pickupDialogue.color = INVIS;
        }
    }
}
