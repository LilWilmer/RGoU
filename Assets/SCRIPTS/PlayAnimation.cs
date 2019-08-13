
/*****************************************************************************
 * AUTH: William Payne
 * FILE: PlayAnimation.cs
 * DATE: 22/01/19
 * PURP: Responisble for playing loaded animations in the Animator object,
 *       Lets listeners know when the animation is finished.
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour {

    //EVENTS------------------------------------------------------------------
    public delegate void delAnimFinished(string AnimationName);
    public event delAnimFinished AnimationFinished;

    //FIELDS------------------------------------------------------------------
    public string AnimationName;
    public Animator anim;

    //METHODS-----------------------------------------------------------------
    //SETUP FUNCTION:
    private void Start()
    {
        //anim = GetComponentInChildren<Animator>();
    }

    //PLAY THE LOADED ANIMATION:
    public void PlayAnim()
    {
        anim.Play(AnimationName);
    }

    public void PlayAnim(string AnimationName)
    {
        anim.Play(AnimationName);
    }

    //CALLS EVENT AFTER ANIMATION:
    public void AnimFinished(string AnimationName)
    {
        //If their are listeners let them know the animation finished
        if (AnimationFinished != null)
        {
            Debug.Log("PlayAnimation : AnimFinished : 48 -> "+AnimationName);
            AnimationFinished(AnimationName);
        }
    }
}
