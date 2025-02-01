using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using MySerializeJson;
using System.IO;
using Mapa;
using Partida;
using Heroes;
using Unity.VisualScripting;

public class ConstructordePartida : MonoBehaviour
{
    public Game game;
    [SerializeField]public List<GameObject> _Heroes;
    [SerializeField]public List<GameObject> _Villanos;
    [SerializeField] public List<GameObject> _Tokens; 
    public Sprite[] cursor;
    public List<Sprite> HeroesGra;
    public List<Sprite> Heroes
    {
        get{ return HeroesGra; }
        set{HeroesGra = value;}
    }
    public List<Sprite> toksgra;
    public List<Heroe> HeroesGame;
    public List<Heroe> VillanosGame;
    [SerializeField] public Transform[,] tilesmap;
    [SerializeField]public GameObject Techo;
    [SerializeField]public GameObject Techo2;
    [SerializeField]public GameObject Techo3;
    [SerializeField]public GameObject Pared1;
    [SerializeField]public GameObject Pared2;
    [SerializeField]public GameObject Pared3;
    [SerializeField]public GameObject Pared4;
    [SerializeField]public GameObject Camino;
    GameGraphic gamegra;
    void Start()
    {
        var rutaH = Path.Combine(Application.persistentDataPath, "DatosGame.json");
        var Datostext = File.ReadAllText(rutaH);
        MySerInit Datos = JsonUtility.FromJson<MySerInit>(Datostext);
        Construir(Datos);
    }
    public void Construir(MySerInit datos)
    {
        int cantEntr = 0;
        for (int i = 0; i < 6; i++)
        {
            if (datos.Heroes[i])
            {
                cantEntr++;
            }
        }
        mapa Map = new mapa(datos.MapSize, cantEntr, cantEntr);
        Map.Paredes(Map.ENTRDS, Map.SALDS);
        for (int i = 0; i < 6; i++)
        {
            if (datos.Villians[i] == true)
            {
                ConstruirV(i, Map);
            }
            if (datos.Heroes[i] == true)
            {
                ConstruirH(i, Map);
            }
        }

        game = new Game(Map, HeroesGame, VillanosGame);
        tilesmap = new Transform[Map.SIZE,Map.SIZE];
        // gamegra.Techo = Techo;
        // gamegra.Techo2 = Techo2;
        // gamegra.Techo3 = Techo3;
        // gamegra.Pared1 = Pared1;
        // gamegra.Pared2 = Pared2;
        // gamegra.Pared3 = Pared3;
        // gamegra.Pared4 = Pared4;
        // gamegra.Camino = Camino;
        Instantiate(Resources.Load<GameObject>("GameGraphic"), new Vector3(0,0,0), Quaternion.identity);
        Destroy(this.gameObject);
    }
    public void ConstruirH(int n, mapa Map)
    {
        switch (n)
        {
            case 0:
            #region Parte de Logica
            Tipo.AtacarPoder FireBall = new Tipo.AtacarPoder("Fire Ball", 10, 4, 25, 20);
            Tipo Knight = new Tipo("Knight", (int)Elemento.Fuego, (int)Elemento.Fuego, (int)Elemento.Agua, 1, FireBall);
            token Pawn = new token("Pawn", 1, 10, 50, 50, 4, 10);
            Heroe KnightFire = new Heroe("Knight Fire", Map, 10, 1, Knight, 4, 10, 90, 110, new token[]{Pawn});
            #endregion
            #region Grafica
            GameObject KnightFireO = new GameObject("KnightFire");
            SpriteRenderer sprite1  = KnightFireO.AddComponent<SpriteRenderer>();
            GameObject tokengra1 = new GameObject("token1");
            SpriteRenderer spritet1 = tokengra1.AddComponent<SpriteRenderer>();
            spritet1.sortingLayerName = "Knight";
            spritet1.sprite = toksgra[0];
            _Tokens.Add(tokengra1);
            GameObject cursorA = new GameObject("CursorActivo");
            GameObject cursorS = new GameObject("CursorSeleccion");
            cursorA.SetActive(false);
            cursorS.SetActive(false);
            SpriteRenderer cursorAsprite3 = cursorA.AddComponent<SpriteRenderer>(); 
            SpriteRenderer cursorSsprite = cursorS.AddComponent<SpriteRenderer>();
            cursorAsprite3.sortingLayerName = "Cursor";
            cursorSsprite.sortingLayerName = "Cursor";
            cursorAsprite3.sprite = cursor[0];
            cursorSsprite.sprite = cursor[1];
            cursorA.transform.SetParent(KnightFireO.transform);
            cursorS.transform.SetParent(KnightFireO.transform);
            cursorA.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursorA.transform.localPosition = new Vector3(0, 0.66f/29, 0);  
            //SpriteRenderer 
            GameObject cursortA = Instantiate(cursorA, new Vector3(0,0,0), Quaternion.identity);
            GameObject cursortS = Instantiate(cursorS, new Vector3(0,0,0), Quaternion.identity);
            cursortA.transform.SetParent(tokengra1.transform);
            cursortS.transform.SetParent(tokengra1.transform);
            cursortA.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursortA.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            sprite1.sortingLayerName = "Knight";
            sprite1.sprite = HeroesGra[0];
            HeroesGame.Add(KnightFire);
            _Heroes.Add(KnightFireO);
            #endregion
            break;
            case 1:
            Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            token MagicPawn = new token("Magic Pawn", 1, 5, 40, 40, 6, 12);
            Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            GameObject BlueMagicianO = new GameObject("BlueMagician");
            SpriteRenderer sprite2  = BlueMagicianO.AddComponent<SpriteRenderer>();
            GameObject tokengra2 = new GameObject("token2");
            SpriteRenderer spritet2 = tokengra2.AddComponent<SpriteRenderer>();
            spritet2.sortingLayerName = "Knight";
            spritet2.sprite = toksgra[1];
            _Tokens.Add(tokengra2);
            GameObject cursorA2 = new GameObject("CursorActivo");
            GameObject cursorS2 = new GameObject("CursorSeleccion");
            cursorA2.SetActive(false);
            cursorS2.SetActive(false);
            SpriteRenderer cursorAsprite2 = cursorA2.AddComponent<SpriteRenderer>(); 
            SpriteRenderer cursorSsprite2 = cursorS2.AddComponent<SpriteRenderer>();
            cursorAsprite2.sortingLayerName = "Cursor";
            cursorSsprite2.sortingLayerName = "Cursor";
            cursorAsprite2.sprite = cursor[0];
            cursorSsprite2.sprite = cursor[1];
            cursorA2.transform.SetParent(BlueMagicianO.transform);
            cursorS2.transform.SetParent(BlueMagicianO.transform);
            cursorA2.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS2.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS2.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursorA2.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            //SpriteRenderer 
            GameObject cursortA2 = Instantiate(cursorA2, new Vector3(0,0,0), Quaternion.identity);
            GameObject cursortS2 = Instantiate(cursorS2, new Vector3(0,0,0), Quaternion.identity);
            cursortA2.transform.SetParent(tokengra2.transform);
            cursortS2.transform.SetParent(tokengra2.transform);
            cursortA2.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS2.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS2.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursortA2.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            sprite2.sortingLayerName = "Knight";
            sprite2.sprite = HeroesGra[1];
            HeroesGame.Add(BlueMagician);
            _Heroes.Add(BlueMagicianO);
            break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
        }
    }
    public void ConstruirV(int n, mapa Map)
    {
        switch (n)
        {
            case 0:
            Tipo.AtacarPoder FireBall = new Tipo.AtacarPoder("Fire Ball", 10, 4, 25, 20);
            Tipo Knight = new Tipo("Knight", (int)Elemento.Fuego, (int)Elemento.Fuego, (int)Elemento.Agua, 1, FireBall);
            token DarkPawn = new token("Pawn", 0, 10, 50, 50, 4, 10);
            Heroe KnightFireVillian = new Heroe("Knight Fire", Map, 10, 0, Knight, 4, 10, 90, 110, new token[]{DarkPawn});
            GameObject KnightFireVillianO = new GameObject("KnightFireVillian");
            SpriteRenderer sprite3  = KnightFireVillianO.AddComponent<SpriteRenderer>();
            GameObject tokengra3 = new GameObject("token3");
            SpriteRenderer spritet3 = tokengra3.AddComponent<SpriteRenderer>();
            spritet3.sortingLayerName = "Knight";
            spritet3.sprite = toksgra[0];
            _Tokens.Add(tokengra3);
            GameObject cursorA3 = new GameObject("CursorActivo");
            GameObject cursorS3 = new GameObject("CursorSeleccion");
            cursorA3.SetActive(false);
            cursorS3.SetActive(false);
            SpriteRenderer cursorAsprite3 = cursorA3.AddComponent<SpriteRenderer>(); 
            SpriteRenderer cursorSsprite3 = cursorS3.AddComponent<SpriteRenderer>();
            cursorAsprite3.sortingLayerName = "Cursor";
            cursorSsprite3.sortingLayerName = "Cursor";
            cursorAsprite3.sprite = cursor[0];
            cursorSsprite3.sprite = cursor[1];
            cursorA3.transform.SetParent(KnightFireVillianO.transform);
            cursorS3.transform.SetParent(KnightFireVillianO.transform);
            cursorA3.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS3.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS3.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursorA3.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            //SpriteRenderer 
            GameObject cursortA3 = Instantiate(cursorA3, new Vector3(0,0,0), Quaternion.identity);
            GameObject cursortS3 = Instantiate(cursorS3, new Vector3(0,0,0), Quaternion.identity);
            cursortA3.transform.SetParent(tokengra3.transform);
            cursortS3.transform.SetParent(tokengra3.transform);
            cursortA3.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS3.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS3.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursortA3.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            sprite3.sortingLayerName = "Knight";
            sprite3.sprite = HeroesGra[0];
            VillanosGame.Add(KnightFireVillian);
            _Villanos.Add(KnightFireVillianO);
            break;
            case 1:
            Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            token DarkMagicPawn = new token("Magic Pawn", 0, 5, 40, 40, 6, 12);
            Heroe BlueMagicianVillian = new Heroe("Blue Magician", Map, 10, 0, Magician, 4, 10, 90, 110, new token[]{DarkMagicPawn});
            GameObject BlueMagicianVillianO = new GameObject("BlueMagicianVillian");
            SpriteRenderer sprite4  = BlueMagicianVillianO.AddComponent<SpriteRenderer>();
            GameObject tokengra4 = new GameObject("token4");
            SpriteRenderer spritet4 = tokengra4.AddComponent<SpriteRenderer>();
            spritet4.sortingLayerName = "Knight";
            spritet4.sprite = toksgra[1];
            _Tokens.Add(tokengra4);
            GameObject cursorA4 = new GameObject("CursorActivo");
            GameObject cursorS4 = new GameObject("CursorSeleccion");
            cursorA4.SetActive(false);
            cursorS4.SetActive(false);
            SpriteRenderer cursorAsprite4 = cursorA4.AddComponent<SpriteRenderer>(); 
            SpriteRenderer cursorSsprite4 = cursorS4.AddComponent<SpriteRenderer>();
            cursorAsprite4.sortingLayerName = "Cursor";
            cursorSsprite4.sortingLayerName = "Cursor";
            cursorAsprite4.sprite = cursor[0];
            cursorSsprite4.sprite = cursor[1];
            cursorA4.transform.SetParent(BlueMagicianVillianO.transform);
            cursorS4.transform.SetParent(BlueMagicianVillianO.transform);
            cursorA4.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS4.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursorS4.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursorA4.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            //SpriteRenderer
            GameObject cursortA4 = Instantiate(cursorA4, new Vector3(0,0,0), Quaternion.identity);
            GameObject cursortS4 = Instantiate(cursorS4, new Vector3(0,0,0), Quaternion.identity);
            cursortA4.transform.SetParent(tokengra4.transform);
            cursortS4.transform.SetParent(tokengra4.transform);
            cursortA4.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS4.transform.localScale = new Vector3(0.018f, 0.018f, 0.018f);
            cursortS4.transform.localPosition = new Vector3(0, 0.66f/29, 0);
            cursortA4.transform.localPosition = new Vector3(0, 0.66f/29, 0); 
            sprite4.sortingLayerName = "Knight";
            sprite4.sprite = HeroesGra[1];
            VillanosGame.Add(BlueMagicianVillian);
            _Villanos.Add(BlueMagicianVillianO);
            break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
            // case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;case 1:
            // Tipo.AtacarPoder MagicStorm = new Tipo.AtacarPoder("Magic Storm", 12, 3, 25, 15);
            // Tipo Magician = new Tipo("Magician", (int)Elemento.Agua, (int)Elemento.Agua, (int)Elemento.Fuego, 1, MagicStorm);
            // token MagicPawn = new token("Magic Pawn", 5, 40, 40, 6, 12);
            // Heroe BlueMagician = new Heroe("Blue Magician", Map, 10, 1, Magician, 4, 10, 90, 110, new token[]{MagicPawn});
            // break;
        }
    }
}
