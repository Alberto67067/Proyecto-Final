using UnityEngine;
using System;
using UnityEngine.UI;
using Unity;
using UnityEngine.SceneManagement;

public class ButtomExit : MonoBehaviour
{
    public void Exit()
    {
        //Debug.Log("AAAA");
        Application.Quit();
    }
    public void Goto2()
    {
        SceneManager.LoadScene("Inicio de Partida");
    }
}
