using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Jugable : MonoBehaviour
{
    const float velocidad = .25f;
    Vector3 destino;
    public bool moving;
    public IEnumerator MoverDestino(Vector3 nuevo)
    {
        destino = nuevo;
        moving = true;
        float time = 0;
        while(time < velocidad)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, time/velocidad);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = destino;
        moving = false;
    } 

}
