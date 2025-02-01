using System.Data;
using System.Drawing;
using System;

namespace MapaB
{
    [Serializable]class MapaBack
    {
        int size;
        public int SIZE{
            get{ return size; }
            //set{ size = value; }
        }
        Celda[,] map;
        public Celda[,] MAP{
            get{ return map; }
            set{ map = value; }
        }
        Random random = new Random();
        public MapaBack(int size)
        {
            this.size = size;
            this.map = new Celda[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    map[i, j] = new Celda();
                }
            }
        }
        public virtual bool Esvalid(int x, int y)
        {
            if(x > SIZE - 1|| x < 0 || y > SIZE - 1|| y < 0 || MAP[x, y].caminada == true)
            {
                return false;
            }
            return true;
        }
        public virtual bool Camino(int x, int y, int px, int py)
        {
            if(x > SIZE - 1|| x < 0 || y > SIZE - 1|| y < 0 || MAP[x, y].caminada == true)
            {
                return false;
            }
            else
            {
                MAP[x,y].caminada = true;
                int[] dx = {-1,1,0,0};
                int[] dy = {0,0,1,-1};
                for (int i = 0; i < 4; i++)
                {
                    int r = random.Next(i ,4);
                    int aux = dx[0];
                    dx[0] = dx[r];
                    dx[r] = aux;
                    aux = dy[0];
                    dy[0] = dy[r];
                    dy[r] = aux;

                }
                if(x > px){MAP[px, py].paredR = false;}
                if(x < px){MAP[x, y].paredR = false;}
                if(y > py){MAP[x, y].paredU = false;}
                if(y < py){MAP[px, py].paredU = false;}
                
                
                if(!Camino(x + dx[0], y + dy[0], x, y))
                {
                    
                }
                else if(!Camino(x + dx[1], y + dy[1], x, y))
                {
                
                }
                else if(!Camino(x + dx[2], y + dy[2], x, y))
                {

                }
                else if(!Camino(x + dx[3], y + dy[3], x, y))
                {
                    return false;
                }
                //return true;
            }
            return true;
        }
        public virtual void Show()
        {
            string[,] maps = new string[SIZE + SIZE + 1, SIZE + SIZE + 1];
            for (int h = 0; h < SIZE + SIZE - 1; h++)
            {
                for (int k = 0; k < SIZE + SIZE - 1; k++)
                {
                    maps[h,k] = "0";
                }
            }
            for (int i = 0, h = 1; i < SIZE; i++,h +=2)
            {
                for (int j = 0,k = 1; j < SIZE; j++,k +=2)
                {
                    if(!MAP[i, j].paredR)
                    {
                        maps[h,k] = " ";
                        maps[h + 1, k] = " ";
                    }
                    if(!MAP[i, j].paredU)
                    {
                        maps[h, k] =" ";
                        maps[h, k - 1] = " ";
                    }
                }
            }
            for (int i = 0; i < maps.GetLength(0); i++)
            {
                for (int j = 0; j < maps.GetLength(1); j++)
                {
                    Console.Write(maps[i,j] + " ");
                }
                System.Console.WriteLine("");
            }
        }
 
        static void Main()
        {
            MapaBack mapaBack = new MapaBack(5);
            mapaBack.Camino(0,0,0,0);
            mapaBack.Show();
        }
    }
    public class Celda
    {
        public bool paredR;
        public bool paredU;
        public bool caminada;
        public Celda()
        {
            paredR = true;
            paredU = true;
            caminada = false;
        }
    }
}
