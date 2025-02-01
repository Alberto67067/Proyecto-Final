using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using Mapa;
using Unity.VisualScripting;
using JetBrains.Annotations;
namespace Heroes
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable] 
    public class Heroe : Objetivo// De aqui en adelante en donde sea que este escrito "Personaje" me refiero a Heroe, es que el nombre este lo cambie despues de haberlo hecho todo
    //Perdon, a quien este leyendo mi codigo.
    {
        public string nombre{get;} 
        //public (int, int) pos;
        public (int, int) pos0;
        //public mapa map;
        public Queue<huella> rastro {get; set;}
        int tamanRastro;
        public int TAMANRASTRO{
            get { return tamanRastro;}
            set { tamanRastro = value;}
        }
        public int Bando;// 0 sera el bando Hunter y 1 sera el bando Explorer
        public Tipo tipo;
        //public int velocidad{get; set;}
        //public int velocidad0;
        public int vision{get; set;}//Me dara un radio de la vision que tiene el personaje, esto de seguro sera de las partes mas dificiles de programar
        public int vision0;
        public int energia{get; set;}
        public int energiaMax{get; set;}
        //public int vidaMax{get; set;}
        //public int vida{get; set;}
        //public bool summon;
        int conteoDead;
        public token[] tokens{get;}//Me dara los tokens que cada heroe podra invocar
        //public int invocaciones{get;}// Me dara la cantidad de invocaiones de tokens cada X turnos que podra hacer el Heroe
        /// <summary>
        /// Esto crea un objeto que rellena todos los campos.
        /// </summary>
        /// <param name="nombre">Bueno como indica esto, sera el nombre del Personaje</param>
        /// <param name="map">El mapa para el que esta ficha sera colocado</param>
        /// <param name="tamañoderastro">Esto me dara el tamano del rastro definido previamente pra cada jugador</param>
        /// <param name="Bando">El Bando sera {0: para Hunters, 1: Para Explorer}</param>
        /// <param name="tipo">Los tipos los definire despues, y estos me seran utlies dependiendo del personaje</param>
        /// <param name="velocidad">La cantidad k de casillas que los jugadores podran moverse por turno</param>
        /// <param name="vision">El radio de casillas que podra ver el jugador en si</param>
        /// <param name="energiaMax">La energiaMax que tendra el jugador</param>
        /// <param name="vidaMax">La vida Max que tendra el jugador</param>
        /// <param name="tokens">Los tokens que podra invocar el Heroe</param>
        public Heroe(string nombre, mapa map, int tamanRastro, int Bando, Tipo tipo, int velocidad, int vision, int energiaMax, int vidaMax, token[] tokens)
        {
            this.nombre = nombre;
            this.Bando = Bando;
            this.map = map;
            pos = Posicionador(map, Bando);//Tendra que ser igual a la posicion inicial
            pos0 = pos;
            rastro = new Queue<huella>();
            this.tamanRastro = tamanRastro;
            this.tipo = tipo;
            this.velocidad = velocidad;
            this.velocidad0 = velocidad;
            this.vision = vision;
            this.vision0 = vision;
            this.energia = energiaMax;
            this.energiaMax = energiaMax;
            this.vidaMax = vidaMax;
            this.vida = vidaMax;
            this.tokens = tokens;
            this.summon = true;
            conteoDead = 0;
        }
        public virtual (int, int) Posicionador(mapa map, int band)
        {
            (int, int) posi = (0, 0);
            Random random = new Random();
            if(band == 0)
            {
                posi = map.PosicionadorOut();
                (int, int) temp = (posi.Item1 - (int)random.Next(0, map.SIZE/2), posi.Item2 - (int)random.Next(0, map.SIZE/2));
                if(map.MAP[temp.Item1, temp.Item2] == 0)
                {
                    posi = (temp.Item1, temp.Item2);//Tengo que arreglar esto
                }
                else
                {
                    Posicionador(map, band);
                }
            }
            if(band == 1)
            {
                posi = map.ENTRDS[random.Next(0, map.ENTRDS.Count)];
            }
            return posi;
        }
        public void IsDead()
        {
            if(!summon)
            {
                vida = vidaMax;
                energia = energiaMax;
                rastro.Clear();
                foreach (var item in tokens)
                {
                    item.SUMMON = false;
                }
                summon = true;
                velocidad = velocidad0;
                vision = vision0;
            }
        }
        /// <summary>
        /// Esto tengo que terminarlo
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public bool EstaEnRango(Objetivo target, Objetivo lanz, int rango,mapa map)
        {
            int[] dx = { 1, -1, 0, 0, };
            int[] dy = { 0, 0, -1, 1 };
            var x = new Queue<Tuple<int, int>>();
            var y = new HashSet<Tuple<int, int>>();
            x.Enqueue(new(lanz.pos.Item1, lanz.pos.Item2));
            y.Add(new(lanz.pos.Item1, lanz.pos.Item2));
            int costo = 1;
            while (costo <= rango)
            {
                int nivelactual = x.Count;
                for(int i = 0; i < nivelactual; i++)
                {
                    var item = x.Dequeue();
                    for (int k = 0; k < 4; k++)
                    {
                        if (map.PosicionValida(map.MAP, item.Item1 + dx[k], item.Item2 + dy[k], true) && map.MAP[item.Item1 + dx[k], item.Item2 + dy[k]] != 1 && !y.Contains(new(item.Item1 + dx[k], item.Item2 + dy[k])))
                        {
                            y.Add(new Tuple<int, int>(item.Item1 + dx[k], item.Item2 + dy[k]));
                            x.Enqueue(new Tuple<int, int>(item.Item1 + dx[k], item.Item2 + dy[k]));
                        }
                        if ((item.Item1 + dx[k], item.Item2 + dy[k]) == target.pos)
                        {
                            return true;
                        }
                    }
                }
                costo++;
            }
            return false;
        }
        public bool EstaEnRango((int, int) target, (int, int) lanz, int rango, mapa map)
        {
            int[] dx = { 1, -1, 0, 0, };
            int[] dy = { 0, 0, -1, 1 };
            var x = new Queue<Tuple<int, int>>();
            x.Enqueue(new(lanz.Item1, lanz.Item2));
            int costo = 1;
            while (costo <= rango)
            {
                var y = x.ToArray();
                foreach (var item in y)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (map.PosicionValida(map.MAP, item.Item1 + dx[k], item.Item2 + dy[k], true) && map.MAP[item.Item1 + dx[k], item.Item2 + dy[k]] != 1 && !x.Contains(new(item.Item1 + dx[k], item.Item2 + dy[k])))
                        {
                            x.Enqueue(new Tuple<int, int>(item.Item1 + dx[k], item.Item2 + dy[k]));
                        }
                        if ((item.Item1 + dx[k], item.Item2 + dy[k]) == target)
                        {
                            return true;
                        }
                    }
                    x.Dequeue();
                }
                costo++;
            }
            return false;
        }
        static void Main()
        {}
    }
        [Serializable]public class Tipo
        {
            //Enum que me dara su elemento
            string nombre;
            public List<Poder> poderes;
            int elemento;
            int afinidad;
            int debilidad;
            int BiomaAfin;//Esto lo definire en mapa mas tarde, seran los biomas que ubicare en el mapa.
            public Tipo(string nombre, int elemento, int afinidad, int debilidad, int BiomaAfin, params Poder[] poderes)
            {
                this.nombre = nombre;
                this.elemento = elemento;
                this.afinidad = afinidad;
                this.debilidad = debilidad;
                this.BiomaAfin = BiomaAfin;
                this.poderes = poderes.ToList();
            }
            /*      El poder de cada personaje sera unico, ya me ocupare en el Main del juego de crearlos
                hsta ahora esta sera la plantilla que usare:
                -nombre, me parece que no necesito explicarlo
                -poder, ya con los poderes creados, y los personajes me ocupare de asegnarlos, los poderes
                por supuesto responderan a un elemento, puede que me de por crear un personaje con mas de 
                un poder.
                -elemento, asignare a cada Heroe un elemnto, esto tendra relacion conceptual con su afinidad 
                al bioma, su afinidad, y su debilidad.
                    los elementos hasta ahora 5/12/24
                    -0: Fuego
                    -1: Agua
                    -2: Hielo
                    -3: Viento
                    -4: Tierra
                    -5: Acero
                    -6: Luz
                    -7: Oscuridad
                afinidad, con lo explicado anteriormente creo que evidente lo que esto significara, ya pondre
                una imagen, que ademas hare en Paint seguramente explicando la relacion, pero sera parecida a 
                Pokemon, sujeto a algunas libertades que me tomare como autor.
                debilidad, || Iden ||
                BiomaAfin, mas de lo mismo.
            */
            [Serializable]public class Poder
            {
                public string nombre;
                public int recarga;
                public int energiaNec;
                public int rango;
                public int turnsactivo;
                public bool activo;
                // public Poder(string nombre, int duracion, int rango,int recarga, int energiaNec)
                // {
                //     this.nombre = nombre;
                //     this.recarga = recarga;
                //     this.energiaNec = energiaNec;
                //     this.duracion = duracion;
                //     this.rango = rango;
                //     tactivo = 0;
                //     activo = false;
                // }
            }
            internal interface ICurador
            {
                public void Curar(Objetivo Target);
            }
            internal interface IAttack
            {
                public void Atacar(Objetivo Target);
            }
            internal interface IMovedor
            {
                public void Mover(Objetivo Lanz);
            }
            internal interface IDestruc
            {
                public void Destruir();
            }
            internal interface IColocador
            {
                public void Colocar(Heroe Lanz);
            }
            public class AtacarPoder : Poder, IAttack
            {
                public int damage;
                public AtacarPoder(string nombre, int rango,int recarga, int energiaNec, int damage)
                {
                    this.nombre = nombre;
                    this.recarga = recarga;
                    this.energiaNec = energiaNec;
                    this.rango = rango;
                    this.damage = damage;
                    this.turnsactivo = 0;
                    this.activo = false;
                }
                public void Atacar(Objetivo Target)
                {
                    Target.vida -= this.damage;
                    if (Target.vida <= Target.vidaMax)
                    {
                        Target.summon = false;
                    }
                }
            
            }
            [Serializable]public class CurarPoder : Poder, ICurador
            {
                public int health;
                public int duracion;
                public CurarPoder(string nombre, int duracion, int rango,int recarga, int energiaNec, int health)
                {
                    this.nombre = nombre;
                    this.recarga = recarga;
                    this.duracion = duracion;
                    this.energiaNec = energiaNec;
                    this.rango = rango;
                    this.health = health;
                    this.turnsactivo = 0;
                    this.activo = false;
                }
                public void Curar(Objetivo Target)
                {
                    Target.vida += this.health;
                    if (Target.vida > Target.vidaMax)
                    {
                        Target.vida = Target.vidaMax;
                    }
                }
            }
            [Serializable]public class MoverPoder : Poder, IMovedor
            {
                int movs;
                public MoverPoder(string nombre, int movs, int rango,int recarga, int energiaNec)
                {
                    this.nombre = nombre;
                    this.recarga = recarga;
                    this.energiaNec = energiaNec;
                    this.rango = rango;
                    this.turnsactivo = 0;
                    this.movs = movs;
                    activo = false;
                }
                public void Mover(Objetivo Lanz)
                {  
                    if(activo)
                    {
                        Lanz.velocidad += movs;
                    }
                    else
                    {
                        Lanz.velocidad += movs;
                    }
                }
            }
            [Serializable]public class ColocarPoder : Poder, IColocador
            {
                int cantObs;
                public ColocarPoder(string nombre, int cantObs, int rango,int recarga, int energiaNec)
                {
                    this.nombre = nombre;
                    this.recarga = recarga;
                    this.energiaNec = energiaNec;
                    this.rango = rango;
                    this.turnsactivo = 0;
                    this.cantObs = cantObs;
                    activo = false;
                }
                public void Colocar(Heroe Lanz)
                {
                    int ObsCol = 0;
                    while(ObsCol <= cantObs)
                    {
                        //Terminar aqui
                        ObsCol++;
                    }
                }
            }
            [Serializable]public class DestruirPoder : Poder, IDestruc
        {
            int obsDest;
            public DestruirPoder(string nombre, int obsDest, int rango,int recarga, int energiaNec)
            {
                this.nombre = nombre;
                this.recarga = recarga;
                this.energiaNec = energiaNec;
                this.rango = rango;
                this.turnsactivo = 0;
                this.obsDest = obsDest;
                activo = false;
            }
            public void Destruir()
            {
                int CantDest = 0;
                while(CantDest <= obsDest)
                {
                    CantDest++;
                }
            }
        }
    }
        [Serializable]public class token : Objetivo
        {
            string nombre;
            public string NOMBRE{
                get{ return nombre; }
            }
            //(int, int) pos;
            public (int, int) POS{
                get { return pos; }
                set { pos = value;}
            }
            int damage1;
            public int Damage1{
                get { return damage1; }
                set { damage1 = value;}
            }
            int damage;
            public int Damage{
                get { return damage; }
                set { damage = value;}
            }
            //int vidaMax;
            public int VIDAMAX{
                get {return vidaMax;}
            }
            //int vida;// Mas tarde tendre que hacer que vida y damage sean mutables desde fuera de este Namespace
            public int VIDA{
                get {return vida;}
                set {vida = value;}
            }
            //int velocidad;
            public int VELOCIDAD{
                get {return velocidad;}
                set { velocidad = value;}
            }
            int duracion;//La cantidad de turnos que podra estar el token invocado a menos que lo maten antes
            public int DURACION{
                get {return duracion;}
                set {duracion = value;}
            }
            //bool summon;
            public bool SUMMON{
                get {return summon;}
                set {summon = value;}
            }
            int tsummon;
            public int TSUMON{
                get {return tsummon;}
                set {tsummon = value;}
            }
            public int Bando;
            public token(string nombre, int bando, int damage, int vidaMax, int vida, int velocidad, int duracion)
            {
                this.nombre = nombre;
                this.damage1 = damage;
                this.damage = damage;
                this.vidaMax = vidaMax;
                this.vida = vida;
                this.velocidad = velocidad;
                this.duracion = duracion;
                summon = false;
                tsummon = 0;
                this.Bando = bando;
            }
            public bool IsDead()
            {
                if ((TSUMON >= DURACION || VIDA <= 0) && SUMMON)
                {    
                    Damage = Damage1;
                    VIDA = VIDAMAX;
                    TSUMON = 0;
                    SUMMON = false;
                    return true;
                }
                return false;
            }
            public bool EstaEnRango(Objetivo lanz, Objetivo target)
            {
                if((lanz.pos.Item1 + 1, lanz.pos.Item2) == target.pos 
                || (lanz.pos.Item1 - 1, lanz.pos.Item2) == target.pos
                || (lanz.pos.Item1, lanz.pos.Item2 + 1) == target.pos
                || (lanz.pos.Item1, lanz.pos.Item2 - 1) == target.pos )
                {
                    return true;
                }
                return false;
            }
        }
        [Serializable]public class huella
        {
            public (int, int) pos;//Esto me dara la posicion de la huella
            public int direccion;//Esto la direccion del movimiento, no se si sera mas optimo esto o ver en donde esta el proximo mov
            public huella((int, int) pos, int direccion)
            {
                this.pos = pos;
                this.direccion = direccion;
                //Tengo que usar un tipo Enum aqui, ls direcciones por ahora seran
                // 0 = Norte
                // 1 = Este
                // 2 = Sur
                // 3 = Oeste
            }
        }
}
public abstract class Objetivo
{
    public (int, int) pos;
    public int vidaMax;
    public int vida;
    public bool summon;
    public int velocidad;
    public int velocidad0;
    public mapa map;
    public mapa MAP{
        get { return map; }
        set { map = value; }
    }
}
// public class MiQueue
// {
//     (int, int)[] arreglo {get;set;}
//     int size {get;set;}
//     public MiQueue(int size)
//     {
//         this.size = size;
//         arreglo = new (int, int)[size];
//     }
//     public void Enqueue((int, int) item)
//     {
        
//     }
// }
public enum Direccion {Norte, Izqu, Sur, Derecha};
public enum Elemento {Fuego, Tierra, Viento, Agua, Luz, Oscuridad};
