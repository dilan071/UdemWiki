using System;


public class Nodo
{
    public char[,] Tablero { get; set; }
    public List<Nodo> Hijos { get; set; }
    public int Valor { get; set; }

    public Nodo(char[,] tablero)
    {
        Tablero = tablero;
        Hijos = new List<Nodo>();
    }
}