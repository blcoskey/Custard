using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public Camera blue;
    public Camera red;
    public bool redCamOn;
    public void SwitchCam(){
        if(redCamOn){
            blue.gameObject.SetActive(true);
            red.gameObject.SetActive(false);
        }
        else{
            blue.gameObject.SetActive(false);
            red.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Test");
        SwitchCam();
    }
}
