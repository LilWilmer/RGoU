using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //FIELDS==================================================================
    public PlayAnimation anim;
    public int CurrentCamera;
    public Camera[] PlayerCameras;
    public Camera GameOverCamera;
    public Camera MenuCamera;
    public Ur UrHandler { get; set; }
    private Observer observer;

    //METHODS=================================================================
    public void Setup()
    {
        CurrentCamera = 0;
        this.observer = new Observer(this);
    }

    public void NextCamera()
    {
        if(CurrentCamera == 0)
        {
            anim.PlayAnim("Cool180_L");
        }
        else
        {
            anim.PlayAnim("Cool180");
        }
        //this.PlayerCameras[CurrentCamera].enabled = false;

        this.CurrentCamera += 1;
        this.CurrentCamera %= 2;

        //this.PlayerCameras[CurrentCamera].enabled = true;*/
    }

    public void SetGameOverCamera()
    {/*
        foreach(Camera c in PlayerCameras)
        {
            c.enabled = false;
        }
        this.GameOverCamera.enabled = true;*/
    }

    private class Observer : TurnObserver, EndObserver
    {
        private CameraController cc;

        public Observer(CameraController cc)
        {
            this.cc = cc;
            this.cc.UrHandler.AddTurnObserver(this);
            this.cc.UrHandler.AddEndObserver(this);
        }
        public void TurnUpdate()
        {
            this.cc.NextCamera();
        }
        public void GameOver()
        {
            this.cc.SetGameOverCamera();
        }

    }


}
