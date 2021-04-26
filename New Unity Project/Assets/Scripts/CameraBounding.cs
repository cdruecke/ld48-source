using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounding : MonoBehaviour
{
    public GameObject cam;
    public int blockDirection = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBecameInvisible() {
        cam.SendMessage("UnblockDirection", blockDirection);
    }

    private void OnBecameVisible() {
        cam.SendMessage("BlockDirection", blockDirection);
    }
}
