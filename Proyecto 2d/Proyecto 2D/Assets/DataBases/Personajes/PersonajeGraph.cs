using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Heroes;

[CreateAssetMenu(fileName = "PersonajeGra", menuName = "PeronajeGra")]
public class PersonajeGra : ScriptableObject
{
    public string nombre;
    public Sprite Img;
}
