using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MC_ChangeScene : MonoBehaviour, MenuCommand
{
    public string TargetScene;
    public void RunMenuCommand()
    {
        SceneManager.LoadScene(TargetScene);
    }
}
