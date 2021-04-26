using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public string itemType;
    public GameObject icon;
    public float maxSize = 1.3F;
    public int minSize = 1;
    public int maxDegrees = 15;
    public bool goingRight;
    public float sizeChangeSpeed = .3F;
    public bool goingUp = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x > maxSize) {
            goingUp = false;
        } else if (transform.localScale.y < minSize) {
            goingUp = true;
        }
        if (transform.localRotation.eulerAngles.z > maxDegrees && transform.localRotation.eulerAngles.z < 180) {
            goingRight = true;
        } else if (transform.localRotation.eulerAngles.z < 360 - maxDegrees && transform.localRotation.eulerAngles.z > 180) {
            goingRight = false;
        }
        if (goingRight) {
            transform.Rotate(new Vector3(0, 0, -sizeChangeSpeed) * 20 * Time.deltaTime);
        } else {
            transform.Rotate(new Vector3(0, 0, sizeChangeSpeed) * 20 * Time.deltaTime);
        }
        if (goingUp) {
            transform.localScale += new Vector3(sizeChangeSpeed, sizeChangeSpeed, 0) * Time.deltaTime;
        } else {
            transform.localScale -= new Vector3(sizeChangeSpeed, sizeChangeSpeed, 0) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerGameState>().SendMessage("PickUpItem", itemType);
            icon.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            Destroy(this.gameObject);
        }
    }


}
