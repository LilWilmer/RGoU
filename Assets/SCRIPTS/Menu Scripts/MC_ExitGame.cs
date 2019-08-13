using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_ExitGame : MonoBehaviour, MenuCommand {

    public void RunMenuCommand()
    {
        Application.Quit();
    }
}
