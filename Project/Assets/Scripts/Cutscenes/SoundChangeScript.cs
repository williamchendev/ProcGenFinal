using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChangeScript : MonoBehaviour
{
    public GameObject delete;
    public AudioClip play_this;

    // Start
    void Start()
    {
        Destroy(delete.gameObject);
        GetComponent<AudioSource>().PlayOneShot(play_this);
    }

}
