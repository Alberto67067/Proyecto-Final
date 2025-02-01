//Esta sera la version 1.00 del proyecto 28/10/2024
//Lo primero que hare sera el mapa
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapa;
using Heroes;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Dynamic;
using System.IO.Pipes;
namespace Partida
{
    [Serializable]
    public class Game
    {
        public int contador=0;
        public int ctkns=0;
        Random random;
        public mapa map;
        public mapa MAP{
            get{return map;}
            set{map=value;}
        }
        string[,] shower;
        public List<Heroe> HeroesGame;
        public List<Heroe> VillansGame;
        public List<Heroe>[] bands;
        public Game(int tamñmap, int cantB)
        {
            map = new mapa(tamñmap, 1, 1);
            map.Paredes(map.ENTRDS, map.SALDS);
            Tipo.Poder FBall = new Tipo.Poder();
            Tipo demon = new Tipo("Demonio", (int)Elem.Fuego, (int)Elem.Viento, (int)Elem.Agua, 1, FBall);
            token normal = new token("N", 1,15, 40, 40, 3, 7); 
            token[] a = {normal};
            Heroe Sid = new Heroe("☻", map, 10, 1, demon, 4, 4, 100, 125, a);
            Heroe Ant = new Heroe("☺", map, 4, 0, demon , 5, 4, 90, 100, a);
            Heroe Mandy = new Heroe("X", map, 4, 0, demon , 5, 4, 90, 100, a);
            bands = new List<Heroe>[cantB];
            HeroesGame = new List<Heroe>{Sid};
            VillansGame = new List<Heroe>{Ant, Mandy};
            bands[0] = HeroesGame;
            bands[1] = VillansGame;
            this.shower = new string[map.SIZE, map.SIZE];
            // for (int i = 0; i < map.SIZE; i++)
            // {
            //     for (int j = 0; j < map.SIZE; j++)
            //     {
            //         if(map.MAP[i, j] == true)
            //         {
            //             shower[i, j] = " ";
            //         }
            //         else
            //         {
            //             shower[i, j] = "█";
            //         }
            //     }
            // }
            foreach((int, int) item in map.SALDS)
            {
                shower[item.Item1, item.Item2] = "F";
            }
        }
        public Game(mapa map, List<Heroe> HeroesGame, List<Heroe> VillansGame)
        {
            this.map = map;
            this.HeroesGame = HeroesGame;
            this.VillansGame = VillansGame;
        }
        static void Main()
        {
            //La logica del juego
            Game game = new Game(10, 2);
            bool victH = false;
            bool victV = false;
            int cantTurnos = 0;
            game.contador = 0;
            while((!victH || victV) && (victH || !victV))//lo deje implementado solo para los bandos 0 y 1, los unicos que hay en el juego por ahora, per de la forma que esta hecho es facil agregar mas bandos con los roles que el usuario decida, solo necesita agregarlo al listado de bandos. Y dependiendo de sus funcionalidades hacer algunas pequeñas modificaciones en la mecanica, si el bando que se agrega no necesita ninguna condicion de victoria es hasta mas sencillo
            {
                for (int j = 0; j < game.bands.Length; j++)
                {
                    for (int i = 0; i < game.bands[j].Count; i++)
                    {
                        while(game.contador < game.bands[j][i].velocidad)
                        {
                            foreach (var ite in game.bands)
                            {
                                foreach (var item in ite)
                                {
                                    game.shower[item.pos.Item1, item.pos.Item2] = item.nombre;
                                }
                            }
                            game.ShowMapG(game.shower);
                            foreach (var item in game.bands[j][i].rastro)
                            {
                                System.Console.WriteLine(item.pos.Item1 +" " + item.pos.Item2);
                            }
                            game.shower[game.bands[j][i].pos.Item1, game.bands[j][i].pos.Item2] = " ";
                            if(game.bands[j][i].Bando == 0)
                            {
                                System.Console.WriteLine("Juega Villano" + game.bands[j][i].nombre);
                            }
                            else
                            {
                                System.Console.WriteLine("Juega Heroe" + game.bands[j][i].nombre);
                            }
                            ConsoleKeyInfo move = Console.ReadKey();
                            game.Movimiento(move, game.map, game.bands[j][i]);
                            Console.Clear();
                            if(game.bands[j][i].Bando == 1)
                            {
                                victH = game.CheckWin1(game.bands[j][i].pos, game.map.SALDS);
                            }    
                            else
                            {    
                                victV = game.CheckWin2(game.bands[0], game.bands[1]);
                            }
                            if(victV || victH)
                            {
                                if(victV)
                                {
                                    System.Console.WriteLine("Gana el Villano");
                                }
                                else
                                {
                                    System.Console.WriteLine("Gana el Heroe");
                                }
                                Console.ReadKey();
                                break;
                            }
                            if(game.bands[j][i].rastro.Count >= game.bands[j][i].TAMANRASTRO)
                            {
                                game.bands[j][i].rastro.Dequeue();
                            }
                        }
                        if(victV || victH)
                        {
                            break;
                        }
                        foreach (var item in game.bands[j][i].tokens)
                        {
                            if(item.VIDA <= 0 || item.TSUMON >= item.DURACION)
                            {
                                item.IsDead();
                            }
                            else if (item.SUMMON)
                            {
                                while(game.ctkns < item.VELOCIDAD)    
                                {
                                    game.shower[item.POS.Item1, item.POS.Item2] = item.NOMBRE;
                                    game.ShowMapG(game.shower);
                                    System.Console.WriteLine("Juega token" + item.NOMBRE);
                                    game.shower[item.POS.Item1, item.POS.Item2] = " ";
                                    ConsoleKeyInfo move2 = Console.ReadKey();
                                    game.MovTkns(move2, game.map, item);
                                    Console.Clear();
                                }
                                game.ctkns = 0;
                            }
                        }
                        cantTurnos++;
                        game.contador = 0;
                    }
                    if(victV || victH)
                    {
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Este metedo chequea la condicion de victoria para los Explorers
        /// </summary>
        /// <param name="pos">La posicion actual del jugador</param>
        /// <param name="x">la posicion de la salidae en X</param>
        /// <param name="y">la posicion de la salida en Y</param>
        /// <returns></returns>
        public bool CheckWin1((int, int) pos, List<(int , int)> salidas)
        {
            foreach((int, int) item in salidas)
            {
                if(item.Item1 == pos.Item1 && item.Item2 == pos.Item2)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Esto chequea la condicion de victoria para los Hunters
        /// </summary>
        /// <param name="posH">La posicion de el Explorer</param>
        /// <param name="posV">La posicion del Hunter</param>
        /// <returns></returns>
        public bool CheckWin2(List<Heroe> H, List<Heroe> V)
        {
            foreach(var itm in H)
            {
                foreach(var item in V)
                {
                    if(itm.pos == item.pos)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void ShowMapG(string[,] m)//Para mostrar el mapa
        {
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    System.Console.Write(m[i, j] + "");
                }
                System.Console.WriteLine("");
            }
        }
        public bool EsValida(int[,] mapa,int x, int y)//Comrpueba si el movimiento es valido en el tablero
        {
            if(x < 0 || x >= mapa.GetLength(0))
            {
                return false;
            }
            if(y < 0 || y >= mapa.GetLength(1))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Movomiento es el metodo que me permite mover los heroes, invocar tokens, activar poderes, etc...
        /// </summary>
        /// <param name="move">Es la variable que me indica que tecla se toco</param>
        /// <param name="map">El mapa en el que se esta jugando la partida, lo uso para validar los movimientos</param>
        /// <param name="hero">El heroe que se mueve</param>
        public void Movimiento(ConsoleKeyInfo move, mapa map, Heroe hero)
        {
            switch(move.Key)
            {
                case ConsoleKey.DownArrow:
                    if(EsValida(map.MAP, hero.pos.Item1 + 1, hero.pos.Item2) && map.MAP[hero.pos.Item1 + 1, hero.pos.Item2] == 0)
                    {
                        huella pista = new huella(hero.pos, 2); 
                        hero.pos.Item1 += 1;
                        hero.rastro.Enqueue(pista);
                        this.contador++;
                    }    
                    break;
                case ConsoleKey.UpArrow:
                    if(EsValida(map.MAP, hero.pos.Item1 - 1, hero.pos.Item2) && map.MAP[hero.pos.Item1 - 1, hero.pos.Item2] == 0)
                    {
                        huella pista = new huella(hero.pos, 0);
                        hero.pos.Item1 -= 1;
                        hero.rastro.Enqueue(pista);
                        this.contador++;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if(EsValida(map.MAP, hero.pos.Item2 - 1, hero.pos.Item2) && map.MAP[hero.pos.Item1, hero.pos.Item2 - 1] == 0)
                    {
                        huella pista = new huella(hero.pos, 3);
                        hero.pos.Item2 -= 1;
                        hero.rastro.Enqueue(pista);
                        this.contador++;
                    }   
                    break;
                case ConsoleKey.RightArrow:
                    if(EsValida(map.MAP, hero.pos.Item2 + 1, hero.pos.Item2) && map.MAP[hero.pos.Item1, hero.pos.Item2 + 1] == 0)
                    {
                        huella pista = new huella(hero.pos, 1);
                        hero.pos.Item2 += 1;
                        hero.rastro.Enqueue(pista);
                        this.contador++;
                    }
                    break;
                case ConsoleKey.F:
                    hero.tokens[0].SUMMON = true;
                    huella[] ras = new huella[1];
                    hero.rastro.CopyTo(ras, hero.rastro.Count - 1);
                    hero.tokens[0].POS = ras[ras.Length - 1].pos;
                    break;
                // case ConsoleKey.S:
                //     System.Console.WriteLine("Que poder desea usar");
                //     int p = int.Parse(Console.ReadLine());
                //     System.Console.WriteLine("Seleccione su objetivo");
                    
                //     hero.tipo.poderes[p].damage
                //     break;
            }
        }
    /// <summary>
    /// Lo mismo que el metodo anterior pero para mover los tokens.
    /// </summary>
    /// <param name="move"></param>
    /// <param name="map"></param>
    /// <param name="tok"></param>
        public void MovTkns(ConsoleKeyInfo move, mapa map, token tok)
        {
            switch(move.Key)
            {
                case ConsoleKey.DownArrow:
                    if(EsValida(map.MAP, tok.POS.Item1 + 1, tok.POS.Item2) && map.MAP[tok.POS.Item1 + 1, tok.POS.Item2] == 0)
                    {
                        (int, int) temp = tok.POS;
                        temp.Item1 += 1;
                        tok.POS = temp;
                        this.ctkns++;
                    }    
                    break;
                case ConsoleKey.UpArrow:
                    if(EsValida(map.MAP, tok.POS.Item1 - 1, tok.POS.Item2) && map.MAP[tok.POS.Item1 - 1, tok.POS.Item2] == 0)
                    {
                        (int, int) temp = tok.POS;
                        temp.Item1 -= 1;
                        tok.POS = temp;
                        this.ctkns++;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if(EsValida(map.MAP, tok.POS.Item2 - 1, tok.POS.Item2) && map.MAP[tok.POS.Item1, tok.POS.Item2 - 1] == 0)
                    {
                        (int, int) temp = tok.POS;
                        temp.Item2 -= 1;
                        tok.POS = temp;
                        this.ctkns++;
                    }   
                    break;
                case ConsoleKey.RightArrow:
                    if(EsValida(map.MAP, tok.POS.Item2 + 1, tok.POS.Item2) && map.MAP[tok.POS.Item1, tok.POS.Item2 + 1] == 0)
                    {
                        (int, int) temp = tok.POS;
                        temp.Item2 += 1;
                        tok.POS = temp;
                        this.ctkns++;
                    }
                    break;
            }
        }
    }
    enum Elem { Fuego, Agua, Viento, Tierra }
}
