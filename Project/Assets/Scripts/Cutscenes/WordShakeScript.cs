using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordShakeScript : MonoBehaviour
{
    // Components
    private TextMesh text;

    // Settings
    public float shake_amount = 0.1f;
    public Vector2 shake_delay = Vector2.one;

    public bool flicker = false;
    public Vector2 flicker_delay = Vector2.one;
    public string flicker_text = "";

    // Variables
    private Vector2 position;

    private float shake_timer;
    private float flicker_timer;

    private bool text_is_flicker;
    private string normal_text;

    // Start Event
    void Start()
    {
        text = GetComponent<TextMesh>();

        shake_timer = 0f;
        flicker_timer = flicker_delay.y;
        flicker_text = flicker_text.Replace("NEWLINE","\n");

        text_is_flicker = false;
        normal_text = text.text;

        position = new Vector2(transform.position.x, transform.position.y);
    }

    // Update Event
    void Update()
    {
        // Shake
        shake_timer -= Time.deltaTime;
        if (shake_timer <= 0) {
            shake_timer = Random.Range(shake_delay.x, shake_delay.y);
            Vector2 shake_offset = new Vector2(Random.Range(-shake_amount, shake_amount), Random.Range(-shake_amount, shake_amount)) + position;
            transform.position = new Vector3(shake_offset.x, shake_offset.y, transform.position.z);
        }
        
        // Flicker
        if (flicker) {
            flicker_timer -= Time.deltaTime;
            if (flicker_timer <= 0) {
                flicker_timer = Random.Range(flicker_delay.x, flicker_delay.y);
                if (!text_is_flicker) {
                    text_is_flicker = true;
                    text.text = flicker_text;
                }
                else {
                    text_is_flicker = false;
                    text.text = normal_text;
                }
            }
        }
    }
}
