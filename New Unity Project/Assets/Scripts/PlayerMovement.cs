using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public int gravityForce = 1;
    public int speed = 1;
    public float verticalInertia = 0;
    public int maxFallSpeed = -3;
    public int jumpForce = 4;
    public bool onFloor;
    public bool facingRight = true;
    public float flutterJump = 1.15F;
    public Animator animator;
    public AudioSource jumpfx;
    public AudioSource walkfx;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float localGravity = gravityForce;
        float hz = Input.GetAxisRaw("Horizontal");
        float vt = Input.GetAxisRaw("Vertical");
        if (Input.GetButton("Jump")) {
            vt = 1;
        }

        if (facingRight && hz < 0) {
            facingRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
        } else if (!facingRight && hz > 0) {
            facingRight = true;
            GetComponent<SpriteRenderer>().flipX = false;
        }
        
        if (vt > 0 && verticalInertia > 0) {
            localGravity /= 4;
        } else if (vt > 0 && verticalInertia <= 0) {
            localGravity /= flutterJump;
        }

        if (vt > 0 && verticalInertia < 0) { // fluttering makes you slower
            hz *= (float).5;
        }

        if (!onFloor) {
            verticalInertia -= localGravity * Time.deltaTime;
        }

        if (verticalInertia < maxFallSpeed) {
            verticalInertia = maxFallSpeed;
        }

        if (verticalInertia == maxFallSpeed && vt < 0) { // fast fall
            verticalInertia *= 1.5F;
        }

        if (vt > 0 && onFloor) {
            verticalInertia = jumpForce;
            onFloor = false;
            animator.SetBool("onFloor", onFloor);
            jumpfx.Play();
        }

        int layerMask = 3 << 8;
        layerMask = ~layerMask;

        Vector3 feet = transform.position + new Vector3(0, -.49F, 0);
        Vector3 head = transform.position + new Vector3(0, .49F, 0);

        RaycastHit2D topRightWallCheck = Physics2D.Raycast(head + new Vector3(.2F, 0, 0), Vector2.right, hz * speed * Time.deltaTime, layerMask);
        RaycastHit2D bottomRightWallCheck = Physics2D.Raycast(feet + new Vector3(.2F, 0, 0), Vector2.right, hz * speed * Time.deltaTime, layerMask);
        RaycastHit2D topLeftWallCheck = Physics2D.Raycast(head + new Vector3(-.2F, 0, 0), Vector2.left, hz * speed * Time.deltaTime, layerMask);
        RaycastHit2D bottomLeftWallCheck = Physics2D.Raycast(feet + new Vector3(-.2F, 0, 0), Vector2.left, hz * speed * Time.deltaTime, layerMask);

        if (topRightWallCheck.collider != null || bottomRightWallCheck.collider != null) {
            if (hz > 0) {
                hz = 0;
            }
        }
        else if (topLeftWallCheck.collider != null || bottomLeftWallCheck.collider != null) {
            if (hz < 0) {
                hz = 0;
            }
        }
        RaycastHit2D ceilingCheck = Physics2D.Raycast(head, Vector2.up, Mathf.Abs(verticalInertia * Time.deltaTime), layerMask);
        RaycastHit2D floorCheck = Physics2D.Raycast(feet, -Vector2.up, Mathf.Max(Mathf.Abs(verticalInertia * Time.deltaTime), .01F), layerMask);
        if (verticalInertia < 0 && floorCheck.collider != null) {
            verticalInertia = 0;
            onFloor = true;
            animator.SetBool("onFloor", onFloor);
            // Snap to floor
            transform.position = new Vector3(transform.position.x, floorCheck.point.y + .5F, transform.position.z);
        }
        else if (onFloor && floorCheck.collider == null) {
            onFloor = false;
            animator.SetBool("onFloor", onFloor);
        }
        else if (verticalInertia > 0 && ceilingCheck.collider != null) {
            verticalInertia = Mathf.Max(-verticalInertia, maxFallSpeed);
        }

        if (hz != 0) {
            animator.SetBool("horizontalInertia", true);
        } else {
            animator.SetBool("horizontalInertia", false);
        }

        if (hz != 0 && onFloor) {
            if (!walkfx.isPlaying) {
                walkfx.Play();
            }
        }
        else if (walkfx.isPlaying) {
            walkfx.Stop();
        }

        animator.SetFloat("verticalInertia", verticalInertia);
        transform.position = transform.position + new Vector3(hz * speed, verticalInertia, 0) * Time.deltaTime;
    }
}
