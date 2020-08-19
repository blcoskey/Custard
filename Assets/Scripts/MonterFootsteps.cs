using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonterFootsteps : MonoBehaviour
{
    public AudioClip[] stepClips;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Step(){
        audioSource.PlayOneShot(GetRandomClip());
    }

    private AudioClip GetRandomClip(){
        return stepClips[UnityEngine.Random.Range(0, stepClips.Length)];
    }
}
