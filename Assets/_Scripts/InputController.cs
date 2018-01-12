using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    Inputs sendInput;

    public struct Inputs
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;

    }

    public struct mouseInputs
    {
        public bool rmb;
        public bool lmb;

        public float x;
        public float y;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("down"))
        {
            sendInput.down = true;
        }
        if (Input.GetButtonUp("down"))
        {
            sendInput.down = false;
        }
        if (Input.GetButtonDown("up"))
        {
            sendInput.up = true;
        }
        if (Input.GetButtonUp("up"))
        {
            sendInput.up = false;
        }
        if (Input.GetButtonDown("left"))
        {
            sendInput.left = true;
        }
        if (Input.GetButtonUp("left"))
        {
            sendInput.left = false;
        }
        if (Input.GetButtonDown("right"))
        {
            sendInput.right = true;
        }
        if (Input.GetButtonUp("right"))
        {
            sendInput.right = false;
        }
    }

}
