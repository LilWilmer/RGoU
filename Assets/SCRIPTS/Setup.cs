using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour {

    public DiceRoller roller;
    public DiceTotalDisplay score;

    public void SetupScore()
    {
        score = new DiceTotalDisplay(roller);
    }

	// Use this for initialization
	void Start () {
        SetupScore();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
