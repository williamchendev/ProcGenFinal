using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    // Enums
    public enum StanceState {
        up,
        center,
        down,
        gun
    }

    // Singleton
    public static PlayerBehaviour instance { get; private set; }
    [Header("Singleton")]
    public float slow_time;
    public float freeze_time;

    // Components
    private BoxCollider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Settings
    [Header("Debug")]
    public bool debug;

    [Header("Physics Settings")]
    public LayerMask solids_layer;
    public float spd = 0.1f;
    public float jump_spd = 1f;
    public float friction = 0.5f;
    public float terminal_velocity = 5f;
    public float gravity_add = 0.01f;
    public float gravity_mult = 1.05f;
    public float physics_interpolation = 0.05f;

    public float slow_multiplier = 0.1f;

    [Header("Combat Settings")]
    public float stance_mouse_angle = 45f;
    public float stance_mouse_yoffset = 0.5f;
    public float stance_transition_spd = 5f;

    // Variables
    [HideInInspector] public bool player_control;
    [HideInInspector] public bool jumped;
    [HideInInspector] public bool grounded;
    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public Vector2 player_velocity;

    [HideInInspector] public Vector2 sprite_stretch;

    public StanceState stance = StanceState.center;
    [HideInInspector] public float stance_transition;
    [HideInInspector] public float gun_transition;

    // OnAwake Initialization Event
    void Awake()
    {
        // Singleton
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            Destroy(gameObject);
            return;
        }
        else {
            gameObject.tag = "Player";
            instance = this;
            slow_time = 0f;
            freeze_time = 0f;
        }
    }

    // Start Initialization Event
    void Start()
    {
        // Components
        col = gameObject.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        // Variables
        player_control = true;
        grounded = false;
        jumped = false;
        velocity = Vector2.zero;
        player_velocity = Vector2.zero;

        sprite_stretch = Vector2.one;

        stance_transition = 0f;
    }

    // Update Event
    void Update()
    {
        // Movement
        player_velocity.x = 0f;
        if (player_control) {
            // Gun Button Press & Gun Stance
            if (Input.GetMouseButton(1)) {
                gun_transition = Mathf.Lerp(gun_transition, 1f, stance_transition_spd * Time.deltaTime);
                if (gun_transition > 0.7f) {
                    stance = StanceState.gun;
                }
            }
            else {
                gun_transition = Mathf.Lerp(gun_transition, 0f, stance_transition_spd * Time.deltaTime);
            }

            // Player Key Movement
            if (gun_transition > 0.8f) {
                // Gun Control
                
            }
            else if (gun_transition < 0.3f) {
                // Moving Left and Right
                if (Input.GetKey(KeyCode.A)) {
                    player_velocity.x = -spd;
                }
                else if (Input.GetKey(KeyCode.D)) {
                    player_velocity.x = spd;
                }

                // Jumping (Jumping won't be in the game it's just here to test the physics)
                if (grounded) {
                    if (Input.GetKeyDown(KeyCode.W)) {
                        jumped = true;
                        grounded = false;
                        velocity.y = jump_spd;

                        sprite_stretch = new Vector2(0.7f, 1.3f);
                    }
                }
            }

            // Sword Stances
            Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float mouse_angle = Mathf.Atan2(mouse_position.y - (transform.position.y + stance_mouse_yoffset), mouse_position.x - transform.position.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(90 - mouse_angle) < stance_mouse_angle) {
                stance_transition = Mathf.Lerp(stance_transition, 1f, stance_transition_spd * Time.deltaTime);
                if (Mathf.Abs(stance_transition - 1f) < 0.5f && gun_transition < 0.3f) {
                    stance = StanceState.up;
                }
            }
            else if (Mathf.Abs(-90 - mouse_angle) < stance_mouse_angle) {
                stance_transition = Mathf.Lerp(stance_transition, -1f, stance_transition_spd * Time.deltaTime);
                if (Mathf.Abs(stance_transition + 1f) < 0.5f && gun_transition < 0.3f) {
                    stance = StanceState.down;
                }
            }
            else {
                stance_transition = Mathf.Lerp(stance_transition, 0f, stance_transition_spd * Time.deltaTime);
                if (Mathf.Abs(stance_transition) < 0.5f && gun_transition < 0.3f) {
                    stance = StanceState.center;
                }
            }
        }

        // Squash and Stretch
        transform.localScale = new Vector3(sprite_stretch.x, sprite_stretch.y, transform.localScale.z);
        sprite_stretch = Vector2.Lerp(sprite_stretch, Vector2.one, Time.deltaTime * 5f);
    }

    // Physics Event
    void FixedUpdate()
    {
        // Rigidbody Physics
        if (freeze_time <= 0) {
            // Grounded
            RaycastHit2D grounded_hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.05f, solids_layer);
            if (grounded_hit.collider != null) {
                if (jumped) {
                    jumped = false;
                }
                else {
                    if (!grounded) {
                        sprite_stretch = new Vector2(1.6f, 0.4f);
                    }
                    grounded = true;
                }
            }
            else {
                // Gravity
                velocity.y -= gravity_add;
                if (velocity.y < 0) {
                    velocity.y *= gravity_mult;
                }
                velocity.y = Mathf.Clamp(velocity.y, -terminal_velocity, velocity.y);
                grounded = false;
            }

            // Movement
            Vector2 physics_velocity = player_velocity + velocity;
            if (physics_velocity != Vector2.zero) {
                // Friction
                if (grounded) {
                    velocity.x = Mathf.MoveTowards(velocity.x, 0, friction * Time.fixedDeltaTime);
                    velocity.y = 0f;
                    player_velocity.y = 0f;
                }

                // Direction
                if (player_control) {
                    if (player_velocity.x != 0) {
                        if (player_velocity.x < 0) {
                            sr.flipX = true;
                        }
                        else {
                            sr.flipX = false;
                        }
                    }
                }

                // Rigidbody Movement
                Vector2 player_position = new Vector2(transform.position.x, transform.position.y);
                RaycastHit2D check_free = Physics2D.BoxCast(player_position + physics_velocity + col.offset, col.size, 0f, Vector2.up, Mathf.Infinity, solids_layer);
                if (check_free.collider == null) {
                    rb.MovePosition(player_position + physics_velocity);
                }
                else {
                    // Precise Collisions & Making sure the player doesn't drop through walls if they're moving too fast
                    float x_move = 0f;
                    float y_move = 0f;

                    // Move Rigidbody Vertically
                    float move_back = 1f;
                    while (move_back >= 0f) {
                        Vector2 check_position = new Vector2(0f, move_back * physics_velocity.y);
                        RaycastHit2D check_free_v = Physics2D.BoxCast(player_position + check_position + col.offset, col.size, 0f, Vector2.up, Mathf.Infinity, solids_layer);
                        if (check_free_v.collider == null) {
                            y_move = check_position.y;
                            break;
                        }
                        move_back -= physics_interpolation;
                    }

                    // Move Rigidbody Horizontally
                    move_back = 1f;
                    while (move_back >= 0f) {
                        Vector2 check_position = new Vector2(move_back * physics_velocity.x, 0f);
                        RaycastHit2D check_free_v = Physics2D.BoxCast(player_position + check_position + col.offset, col.size, 0f, Vector2.up, Mathf.Infinity, solids_layer);
                        if (check_free_v.collider == null) {
                            x_move = check_position.x;
                            break;
                        }
                        move_back -= physics_interpolation;
                    }

                    // Move the Player to Precise Location where it collides with a wall
                    rb.MovePosition(player_position + new Vector2(x_move, y_move));
                }
            }
        }
    }

    // Debug
    void OnDrawGizmos()
    {
        if (debug) {
            // Stance Angle
            Gizmos.color = Color.red;
            float stance_angle_to_rad = stance_mouse_angle * Mathf.Deg2Rad;
            Vector3 stance_mouse_v3 = transform.position + new Vector3(0f, stance_mouse_yoffset, 0f);
            Gizmos.DrawLine(stance_mouse_v3, stance_mouse_v3 + (new Vector3(Mathf.Cos(stance_angle_to_rad + (Mathf.PI / 2)), Mathf.Sin(stance_angle_to_rad + (Mathf.PI / 2)), 0f) * 3f));
            Gizmos.DrawLine(stance_mouse_v3, stance_mouse_v3 + (new Vector3(Mathf.Cos(-stance_angle_to_rad + (Mathf.PI / 2)), Mathf.Sin(-stance_angle_to_rad + (Mathf.PI / 2)), 0f) * 3f));
            Gizmos.DrawLine(stance_mouse_v3, stance_mouse_v3 + (new Vector3(Mathf.Cos(stance_angle_to_rad - (Mathf.PI / 2)), Mathf.Sin(stance_angle_to_rad - (Mathf.PI / 2)), 0f) * 3f));
            Gizmos.DrawLine(stance_mouse_v3, stance_mouse_v3 + (new Vector3(Mathf.Cos(-stance_angle_to_rad - (Mathf.PI / 2)), Mathf.Sin(-stance_angle_to_rad - (Mathf.PI / 2)), 0f) * 3f));
        }
    }

}
