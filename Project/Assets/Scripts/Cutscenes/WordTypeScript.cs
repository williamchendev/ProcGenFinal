using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTypeScript : MonoBehaviour
{

    // Components
    private TextMesh text;

    // Settings
    public string text_content = "";
    public float text_speed = 1f;

    // Variables
    private bool finished;
    private float timer;

    // Start Event
    void Start()
    {
        text = GetComponent<TextMesh>();
        text.text = "";
        finished = false;
        timer = 0f;
    }

    // Update Event
    void Update()
    {
        if (!finished) {
            timer += Time.deltaTime * text_speed;
            int characters = Mathf.Clamp(Mathf.RoundToInt(timer), 0, text_content.Length);
            text.text = text_content.Substring(0, characters);
            if (characters >= text_content.Length) {
                finished = true;
            }
        }
    }
}
