using UnityEngine;
using Unity.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Heroes;
using Mapa;
using MapaB;
using Partida;
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using MySerializeJson;

[Serializable]
public class ButtomGoToPlay : MonoBehaviour
{
    [SerializeField] List<Toggle> togglesH;
    [SerializeField] List<Toggle> togglesV;
    //[SerializeField] List<> gameObjects;
    [SerializeField] InputField Size;
    [SerializeField] bool TypeofMap = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Iniciar()
    {
        int MapSize = int.Parse(Size.text);
        bool[] indexoftogglesH = new bool[6];
        bool[] indexoftogglesV = new bool[6];
        for (int i = 0; i < 6; i++)
        {
            if(togglesH[i].isOn)
            {
                indexoftogglesH[i] = true;
            }
            if(togglesV[i].isOn)
            {
                indexoftogglesV[i] = true;
            }
        }
        MySerInit DatosGame = new MySerInit(MapSize, indexoftogglesH, indexoftogglesV);
        var DatosGame1 = JsonUtility.ToJson(DatosGame);
        var rutaH = Path.Combine(Application.persistentDataPath, "DatosGame.json");
        File.WriteAllText(rutaH, DatosGame1);
        //Debug.Log(Application.persistentDataPath);
        SceneManager.LoadScene("Game");
    }
}

