using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraBehaviour : MonoBehaviour
{

    // Components
    [HideInInspector] public PlayerBehaviour player;

    [Header("UI Components")]
    public Image cursor;
    public Image health;
    public Image[] stances;

    // Settings
    [Header("Camera Settings")]
    public bool debug;
    public bool clamp_camera = false;
    public Vector2 clamp_bounds = new Vector2(0f, 0f);
    [Header("Lerps")]
    public float lerp_spd;
    public Vector2 lerp_bounds;
    [Header("Offsets")]
    public float y_offset;
    public float moving_x_offset;

    // Variables
    [Header("Variables")]
    public bool lerp_to_player = true;

    private Vector2 position;
    private float camera_x_offset;
    private bool lerping;

    // Start Event
    void Start()
    {
        // [Hide Cursor & View Details]
        Cursor.visible = false;

        // Components
        player = PlayerBehaviour.instance;

        // Variables
        position = new Vector2(transform.position.x, transform.position.y);
        lerping = false;

        // Move Camera at Start
        for (int i = 0; i < 30; i++) {
            FixedUpdate();
        }
    }

    // Update Event
    void Update()
    {
        // Player Stances
        float health_alpha = health.color.a;
        float gun_stance_alpha = Mathf.Pow(player.gun_transition, 2f);

        stances[0].rectTransform.localPosition = new Vector3(0f, (player.stance_transition * -50f) + 50f, 0f);
        Color stance_0_color = health.color;
        stance_0_color.a = Mathf.Pow(Mathf.Clamp(player.stance_transition - gun_stance_alpha, 0f, 1f), 3f);
        stances[0].color = stance_0_color * health_alpha;
        stances[1].rectTransform.localPosition = new Vector3(0f, (player.stance_transition * -50f), 0f);
        Color stance_1_color = health.color;
        stance_1_color.a = Mathf.Pow(Mathf.Clamp(1f - Mathf.Abs(player.stance_transition) - gun_stance_alpha, 0f, 1f), 3f);
        stances[1].color = stance_1_color * health_alpha;
        stances[2].rectTransform.localPosition = new Vector3(0f, (player.stance_transition * -50f) - 50f, 0f);
        Color stance_2_color = health.color;
        stance_2_color.a = Mathf.Pow(Mathf.Clamp(-player.stance_transition - gun_stance_alpha, 0f, 1f), 3f);
        stances[2].color = stance_2_color * health_alpha;

        Color stance_3_color = health.color;
        stance_3_color.a = gun_stance_alpha;
        stances[3].color = stance_3_color * health_alpha;

        // Mouse Cursor
        Vector3 mouse_input_pos = Input.mousePosition;
        mouse_input_pos.x = Mathf.Round(mouse_input_pos.x);
        mouse_input_pos.y = Mathf.Round(mouse_input_pos.y);
        mouse_input_pos.z = 5f;
        cursor.rectTransform.position = mouse_input_pos;

        // ***Debug*** //
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Physics Update Event
    void FixedUpdate()
    {
        // Camera Movement
        if (lerp_to_player) {
            // Lerp to Player
            if (!lerping) {
                // Check if Player is outside the bounds of the Camera Lerping Box
                if (Mathf.Abs(position.x - player.transform.position.x) > (lerp_bounds.x / 2f) || Mathf.Abs(position.y - (player.transform.position.y + y_offset)) > (lerp_bounds.y / 2f)) {
                    lerping = true;
                }
            }
            else {
                // Lerp the position of the Camera
                lerpToPosition(new Vector2(player.transform.position.x, player.transform.position.y + y_offset));
                // Make sure the Camera is done moving
                if (Vector2.Distance(new Vector2(player.transform.position.x + camera_x_offset, player.transform.position.y + y_offset), position) < 0.1f) {
                    if (Mathf.Abs(camera_x_offset) < 0.05f){
                        lerping = false;
                    }
                }
            }

            // Move the Camera in front of the player if they're moving
            if (player.player_velocity.x != 0) {
                camera_x_offset = Mathf.Lerp(camera_x_offset, moving_x_offset * Mathf.Sign(player.player_velocity.x), Time.fixedDeltaTime * lerp_spd);
            }
            else {
                camera_x_offset = Mathf.Lerp(camera_x_offset, 0, Time.fixedDeltaTime * lerp_spd);
            }
            lerpToPosition(new Vector2(position.x + camera_x_offset, position.y));

            // Move Camera using Camera Variables
            Vector2 rounding_position = position;
            rounding_position.y = Mathf.RoundToInt(rounding_position.y * 48f) / 48f;
            transform.position = new Vector3(rounding_position.x, rounding_position.y, transform.position.z);
            if (clamp_camera) {
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, clamp_bounds.x + 5f, clamp_bounds.y - 5f), transform.position.y, transform.position.z);
            }
        }
    }

    // Misc Methods
    public void lerpToPosition(Vector2 target_pos) {
        // Calculate the new location of the Camera to lerp to
        Vector2 new_pos = Vector2.Lerp(position, target_pos, lerp_spd * Time.fixedDeltaTime);
        position = new Vector2(new_pos.x, new_pos.y);
    }

    // Debug
    void OnDrawGizmos()
    {
        if (debug) {
            // Draw the Bounds of the Lerping Box
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, 0f), new Vector3(lerp_bounds.x, lerp_bounds.y, 1f));
        }
    }
}
