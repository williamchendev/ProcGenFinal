using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneBlockBehaviour : MonoBehaviour
{

    public float block_duration = 1f;
    public bool debug = false;
    [HideInInspector] public bool finished;

    // Initialization Event
    void Awake()
    {
        finished = false;
        if (!debug) {
            gameObject.SetActive(false);
        }
    }

    // Update Event
    void Update()
    {
        if (!finished) {
            block_duration -= Time.deltaTime;
            if (block_duration <= 0) {
                finished = true;
                gameObject.SetActive(false);
            }
        }
    }
}
