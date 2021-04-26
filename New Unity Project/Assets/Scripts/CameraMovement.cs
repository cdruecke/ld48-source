using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public bool blockLeft = false;
    public bool blockRight = false;
    public bool blockUp = false;
    public bool blockDown = false;
    public bool onlyVertical = true;
    public int absX = 9;
    public bool inGame = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (inGame) {
            if (player.transform.position.x < transform.position.x) {

            }
            else if (player.transform.position.x > transform.position.x) {

            }
            var TargetX = player.transform.position.x;
            var TargetY = player.transform.position.y;
            if (Input.GetAxisRaw("Vertical") < 0) {
                TargetY -= 4;
            }
            Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            if (TargetX < transform.position.x && blockLeft) {
                TargetX = transform.position.x;
            }
            else if (TargetX > transform.position.x && blockRight) {
                TargetX = transform.position.x;
            }
            if (TargetY < transform.position.y && blockDown) {
                TargetY = transform.position.y;
            }
            else if (TargetY > transform.position.y && blockUp) {
                TargetY = transform.position.y;
            }
            if (onlyVertical) {
                TargetX = transform.position.x;
            }
            if (TargetX >= absX) {
                TargetX = absX;
            }
            if (TargetX <= -absX) {
                TargetX = -absX;
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(TargetX, TargetY, transform.position.z), Time.deltaTime); // Lerp
                                                                                                                                        // transform.position = new Vector3(TargetX, TargetY, transform.position.z); // Instant
        }
    }
    public void BeginGame() {
        inGame = true;
    }

    void BlockDirection(int dir) {
        if (dir == 0) {
            blockLeft = true;
        } 
        if (dir == 1) {
            blockRight = true;
        }
        if (dir == 2) {
            blockUp = true;
        }
        if (dir == 3) {
            blockDown = true;
        }
    }
    void UnblockDirection(int dir) {
        if (dir == 0) {
            blockLeft = false;
        }
        if (dir == 1) {
            blockRight = false;
        }
        if (dir == 2) {
            blockUp = false;
        }
        if (dir == 3) {
            blockDown = false;
        }
    }

}
