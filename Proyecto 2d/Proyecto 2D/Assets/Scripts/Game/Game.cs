using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Mapa;
using Partida;
using Heroes;
using System.Collections;
using TMPro;
using Mono.Cecil.Cil;


public class GameGraphic : MonoBehaviour
{
    //[SerializeField]public mapa MAP;
    public Game game;
    [SerializeField]public List<GameObject> _Heroes;
    [SerializeField]public List<GameObject> _Villanos;
    [SerializeField] public List<GameObject> _Tokens; 
    [SerializeField]public Transform[,] tilesmap;
    [SerializeField]public GameObject Techo;
    [SerializeField]public GameObject Techo2;
    [SerializeField]public GameObject Techo3;
    [SerializeField]public GameObject Pared1;
    [SerializeField]public GameObject Pared2;
    [SerializeField]public GameObject Pared3;
    [SerializeField]public GameObject Pared4;
    [SerializeField]public GameObject Camino;
    int turno = 0;
    int turnoreal = 0;
    bool Vict = false;
    bool cambioturno = false;
    bool HoV = false;
    int velPA = 0;
    Heroe heroe;
    GameObject heroegraph;
    //int contador = 0;
    public void Start()
    {
        Construir();
        Generar(game.MAP);
        for (int i = 0; i < _Heroes.Count; i++)
        {
            _Heroes[i].transform.position = new Vector2(game.HeroesGame[i].pos0.Item1 + 0.5f, game.HeroesGame[i].pos0.Item2 + 0.5f);
            _Heroes[i].AddComponent<Jugable>();
            //Debug.Log(game.HeroesGame[i].pos0);
            //Debug.Log(game.VillansGame[i].pos0);
            _Villanos[i].transform.position = new Vector2(game.VillansGame[i].pos0.Item1 + 0.5f, game.VillansGame[i].pos0.Item2 + 0.5f);
            _Villanos[i].AddComponent<Jugable>();
            _Heroes[i].transform.localScale = new Vector3(29, 29, 29);
            _Villanos[i].transform.localScale = new Vector3(29, 29, 29);
        }
        foreach (var item in _Tokens)
        {
            item.SetActive(false);
            item.transform.localScale = new Vector3(29, 29, 29);
            item.AddComponent<Jugable>();
        }
        StartCoroutine(JugarTurno());
    }
    public void Generar(mapa map)
    {
        for (int i = 0; i < map.SIZE; i++)
        {
            for (int j = 0; j < map.SIZE; j++)
            {
                Vector2 pos = new Vector2(i + 0.5f, j + 0.5f);
                if(map.MAP[i,j] != 1)
                {
                    GameObject clon = Instantiate(Camino, pos, Quaternion.identity);
                    tilesmap[i,j] = clon.transform;
                }
                else if(map.MAP[i,j] == 1)
                {
                    if(map.PosicionValida(map.MAP, i - 1, j, true) && map.PosicionValida(map.MAP, i, j+ 1, true) && map.PosicionValida(map.MAP, i + 1, j, true) && map.MAP[i,j + 1] == 0 && map.MAP[i - 1, j] == 0 && map.MAP[i + 1, j] == 0)
                    {
                        GameObject clon = Instantiate(Pared1, pos, Quaternion.identity);
                        tilesmap[i,j] = clon.transform;
                    }
                    else if(map.PosicionValida(map.MAP, i - 1, j, true) && map.PosicionValida(map.MAP, i, j+ 1, true) && map.MAP[i,j + 1] == 0 && map.MAP[i - 1, j] == 0)
                    {
                        GameObject clon2 = Instantiate(Pared2, pos, Quaternion.identity);
                        tilesmap[i,j] = clon2.transform;
                    }
                    else if(map.PosicionValida(map.MAP, i + 1, j, true) && map.PosicionValida(map.MAP, i, j+ 1, true) && map.MAP[i,j + 1] == 0 && map.MAP[i + 1, j] == 0)
                    {
                        GameObject clon3 = Instantiate(Pared3, pos, Quaternion.identity);
                        tilesmap[i,j] = clon3.transform;
                    }
                    else if(map.PosicionValida(map.MAP, i, j+ 1, true) && map.MAP[i,j + 1] == 0 /*&& map.MAP[i - 1, j] == 0 && map.PosicionValida(map.MAP, i - 1, j, true)*/)
                    {
                        GameObject clon4 = Instantiate(Pared4, pos, Quaternion.identity);
                        tilesmap[i,j] = clon4.transform;
                    }
                    else
                    {
                       if(map.PosicionValida(map.MAP,i - 1, j, true) && map.MAP[i - 1,j] == 0)
                       {
                            GameObject clon5 = Instantiate(Techo, pos, Quaternion.identity);
                            tilesmap[i,j] = clon5.transform;
                       }
                       else if(map.PosicionValida(map.MAP,i + 1, j, true) && map.MAP[i + 1,j] == 0)
                       {
                            GameObject clon6 = Instantiate(Techo2, pos, Quaternion.identity);
                            tilesmap[i,j] = clon6.transform;
                       }
                       else
                       {
                            GameObject clon7 = Instantiate(Techo3, pos, Quaternion.identity);
                            tilesmap[i,j] = clon7.transform;
                       }
                    }
                }
            }
        }
    }
    public IEnumerator JugarTurno()
    {
        while(!Vict)
        {
            //Esperando = true;
            if (!HoV)
            {
                heroe = game.HeroesGame[turno];
                heroegraph = _Heroes[turno];
            }
            else if (HoV)
            {
                heroe = game.VillansGame[turno];
                heroegraph = _Villanos[turno];
            }
            heroegraph.transform.GetChild(0).gameObject.SetActive(true);
            yield return StartCoroutine(Jugar(heroe));
            heroegraph.transform.GetChild(0).gameObject.SetActive(false);
            if (cambioturno)
            {
                turno = (turno + 1) % game.HeroesGame.Count;
                cambioturno = false;
                velPA = 0;
                if (turno % game.HeroesGame.Count == 0)
                {
                    HoV = !HoV;
                }
            }
            turnoreal++;
        }
    }
    public IEnumerator Jugar(Heroe heroe)
    {
        heroe.energia += 10;
        if (heroe.energia > heroe.energiaMax)
        {
            heroe.energia = heroe.energiaMax;
        }
        while(!cambioturno)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, heroe.pos.Item1, heroe.pos.Item2 + 1, false))
                    {
                        //ismoving = true;
                        heroe.rastro.Enqueue(new huella(heroe.pos, (int)Direccion.Norte));
                        heroe.pos = (heroe.pos.Item1, heroe.pos.Item2 + 1);
                        yield return StartCoroutine(heroegraph.GetComponent<Jugable>().MoverDestino(heroegraph.transform.position + Vector3.up));
                        //ismoving = false;
                        if (velPA == heroe.velocidad)
                            cambioturno = true;
                        //contador++;
                        else
                            velPA++;
                    }
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, heroe.pos.Item1, heroe.pos.Item2 - 1, false))
                    {
                        //ismoving = true;
                        heroe.rastro.Enqueue(new huella(heroe.pos, (int)Direccion.Sur));
                        heroe.pos = (heroe.pos.Item1, heroe.pos.Item2 - 1);
                        yield return StartCoroutine(heroegraph.GetComponent<Jugable>().MoverDestino(heroegraph.transform.position + Vector3.down));
                        //ismoving = false;
                        if (velPA == heroe.velocidad)
                            cambioturno = true;
                        else
                            velPA++;
                        //contador++;
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, heroe.pos.Item1 - 1, heroe.pos.Item2, false))
                    {
                        //ismoving = true;
                        heroe.rastro.Enqueue(new huella(heroe.pos, (int)Direccion.Izqu));
                        heroe.pos = (heroe.pos.Item1 - 1, heroe.pos.Item2);
                        yield return StartCoroutine(heroegraph.GetComponent<Jugable>().MoverDestino(heroegraph.transform.position + Vector3.left));
                        //ismoving = false;
                        if (velPA == heroe.velocidad)
                            cambioturno = true;
                        else
                            velPA++;
                        // contador++;
                    }
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, heroe.pos.Item1 + 1, heroe.pos.Item2, false))
                    {
                        //ismoving = true;
                        heroe.rastro.Enqueue(new huella(heroe.pos, (int)Direccion.Derecha));
                        heroe.pos = (heroe.pos.Item1 + 1, heroe.pos.Item2);
                        yield return StartCoroutine(heroegraph.GetComponent<Jugable>().MoverDestino(heroegraph.transform.position + Vector3.right));
                        //ismoving = false;
                        if (velPA == heroe.velocidad)
                            cambioturno = true;
                        else
                            velPA++;
                        //contador++;
                    }
                }
                else if(Input.GetKey(KeyCode.F) && heroe.rastro.Count >= 12)
                {
                    if(heroe.tokens[0].SUMMON == false && heroe.energia >= 50)
                    {
                        heroe.energia -= 50;
                        heroe.tokens[0].SUMMON = true;
                        if(heroe.Bando == 0)
                        {
                            _Tokens[turno + _Heroes.Count].SetActive(true);
                            List<huella> ras = heroe.rastro.ToList<huella>();
                            heroe.tokens[0].POS = ras[ras.Count - 2].pos;
                            _Tokens[turno + _Heroes.Count].transform.position = new Vector3(heroe.tokens[0].POS.Item1 + .5f, heroe.tokens[0].POS.Item2 + .5f, 0f);
                        }
                        else
                        {
                            _Tokens[turno].SetActive(true);
                            List<huella> ras = heroe.rastro.ToList<huella>();
                            heroe.tokens[0].POS = ras[ras.Count - 2].pos;
                            _Tokens[turno].transform.position = new Vector3(heroe.tokens[0].POS.Item1 + .5f, heroe.tokens[0].POS.Item2 + .5f, 0f);
                        }
                    }
                }
                else if(Input.GetKey(KeyCode.Q))
                {
                    //StartCoroutine(AtacarPoder());
                }
                else if (Input.GetKey(KeyCode.Space))
                {
                    Vict = true;
                }
            }
            if(game.map.MAP[heroe.pos.Item1, heroe.pos.Item2] == 2)
            {
                heroe.vida -= 10;
            }
            else if(game.map.MAP[heroe.pos.Item1, heroe.pos.Item2] == 3)
            {
                cambioturno = true;
            }
            // if (heroe.tokens[0].SUMMON == true)
            // {
                
            // }
            yield return null;
        }
        if (heroe.tokens[0].SUMMON == true)
        {
            if (heroe.Bando == 0)
            {
                _Tokens[turno + _Heroes.Count].transform.GetChild(0).gameObject.SetActive(true);
                yield return StartCoroutine(JugarToken(heroe.tokens[0], _Tokens[turno + _Heroes.Count]));
                _Tokens[turno + _Heroes.Count].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                _Tokens[turno].transform.GetChild(0).gameObject.SetActive(true);
                yield return StartCoroutine(JugarToken(heroe.tokens[0], _Tokens[turno]));
                _Tokens[turno].transform.GetChild(0).gameObject.SetActive(false);
            }
            heroe.tokens[0].TSUMON++;
            if(heroe.tokens[0].IsDead())//Ahorita reviso si poner esto en el update
            {
                if (heroe.Bando == 0)
                {
                    _Tokens[turno + _Heroes.Count].SetActive(false);
                }
                else
                {
                    _Tokens[turno].SetActive(false);
                }
            }
        }
    }
    public IEnumerator JugarToken(token tok, GameObject tokgraph)
    {
        int contador = 0;
        bool caminando = true;
        while(caminando)
        {
            //Curs2(tokgraph.transform.GetChild(0).gameObject);
            if (Input.anyKeyDown)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, tok.POS.Item1, tok.POS.Item2 + 1, false))
                    {
                        //ismoving = true;
                        tok.POS = (tok.POS.Item1, tok.POS.Item2 + 1);
                        yield return StartCoroutine(tokgraph.GetComponent<Jugable>().MoverDestino(tokgraph.transform.position + Vector3.up));
                        //ismoving = false;
                        contador++;
                    }
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, tok.POS.Item1, tok.POS.Item2 - 1, false))
                    {
                        //ismoving = true;
                        tok.POS = (tok.POS.Item1, tok.POS.Item2 - 1);
                        yield return StartCoroutine(tokgraph.GetComponent<Jugable>().MoverDestino(tokgraph.transform.position + Vector3.down));
                        contador++;
                    }
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, tok.POS.Item1 - 1, tok.POS.Item2, false))
                    {
                        //ismoving = true;
                        tok.POS = (tok.POS.Item1 - 1, tok.POS.Item2);
                        yield return StartCoroutine(tokgraph.GetComponent<Jugable>().MoverDestino(tokgraph.transform.position + Vector3.left));
                        contador++;
                    }
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (heroe.map.PosicionValida(heroe.map.MAP, tok.POS.Item1 + 1, tok.POS.Item2, false))
                    {
                        tok.POS = (tok.POS.Item1 + 1, tok.POS.Item2);
                        yield return StartCoroutine(tokgraph.GetComponent<Jugable>().MoverDestino(tokgraph.transform.position + Vector3.right));
                        contador++;
                    }
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    // Objetivo target = Choise(tok, 0);
                    // if (target != null)
                    // {
                    //     target.vida -= tok.Damage;
                    // }
                }
            }

            if (contador >= tok.VELOCIDAD)
            {
                caminando = false;
            }
            if(game.map.MAP[tok.pos.Item1, tok.pos.Item2] == 2)
            {
                tok.vida -= 10;
            }
            else if(game.map.MAP[tok.pos.Item1, tok.pos.Item2] == 3)
            {
                caminando = false;
            }
            yield return null;
        }
    } 
    /// <summary>
    /// Selecciona los posibles objetivos
    /// </summary>
    /// <param name="lanz"></param>
    /// <param name="TargetsGra"></param>
    /// <param name="Targets"></param>
    /// <param name="i"></param>
    public void Select(Objetivo lanz, ref List<GameObject> TargetsGra, ref List<Objetivo> Targets, int i)
    {
        if(lanz is Heroe lanzh)
        {
            if(lanzh.Bando == 0)
            {
                foreach (Heroe item in game.HeroesGame)
                {
                    if(lanzh.EstaEnRango(item.pos, lanz.pos, lanzh.tipo.poderes[i].rango, lanz.map))
                    {
                        Targets.Add(item);
                        TargetsGra.Add(_Heroes[game.HeroesGame.IndexOf(item)]);
                        if(item.tokens[0].SUMMON && lanzh.EstaEnRango(item.tokens[0], lanz, lanzh.tipo.poderes[i].rango, lanz.map))
                        {
                            TargetsGra.Add(_Tokens[game.HeroesGame.IndexOf(item)]);
                            Targets.Add(item.tokens[0]);
                        }
                    }
                }
            }
            else
            {
                foreach (Heroe item in game.VillansGame)
                {
                    if(lanzh.EstaEnRango(item.pos, lanz.pos, lanzh.tipo.poderes[i].rango, lanz.map))
                    {
                        Targets.Add(item);
                        TargetsGra.Add(_Villanos[game.VillansGame.IndexOf(item)]);
                        if(item.tokens[0].SUMMON && lanzh.EstaEnRango(item.tokens[0], lanz, lanzh.tipo.poderes[i].rango, lanz.map))
                        {
                            TargetsGra.Add(_Tokens[game.HeroesGame.IndexOf(item) + game.VillansGame.Count]);
                            Targets.Add(item.tokens[0]);
                        }
                    }
                }
            }
        }
        else if(lanz is token lanzt)
        {
            if(lanzt.Bando == 0)
            {
                foreach (Heroe item in game.HeroesGame)
                {
                    if(lanzt.EstaEnRango(lanzt, item))
                    {
                        Targets.Add(item);
                        TargetsGra.Add(_Heroes[game.HeroesGame.IndexOf(item)]);
                        if(item.tokens[0].SUMMON && lanzt.EstaEnRango(lanzt, item.tokens[0]))
                        {
                            TargetsGra.Add(_Tokens[game.HeroesGame.IndexOf(item)]);
                            Targets.Add(item.tokens[0]);
                        }
                    }
                }
            }
            else
            {
                foreach (Heroe item in game.VillansGame)
                {
                    if(lanzt.EstaEnRango(lanzt, item))
                    {
                        Targets.Add(item);
                        TargetsGra.Add(_Villanos[game.VillansGame.IndexOf(item)]);
                        if(item.tokens[0].SUMMON && lanzt.EstaEnRango(lanzt, item.tokens[0]))
                        {
                            TargetsGra.Add(_Tokens[game.VillansGame.IndexOf(item) + game.HeroesGame.Count]);
                            Targets.Add(item.tokens[0]);
                        }
                    }
                }
            }
        }
    }
    // public IEnumerator Iterete(GameObject TargetGra)
    // {
    //     TargetGra.transform.GetChild(1).gameObject.SetActive(true);
    //     yield return new WaitForSeconds(0.2f);
    //     TargetGra.transform.GetChild(1).gameObject.SetActive(false);
    //     yield return new WaitForSeconds(.2f);
    //     StopCoroutine("Iterete");
    //     //yield return Targets[indice];
    // }
    // private Objetivo Choise(Objetivo lanz, int i)
    // {
    //     int indice = 0;
    //     bool lanzado = true;
    //     List<Objetivo> Targets = new List<Objetivo>(); 
    //     List<GameObject> TargetsGra = new List<GameObject>();
    //     Select(lanz, ref TargetsGra, ref Targets, i);
    //     IEnumerator indice2 = Choise2(indice, lanzado, Targets, TargetsGra);
    //     indice = (int)indice2.Current;
    //     return Targets[indice];
    // }
    // public IEnumerator Choise2(int indice, bool lanzado, List<Objetivo> Targets, List<GameObject> TargetsGra)
    // {
    //     while(lanzado)
    //     {
    //         indice = indice % Targets.Count;
    //         TargetsGra[indice].transform.GetChild(1).gameObject.SetActive(true);
    //         if(Input.anyKeyDown)
    //         {
    //             if (Input.GetKeyDown(KeyCode.RightArrow))
    //             {
    //                 indice++;
    //             }
    //             else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //             {
    //                 indice--;
    //             }
    //             else if (Input.GetKeyDown(KeyCode.E))
    //             {
    //                 lanzado = false;
    //             }
    //         }
    //         TargetsGra[indice].transform.GetChild(1).gameObject.SetActive(false);
    //     }
    //     return new choisenewEnumerator(indice);
        
    // }
    public class choisenewEnumerator : IEnumerator
    {
        int indice;
        public choisenewEnumerator(int indice)
        {
            this.indice = indice;
        }

        public object Current => indice;

        public bool MoveNext()
        {
            return true;
        }

        public void Reset()
        {
        }
    }
    // public IEnumerator AtacarPoder()
    // {
    //     Objetivo target = Choise(heroe, 0);
    //     if (heroe.tipo.poderes[0] is Tipo.AtacarPoder a)
    //     {
    //         if (target != null && heroe.energia >= a.energiaNec)
    //         {
    //             heroe.energia -= a.energiaNec;
    //             a.Atacar(target);
    //         }
    //     }
    //     yield return null;
    // }
    public void Construir()
    {
        ConstructordePartida a = GameObject.Find("Constructor").GetComponent<ConstructordePartida>();
        Techo = a.Techo;
        Techo2 = a.Techo2;
        Techo3 = a.Techo3;
        Pared1 = a.Pared1;
        Pared2 = a.Pared2;
        Pared3 = a.Pared3;
        Pared4 = a.Pared4;
        Camino = a.Camino;
        tilesmap = a.tilesmap;
        game = a.game;
        _Heroes = a._Heroes;
        _Villanos = a._Villanos;
        _Tokens = a._Tokens;
        Destroy(a);
    }
}
