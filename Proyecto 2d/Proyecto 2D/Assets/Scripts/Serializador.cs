using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using Heroes;
using Mapa;
namespace MySerializeJson
{
    public class Serializador : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Tipo.AtacarPoder FireBall = new Tipo.AtacarPoder("Fire Ball", 10, 4, 25, 20);
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo.AtacarPoder AttackofAll = new Tipo.AtacarPoder("Attack of All", 4, 10, 100, 60);
            // Tipo.CurarPoder SanacionLeve = new Tipo.CurarPoder("Sanacion Leve", 1, 1, 4, 15, 20);
            // Tipo.CurarPoder SanacionAlta = new Tipo.CurarPoder("Sanacion Media", 1, 1, 5, 30, 40);
            // Tipo.CurarPoder SanacionAliada = new Tipo.CurarPoder("Sanacion Aliada", 1, 10, 7, 45, 25);
            // Tipo.MoverPoder AltaVelocidad = new Tipo.MoverPoder("Alta Velocidad", 7, 1, 8, 35);
            // Tipo.MoverPoder Huida = new Tipo.MoverPoder("Huida", 15, 1, 10, 80);
            // Tipo.DestruirPoder MineriaAvanzada = new Tipo.DestruirPoder("Mineria Avanzada", 8, 10, 8, 40);
            // Tipo.DestruirPoder MineriaBasica = new Tipo.DestruirPoder("Mineria Basica", 5, 8, 6, 40);
            // Tipo Knight = new Tipo("Knight", (int)Elemento.Fuego, (int)Elemento.Fuego, (int)Elemento.Agua, 1, FireBall);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
    [Serializable]
    public class MySerInit
    {
        public int MapSize;
        public bool[] Heroes;
        public bool[] Villians;
        public MySerInit(int MapSize, bool[] Heroes, bool[] Villians)
        {
            this.MapSize = MapSize;
            this.Heroes = Heroes;
            this.Villians = Villians;
        }
    }
}
