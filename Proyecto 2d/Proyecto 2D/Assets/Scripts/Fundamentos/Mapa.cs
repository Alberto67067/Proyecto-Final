//Esta sera la version 1.01 de creacion de un mapa
using System;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using Unity.VisualScripting;

namespace Mapa
{
    /// <summary>
    /// La clase mapa me servira para definir los objetos de tipo mapa,  que utilizare en Game(clase en maze runners)
    /// size me dara su tamaño, map sera el mapa asi con el camino y Entrds sera la lista de posibles entradas. Salds, sera la lista de salidas
    /// </summary>
    [Serializable]public class mapa
    {
        int size;//Su tamaño
        int[,] map;//El mapa en si 0: Camino 1: Es obstaculo 2: Es trampa tipo 2
        List <(int, int)> Entrds;
        List<(int, int)> Salds;
        Random random;
        /// <summary>
        /// Este sera el constructor de los mapas posibles
        /// </summary>
        /// <param name="n">N me dara la dimension del mapa</param>
        /// <param name="Centrds">Este parametro me dara la cantidad de entradas a definir en el mapa</param>
        /// <param name="CSalds">Esta sera la cantidad de salidas posibles, por lo general sera solo uno pero bueno ya veremos</param> 
        public mapa(int n, int Centrds, int CSalds)//Constructor
        {
            random = new Random();
            size = n;
            map = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    map[i, j] = 0;
                }
            }
            Entrds = new List<(int, int)> ();
            Salds = new List<(int, int)> ();
            int cont = 0;
            while(cont < Centrds)
            {
                Entrds.Add(PosicionadorInix());
                cont++;
            }
            cont = 0;
            while(cont < CSalds)
            {
                Salds.Add(PosicionadorOut());
                cont++;
            }
        }
        public int SIZE{
            get { return size; }
           // set { size = value; }
        }
        public int[,] MAP{
            get { return map; }
            set { map = value; }
        }
        public List<(int, int)> ENTRDS{
            get { return Entrds; }
        }
        public List<(int, int)> SALDS{
            get { return Salds; }
        }

        // public virtual void ShowMap()//Metodo para mostrar el mapa
        // {
        //     string[,] mapa = new string[SIZE, SIZE];
        //     for (int i = 0; i < this.size; i++)
        //     {
        //         for(int j = 0;j < SIZE;j++)
        //         {
        //             if(MAP[i, j] == true)
        //             {
        //                 mapa[i,j] = "█";
        //                 Console.ForegroundColor = ConsoleColor.Blue;
        //                 System.Console.Write(mapa[i,j]);
        //             }
        //             else
        //             {
        //                 mapa[i,j] = "█";
        //                 Console.ForegroundColor = ConsoleColor.Red;
        //                 System.Console.Write(mapa[i,j]);
        //             }
        //             //System.Console.Write(mapa[i, j]);
        //         }
        //         System.Console.WriteLine("");
        //     }
        // }
        public virtual (int, int) PosicionadorInix()//Posicionador de Inicio
        {
            int value = random.Next(0, SIZE/2);//Me da su posicion 0,value o value, 0
            int filcol = random.Next(0, 2);// Me dice si sera en la fina o en la columna
            #region Pruebas iniciales 15/12
            // if(filcol == 0)
            // {
            //     MAP[0, value] = true;
            //     x = 0;
            //     y = value;
            // }
            // else
            // {
            //     MAP[value, 0] = true;
            //     x = value;
            //     y = 0;
            // }
            #endregion
            if (filcol == 0)
            {
                return (value, 0);
            }
            else
            {
                return (0, value);
            }
            // Me da la posicion Aleatoria de la entrada en una casilla de la priemra fila o columna
        }
        public virtual (int, int) PosicionadorOut()
        {
            int value = random.Next(SIZE/2, SIZE);
            int filcol = random.Next(0, 2);
            #region Pruebas iniciales 15/12
            // if(filcol == 0)
            // {
            //     MAP[SIZE - 1, value] = true;
            //     x = SIZE - 1;
            //     y = value;
            // }
            // else
            // {
            //     MAP[value, SIZE - 1] = true;
            //     x = value;
            //     y = SIZE - 1;
            // }
            #endregion 
            if(filcol == 0)
            {
                return (value, SIZE - 1);
            }
            else
            {
                return (SIZE - 1, value);
            }
            //Analogo al metodo anterior pero me en la ultima fila o columna
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
        public bool[,] ClonMatriz(bool[,] matriz)
        {
        bool[,] clon = new bool[matriz.GetLength(0), matriz.GetLength(1)];
        for (int i = 0; i < clon.GetLength(0); i++)
        {
            for (int j = 0; j < clon.GetLength(1); j++)
            {
                clon[i,j] = matriz[i,j];
            }
        }
        return clon;
        }
        public int[,] ClonMatriz(int[,] matriz)
        {
            int[,] clon = new int[matriz.GetLength(0), matriz.GetLength(1)];
            for (int i = 0; i < clon.GetLength(0); i++)
            {
                for (int j = 0; j < clon.GetLength(1); j++)
                {
                    clon[i,j] = matriz[i,j];
                }
            }
            return clon;
        }
        public virtual string[,] ToStringM(bool[,] matriz)
        {
            string [,] clon = new string[matriz.GetLength(0), matriz.GetLength(1)];
            for (int i = 0; i < clon.GetLength(0); i++)
            {
                for (int j = 0; j < clon.GetLength(1); j++)
                {
                    if(matriz[i,j] == true)
                    {
                        clon[i,j] = " ";
                    }
                    else
                    {
                        clon[i,j] = "█";
                    }
                }   
            }
            return clon;
        }
        /// <summary>
        /// Este metodo me hara el camino, es un Algoritmo de Lee de toda la vida.
        /// </summary>
        /// <param name="mapa"> el objeto de tipo "mapa" a ser evaluado</param>
        /// <param name="x"> Coordena x de la entrada</param>
        /// <param name="y"> Coordena y de la entrada</param>
        /// <param name="sx">Coordena x de la salida</param>
        /// <param name="sy">Coordena y de la salida</param>
        /// <returns>Devolvera "false" en caso de que no exista un camino posible, y "true" en caso contrario</returns>
        public bool Camino(int[,] mapa, (int, int) entr)
        {
            // mapa.map[x, y] = false;
            // mapa.map[sx, sy] = false;
            int[,] clon = ClonMatriz(mapa);
            int[] dx = {1,-1,0,0};
            int[] dy = {0,0,1,-1};
            int[,] clonint = new int[clon.GetLength(0), clon.GetLength(1)];
            for (int i = 0; i < clon.GetLength(0); i++)
            {
                for (int j = 0; j < clon.GetLength(1); j++)
                {
                    if(clon[i, j] == 1)
                    {
                        clonint[i, j] = -1;
                    }
                    else
                    {
                        clonint[i, j] = 0;
                    }
                }
            }
            clonint[entr.Item1, entr.Item2] = 1;
            bool cambio;
            do
            {
                cambio = false;
                for(int i = 0; i < clonint.GetLength(0); i++)
                {
                    for(int j = 0; j < clonint.GetLength(1); j++)
                    {
                        if(clonint[i, j] == 0)
                        {
                            continue;
                        }
                        if(clon[i, j] == 1)
                        {
                            continue;
                        }
                        for (int k = 0; k < dx.Length; k++)
                        {
                            int px = i + dx[k];
                            int py = j + dy[k];
                            if(PosicionValida(mapa, px, py, true) && clon[px, py] != 1 && clonint[px, py] != -1)
                            {
                                if (clonint[px, py] == 0)
                                {
                                    clonint[px, py] = 1 + clonint[i, j];
                                    cambio = true;
                                    continue;
                                }
                                if (clonint[px, py] > (1 + clonint[i, j]) && clonint[px, py] != 0)
                                {
                                    clonint[px, py] = 1 + clonint[i, j];
                                    cambio = true;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }while(cambio);
            for (int i = 0; i < clon.GetLength(0); i++)
            {
                for (int j = 0; j < clon.GetLength(1); j++)
                {
                    if (clonint[i, j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Este metodo esta diseñado para mover los pares ordenados hacia la izquiera y asi ir tomando los valores entre cero
        /// y el indice k, de forma tal que no se me repitan y acorte tiempo de los que ya se que no seran solucion.
        /// </summary>
        /// <param name="n">El arreglo de todos los pares ordenados de la matriz</param>
        /// <param name="k">El indice</param>
        /// <returns></returns>
        private (int, int) Pos_AleatI((int, int)[] n, int k)
        {
            int valor = random.Next(0,n.Length - k - 1);
            return n[valor];
        }
        /// <summary>
        /// CantObs es un metodo que hice que me devolvera la cantidad de obstaculos que tendra el mapa
        /// Lo balancea de forma tal que me devuelva un numero de obstaculos razonable que no sean muy pocos ni muchos
        /// la formula que use esta escrita en el metodo
        /// </summary>
        /// <param name="mapa">Este prarametro sera para introducir el mapa</param>
        /// <param name="ent">una de las entradas del mapa</param>
        /// <param name="sal">Una de las salidas, aunque por lo general mis mapas no tendran mas de una salida</param>
        /// <returns></returns>
        private int CantObs(int [,] mapa, (int, int) ent, (int, int) sal)
        {
            int dis = Math.Abs(ent.Item1 - sal.Item1) + Math.Abs(ent.Item2 - sal.Item2);
            int cantO = random.Next((mapa.GetLength(0) * (mapa.GetLength(1) - (int)Math.Sqrt(mapa.GetLength(0))))/2, (mapa.Length - dis)/2);
            return cantO;
        }
        /// <summary>
        /// Este metodo ira moviendo los pares ya usados hacia el extremo derecho del mapa
        /// </summary>
        /// <param name="a"></param>
        /// <param name="pos"></param>
        /// <param name="h"></param>
        private void MoverBack((int, int)[] a, (int, int) pos, int h)
        {
            (int, int) aux;
            for (int i = 0; i < a.Length - h; i++)
            {
                if(a[i] == pos)
                {
                    aux = a[a.Length - 1 - h];
                    a[a.Length - 1 - h] = a[i];
                    a[i] = aux;
                }
            }
        }
        /// <summary>
        /// Este metodo lo hice rapidin para devolver Elementos de las listas que utilizare en paredes
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public virtual (int, int) DevolverR(List<(int, int)> lista)
        { 
            (int, int) value = lista[random.Next(0, lista.Count)];
            return value;
        }
        public virtual (int, int)[] ArrPos(int size)
        {
            int h = 0;
            (int, int)[] posiciones = new (int, int)[size * size];
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++, h++)
                {
                    posiciones[h] = (i, j);
                }
            }
            return posiciones;
        }
        public virtual void Paredes(List<(int, int)> Entradas, List<(int, int)> Salidas)
        {
            int cantA = Entradas.Count + Salidas.Count;
            int cantO = CantObs(MAP,DevolverR(Entradas), DevolverR(Salidas)) + cantA;
            int h = 0;
            (int, int)[] posiciones = ArrPos(SIZE);
            foreach((int,int) item in Entradas)//Mover todas las entradas hacia atras para que no sean colocadas como obstaculo
            {
                MoverBack(posiciones, item, h);
                h++;
            }
            foreach((int,int) item in Salidas)//Mover todas las salidas hacia atras para que no sean colocadas como obstaculos
            {
                MoverBack(posiciones, item, h);
                h++;
            }
            while(cantA <= cantO)
            {
                (int,int) pos = Pos_AleatI(posiciones, cantA);
                MAP[pos.Item1, pos.Item2] = 1;
                bool compr = Camino(MAP, Entradas[0]);
                MoverBack(posiciones, pos, cantA);
                if(compr)
                {
                    cantA++;
                    continue;
                }
                else
                {
                    MAP[pos.Item1, pos.Item2] = 0;
                    continue;
                }
            }
            int r = random.Next(SIZE, SIZE + SIZE/10);
            h = 0;
            posiciones = new (int,int)[(SIZE * SIZE) - cantA + Entradas.Count + Salidas.Count];
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if(MAP[i, j] == 0)
                    {
                        posiciones[h] = (i, j);
                        h++;                    
                    }
                }
            }
            h = 0;
            foreach((int,int) item in Entradas)//Mover todas las entradas hacia atras para que no sean colocadas como obstaculo
            {
                MoverBack(posiciones, item, h);
                h++;
            }
            foreach((int,int) item in Salidas)//Mover todas las salidas hacia atras para que no sean colocadas como obstaculos
            {
                MoverBack(posiciones, item, h);
                h++;
            }
            for (int i = 0; i < r/2; i++, h++)
            {
                (int, int) pos = Pos_AleatI(posiciones, h);
                MAP[pos.Item1, pos.Item2] = 2;
            }
            for (int i = 0; i < r/2; i++, h++)
            {
                (int, int) pos = Pos_AleatI(posiciones, h);
                MAP[pos.Item1, pos.Item2] = 3;
            }
        } 
        // static void Main()
        // {
        //     int inicio = Environment.TickCount;
        //     mapa a = new mapa(100, 2, 2);
        //     // System.Console.WriteLine("Dimensiones: " + a.map.Length + " " + a.size);
        //     //int sx;// Coordenada X de salida
        //     //int sy;// Coordenada Y de salida
        //     //int x;//  Coordenada X de inicio 
        //     //int y;//  Coordenada y de inicio
        //     //a.PosicionadorInix(out x, out y);
        //     //a.PosicionadorOut(out sx, out sy);
        //     // System.Console.WriteLine("Coordenadas de la entrada en: "+ x +", "+ y);
        //     // Camino(a, b);
        //     // ShowMap(a);
        //     // System.Console.WriteLine("Coordenadas de la salida en: "+ sx +", "+ sy);
        //     // System.Console.WriteLine(Camino(a, x, y, sx, sy));
        //     // ShowMap(a);
        //     a.Paredes(a.Entrds, a.Salds);
        //     a.ShowMap();
        //     // System.Console.WriteLine("Entrada en: "+ x +", "+ y);
        //     // System.Console.WriteLine("Salida en: "+ sx +", "+ sy);
        //     System.Console.WriteLine(Environment.TickCount - inicio);
        // }
    }
}