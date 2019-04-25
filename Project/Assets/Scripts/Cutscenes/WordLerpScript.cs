using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordLerpScript : MonoBehaviour
{

    // Components
    private TextMesh text;

    // Settings
    [Header("Word Settings")]
    public bool lerp = false;
    public float lerp_exponent = 1f;
    public float lerp_multiply = 1f;
    public float word_speed = 5f;
    public float word_snap = 1f;
    public Vector2 word_target_position = Vector2.zero;

    [Header("Color Settings")]
    public bool color_change = false;
    public Color snap_color = Color.white;

    // Variables
    private float increase;
    private bool snapped;

    // Start Event
    void Start()
    {
        increase = 0f;
        text = GetComponent<TextMesh>();
        snapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!snapped) {
            Vector2 current_pos = new Vector2(transform.position.x, transform.position.y);
            Vector2 new_pos;
            if (lerp) {
                increase += Time.deltaTime * word_speed;
                increase = Mathf.Pow(increase, lerp_exponent);
                increase = increase * lerp_multiply;
                new_pos = Vector2.MoveTowards(current_pos, word_target_position, increase);
            }
            else {
                new_pos = Vector2.MoveTowards(current_pos, word_target_position, Time.deltaTime * word_speed);
            }
            transform.position = new Vector3(new_pos.x, new_pos.y, transform.position.z);

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), word_target_position) <= word_snap) {
                transform.position = new Vector3(word_target_position.x, word_target_position.y, transform.position.z);
                if (color_change) {
                    text.color = snap_color;
                }
                snapped = true;
            }
        }
    }
}
