using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeWindowManager : MonoBehaviour {

    public Canvas canvas;
    public bool CanvasToggle;

	// Use this for initialization
	void Start ()
    {
        this.canvas = GetComponent<Canvas>();	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            ToggleMenu();
        }
	}

    public void ToggleMenu()
    {
        if (CanvasToggle)
        {
            this.canvas.enabled = false;
        }
        else
        {
            this.canvas.enabled = true;
        }
        CanvasToggle = !CanvasToggle;
    }
}
