using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameState : MonoBehaviour {
    public bool pickedUpMilk = false;
    public bool pickedUpAcorn = false;
    public bool pickedUpKazoo = false;
    public bool startingSpookyTime = false;
    public bool spookyTime = false;
    public bool winGame = false;
    public GameObject endGameText;
    public GameObject menuTitle;
    public GameObject cam;
    public GameObject playButton;
    public SquirrelSpeech bouncerSquirrel;
    public AudioSource happyMusic;
    public AudioSource scaryMusic;
    public AudioSource menuMusic;
    public GameObject escapeDialogue;
    public GameObject bloons;
    public UnityEngine.UI.Image fadeOut;
    public float fadeOutTimer;
    public float maxFadeTimer;
    public bool isFadingIn;
    public bool isFadingOut;
    // Start is called before the first frame update
    void Start() {
        menuMusic.Play();
    }

    // Update is called once per frame
    void Update() {
        if (isFadingIn) {
            Color newColor = new Color(fadeOut.color.r, fadeOut.color.g, fadeOut.color.b, 1 - (fadeOutTimer / maxFadeTimer));
            fadeOut.color = newColor;
        } else if (isFadingOut) {
            Color newColor = new Color(fadeOut.color.r, fadeOut.color.g, fadeOut.color.b, fadeOutTimer / maxFadeTimer);
            fadeOut.color = newColor;
        }

        if (fadeOutTimer <= 0) {
            if (isFadingIn) {
                isFadingIn = false;
                isFadingOut = false;
                if (!startingSpookyTime) {
                    menuTitle.SetActive(false);
                    menuMusic.Stop();
                    happyMusic.Play();
                    cam.SendMessage("BeginGame");
                    cam.GetComponent<Camera>().cullingMask = ~0;
                    cam.GetComponent<Camera>().backgroundColor = new Color(0.4666667F, 0.3176471F, 0.2431373F);
                }
                else if (winGame) {
                    cam.GetComponent<Camera>().cullingMask = 0;
                    cam.GetComponent<Camera>().backgroundColor = Color.black;
                    scaryMusic.Stop();
                    menuMusic.Play();
                    endGameText.SetActive(true);
                }
                else {
                    spookyTime = true;
                    bloons.SetActive(false);
                    happyMusic.Stop();
                    scaryMusic.Play();
                    bouncerSquirrel.SendMessage("Talk", "Are you enjoying the party?");
                    Color spookyColor = new Color(.283F, .223F, .283F);
                    cam.GetComponent<Camera>().backgroundColor = spookyColor;
                    escapeDialogue.SetActive(true);
                }
                BeginFadeOut();
            } else {
                isFadingIn = false;
                isFadingOut = false;
            }
        } else {
            fadeOutTimer -= Time.deltaTime;
        }

    }

    void PickUpItem(string item) {
        if (item == "milk") {
            pickedUpMilk = true;
        }

        if (item == "acorn") {
            pickedUpAcorn = true;
        }

        if (item == "kazoo") {
            pickedUpKazoo = true;
        }
    }

    public void BeginFadeOut() {
        fadeOutTimer = 1F;
        maxFadeTimer = 1F;
        isFadingIn = false;
        isFadingOut = true;
    }

    public void BeginFadeIn() {
        fadeOutTimer = .5F;
        maxFadeTimer = .5F;
        isFadingIn = true;
        isFadingOut = false;
    }

    public void StartGame() {
        pickedUpMilk = false;
        pickedUpAcorn = false;
        pickedUpKazoo = false;
        spookyTime = false;
        winGame = false;
        playButton.SetActive(false);
        BeginFadeIn();
    }

    public void EndGame() {
        BeginFadeIn();
        winGame = true;
    }

    void StartSpookyTime() {
        startingSpookyTime = true;
        BeginFadeIn();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("PartyDoor")) {
            if (!pickedUpMilk) {
                bouncerSquirrel.SendMessage("Talk", "The party can't start without milk, friend.");
            } else if (!pickedUpAcorn) {
                bouncerSquirrel.SendMessage("Talk", "There's not enough snacks - bring an acorn?");
            } else if (!pickedUpKazoo) {
                bouncerSquirrel.SendMessage("Talk", "Can't have a real party without a kazoo!");
            }
            else if (!spookyTime) {
                // party cutscene
                StartSpookyTime();
            }
        }
        else if (collision.gameObject.CompareTag("HomeDoor")) {
            if (spookyTime) {
                EndGame();
            }
        }
    }
}
