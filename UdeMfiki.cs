using System;
using System.Collections.Generic;
using System.Linq;

class UdeMfiki
{
    const int TAMANO = 4;
    char[,] tablero = new char[TAMANO, TAMANO];

    

    public UdeMfiki()
    {
        for (int i = 0; i < TAMANO; i++)
            for (int j = 0; j < TAMANO; j++)
                tablero[i, j] = ' ';
    }

    public void MostrarTablero()
    {
        for (int i = 0; i < TAMANO; i++)
        {
            for (int j = 0; j < TAMANO; j++)
            {
                Console.Write(tablero[i, j]);
                if (j != TAMANO - 1) Console.Write("|");
            }
            Console.WriteLine();
            if (i != TAMANO - 1) Console.WriteLine("---------");
        }
    }

    public bool ComprobarVictoria(char jugador)
    {
        for (int i = 0; i < TAMANO - 1; i++)
        {
            for (int j = 0; j < TAMANO - 1; j++)
            {
                if (tablero[i, j] == jugador && tablero[i + 1, j] == jugador && tablero[i + 1, j + 1] == jugador)
                    return true;

                if (tablero[i, j + 1] == jugador && tablero[i + 1, j] == jugador && tablero[i + 1, j + 1] == jugador)
                    return true;

                if (tablero[i, j] == jugador && tablero[i, j + 1] == jugador && tablero[i + 1, j] == jugador)
                    return true;

                if (tablero[i + 1, j] == jugador && tablero[i, j] == jugador && tablero[i, j + 1] == jugador)
                    return true;
            }
        }
        return false;
    }

    public Nodo ConstruirArbol(char[,] estadoTablero, char jugador)
    {
        Nodo nodoActual = new Nodo(estadoTablero);

        char oponente = (jugador == 'X') ? 'O' : 'X';
        if (ComprobarVictoria(jugador))
        {
            nodoActual.Valor = 10;
            return nodoActual;
        }
        if (ComprobarVictoria(oponente))
        {
            nodoActual.Valor = -10;
            return nodoActual;
        }

        for (int i = 0; i < TAMANO; i++)
            for (int j = 0; j < TAMANO; j++)
                if (estadoTablero[i, j] == ' ')
                {
                    char[,] nuevoEstado = (char[,])estadoTablero.Clone();
                    nuevoEstado[i, j] = jugador;
                    nodoActual.Hijos.Add(ConstruirArbol(nuevoEstado, oponente));
                }

        return nodoActual;
    }

    public (int, int) MejorMovimiento(char jugador)
    {
        Nodo nodoRaiz = ConstruirArbol(tablero, jugador);
        int mejorValor = -1000;
        int mejorFila = -1;
        int mejorColumna = -1;

        foreach (var hijo in nodoRaiz.Hijos)
        {
            int valorMovimiento = MiniMax(hijo, 0, false, jugador, -1000, 1000);
            if (valorMovimiento > mejorValor)
            {
                mejorFila = hijo.Tablero[0, 0];
                mejorColumna = hijo.Tablero[0, 1];
                mejorValor = valorMovimiento;
            }
        }

        return (mejorFila, mejorColumna);
    }

    int MiniMax(Nodo nodo, int profundidad, bool esJugadorMaximizador, char jugador, int alpha, int beta)
    {
        char oponente = (jugador == 'X') ? 'O' : 'X';

        if (nodo.Valor == 10 || nodo.Valor == -10)
            return nodo.Valor;

        if (!nodo.Hijos.Any()) return 0;

        if (esJugadorMaximizador)
        {
            int mejor = -1000;
            foreach (var hijo in nodo.Hijos)
            {
                mejor = Math.Max(mejor, MiniMax(hijo, profundidad + 1, !esJugadorMaximizador, jugador, alpha, beta));
                alpha = Math.Max(alpha, mejor);
                if (beta <= alpha)
                    break;
            }
            return mejor;
        }
        else
        {
            int mejor = 1000;
            foreach (var hijo in nodo.Hijos)
            {
                mejor = Math.Min(mejor, MiniMax(hijo, profundidad + 1, !esJugadorMaximizador, jugador, alpha, beta));
                beta = Math.Min(beta, mejor);
                if (beta <= alpha)
                    break;
            }
            return mejor;
        }
    }


    public void Jugar()
    {
        while (true)
        {
            MostrarTablero();
            Console.WriteLine("¡Tu turno! (formato: fila columna, por ejemplo: 1 2)");
            string[] entrada = Console.ReadLine().Split();
            int fila = int.Parse(entrada[0]);
            int columna = int.Parse(entrada[1]);

            if (tablero[fila, columna] == ' ')
            {
                tablero[fila, columna] = 'X';

                if (ComprobarVictoria('X'))
                {
                    MostrarTablero();
                    Console.WriteLine("¡Has ganado!");
                    break;
                }
            }
            else
            {
                Console.WriteLine("Posición ocupada. Intenta de nuevo.");
                continue;
            }

            var movimientoMaquina = MejorMovimiento('O');
            tablero[movimientoMaquina.Item1, movimientoMaquina.Item2] = 'O';
            if (ComprobarVictoria('O'))
            {
                MostrarTablero();
                Console.WriteLine("¡La máquina ha ganado!");
                break;
            }
        }
    }
}


