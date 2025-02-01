using UnityEngine;
using System;
using Mapa;
using Heroes;
using System.Collections;
using Unity.VisualScripting;

public class MiniMAze5x5 : MonoBehaviour
{
    public int Size = 5;
    public const float moveSpeed = 0.25f;
    public bool ismoving;
    private mapa mapInic;
    public Transform[,] tileTrans;
    public GameObject Knight;
    public GameObject Camino;
    public GameObject Pared;
    public GameObject Techo;
    public UnityEngine.Vector2 move;

    private void Start()
    {
        tileTrans = new Transform[Size, Size];
        mapInic = new mapa(Size,1,1);
        Vector3 posk = new Vector2(mapInic.ENTRDS[0].Item1 - 2,mapInic.ENTRDS[0].Item2 - 2);
        Knight.transform.position = posk;
        move = posk;
        mapInic.Paredes(mapInic.ENTRDS, mapInic.SALDS);
        StartCoroutine(Generar(mapInic.MAP));
        //StopAllCoroutines();
        
    }
    private void Update()
    {
        if(Input.anyKeyDown && !ismoving)
        {
            MoverKnight2(mapInic.MAP);
            StartCoroutine(Mover());
        }
    }
    IEnumerator Generar(int[,] matriz)
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Vector2 pos = new Vector2(i - 2, j - 2);
                if(matriz[i,j] == 0)
                {
                    if(PosicionValida(matriz, i,j + 1, true) && matriz[i,j + 1] == 1)
                    {
                        GameObject clon = Instantiate(Pared, pos, Quaternion.identity);
                        tileTrans[i,j] = clon.transform;
                    }
                    else
                    {
                        GameObject clon3 = Instantiate(Camino, pos, Quaternion.identity);
                        tileTrans[i,j] = clon3.transform;
                    }
                }
                else if(matriz[i,j] == 1)
                {
                    GameObject clon2 = Instantiate(Techo, pos, Quaternion.identity);
                    tileTrans[i,j] = clon2.transform;
                }
                yield return null;
            }
        }
    }
    public bool PosicionValida(int[,] mapa,int x, int y, bool generador)
    {
        if(x < 0 || x >= mapa.GetLength(0))
        {
            return false;
        }
        if(y < 0 || y >= mapa.GetLength(1))
        {
            return false;
        }
        if(!generador)
        {
            if (mapa[x,y] == 1)
            {
                return false;
            }
        }
        return true;
    }
    // public void MoverKnight(int[,] matriz)
    // {
    //     ismoving = true;
    //     float moveElapsed = 0f;
    //     if(Math.Abs(Input.GetAxisRaw("Horizontal")) == 1 || Math.Abs(Input.GetAxisRaw("Vertical")) == 1)
    //     {
    //         if(Input.GetKey("right") && PosicionValida(matriz, (int)Knight.transform.position.x + 3, (int)Knight.transform.position.y + 2))
    //         {
    //             Vector3 temp = new Vector3(Knight.transform.position.x + 1 , Knight.transform.position.y);
    //             while(moveElapsed < moveSpeed)  
    //             {  
    //                 Knight.transform.position = Vector3.Lerp(Knight.transform.position, temp, moveElapsed/moveSpeed);
    //                 moveElapsed += Time.deltaTime;                
    //             }
    //             Knight.transform.position = temp;
    //         }
    //         else if(Input.GetKey("left") && PosicionValida(matriz, (int)Knight.transform.position.x - 1 + 2, (int)Knight.transform.position.y + 2))
    //         {
    //             Vector3 temp = new Vector3(Knight.transform.position.x - 1, Knight.transform.position.y);
    //             Knight.transform.position = Vector3.Lerp(Knight.transform.position, temp, moveElapsed/moveSpeed);
    //             Knight.transform.position = temp;
    //         }
    //         else if(Input.GetKey("down") && PosicionValida(matriz, (int)Knight.transform.position.x + 2, (int)Knight.transform.position.y - 1 + 2))
    //         {
    //             Vector3 temp = new Vector3(Knight.transform.position.x, Knight.transform.position.y - 1);
    //             Knight.transform.position = Vector3.Lerp(Knight.transform.position, temp, moveElapsed/moveSpeed);
    //             Knight.transform.position = temp;
    //         }
    //         else if(Input.GetKey("up") && PosicionValida(matriz, (int)Knight.transform.position.x + 2, (int)Knight.transform.position.y + 3))
    //         {
    //             Vector3 temp = new Vector3(Knight.transform.position.x, Knight.transform.position.y + 1);
    //             Knight.transform.position = Vector3.Lerp(Knight.transform.position, temp, moveElapsed/moveSpeed);
    //             Knight.transform.position = temp;
    //         }
    //     }
    //     ismoving = false;
    // }
    public void MoverKnight2(int[,] matriz)
    {
        if(Math.Abs(Input.GetAxisRaw("Horizontal")) == 1 && PosicionValida(matriz, (int)Input.GetAxisRaw("Horizontal") + (int)Math.Round(Knight.transform.position.x) + 2, (int)Math.Round(Knight.transform.position.y) + 2, false))
        {
            move = new Vector2(Knight.transform.position.x + Input.GetAxisRaw("Horizontal"), Knight.transform.position.y);
        }
        else if(Math.Abs(Input.GetAxisRaw("Vertical")) == 1 && PosicionValida(matriz, (int)Math.Round(Knight.transform.position.x) + 2, (int)Input.GetAxisRaw("Vertical") + (int)Math.Round(Knight.transform.position.y) + 2, false))
        {
            move = new Vector2(Knight.transform.position.x, Knight.transform.position.y + Input.GetAxisRaw("Vertical"));
        }
    }
    IEnumerator Mover()
    {
        ismoving = true;
        float timeElp = 0;
        while(timeElp < moveSpeed)
        {
            Knight.transform.position = Vector2.Lerp(Knight.transform.position, move, timeElp/moveSpeed);
            timeElp += Time.deltaTime;
            yield return null;
        }
        Knight.transform.position = move;
        ismoving = false;
    }
}

