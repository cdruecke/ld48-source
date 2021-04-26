using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SquirrelSpeech : MonoBehaviour
{
    public GameObject dialogue;
    public PlayerGameState gameState;
    public bool isWearingHat = false;
    public bool isWearingMask = false;
    public bool isWearingMustache = false;
    public bool isWearingBow = false;
    public bool isWearingGlasses = false;
    public bool isWearingLips = false;
    public bool isSpooky = false;
    public Animator animator;
    public float readTime = 0;
    public float opacity = 0;
    public string spookyText = "Boo!";
    public AudioSource talking;
    public AudioSource spookyTalking;
    // Start is called before the first frame update
    void Start()
    {
        dialogue.SetActive(false);
        if (isSpooky) {
            animator.SetInteger("variant", 6);
        } else if (isWearingMask) {
            animator.SetInteger("variant", 1);
        } else if (isWearingHat) {
            animator.SetInteger("variant", 2);
        } else if (isWearingBow) {
            animator.SetInteger("variant", 3);
        } else if (isWearingMustache) {
            animator.SetInteger("variant", 4);
        } else if (isWearingLips) {
            animator.SetInteger("variant", 5);
        } else if (isWearingGlasses) {
            animator.SetInteger("variant", 7);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (readTime > 0) {
            readTime -= Time.deltaTime;
        }
        if (readTime <= 0 && opacity > 0) {
            opacity -= Time.deltaTime;
            if (opacity < 0) {
                opacity = 0;
            }
            Color currentColor = dialogue.GetComponent<TextMeshPro>().faceColor;
            currentColor = new Color(currentColor.r, currentColor.g, currentColor.b, opacity);
            dialogue.GetComponent<TextMeshPro>().faceColor = currentColor;
        }
        if (!isSpooky && gameState.spookyTime) {
            isSpooky = true;
            animator.SetInteger("variant", 6);
            Talk(spookyText);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if (isSpooky && opacity <= 0) {
                spookyTalking.Play();
            }
            else if (opacity <= 0) {
                talking.Play();
            }
            dialogue.SetActive(true);
            readTime = 3F;
            opacity = 1;
            Color currentColor = dialogue.GetComponent<TextMeshPro>().faceColor;
            currentColor.a = opacity;
            dialogue.GetComponent<TextMeshPro>().faceColor = currentColor;
        }
    }

    public void Talk(string text) {
        dialogue.GetComponent<TextMeshPro>().SetText(text);
        dialogue.SetActive(false);
    }
}
