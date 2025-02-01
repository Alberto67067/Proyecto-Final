using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Heroes;

public class Personaje : MonoBehaviour
{
    //public PersonajeGra Graph;
    public Heroe heroe; 
    public GameObject tokengra = new GameObject("tokengra");
    public Personaje(PersonajeGra Graph, Heroe heroe)
    {
        //this.Graph = Graph;
        this.heroe = heroe;
    }
    public void Start()
    {

    }
    
}
