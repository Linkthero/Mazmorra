using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    // Declaracion de clase interna
    public class Cell
    {
        public bool visited = false; // Indica si la celda ha sido visitada
        public bool[] status = new bool[4]; // Estado de las puertas: 0-Top, 1-Down, 2-Left, 3-Right
    }
    [SerializeField] private Vector2Int size; // Tamaño del mapa (ancho, alto)
    [SerializeField] public int initPosition = 0; // Posicion inicial en el eje X
    [SerializeField] private GameObject room; // Prefab de la habitacion
    [SerializeField] private Vector2 roomSize; // Tamaño de cada habitacion
    [SerializeField] public int nRooms; // Numero de habitaciones a generar

    [SerializeField] private TMP_InputField nR;
    [SerializeField] private TMP_InputField habIni;

    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject player;

    List<Cell> board; // Lista de celdas del mapa

    private void Start()
    {
        panel.SetActive(true);
        //MazeGenerator(); // Generar el mapa al iniciar
    }
    public void Generate()
    {
        // Elimina las habitaciones generadas anteriormente
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log(nR.text);
        Debug.Log(habIni.text);
        SetHabInicial(int.Parse(habIni.text));
        SetNHabitaciones(int.Parse(nR.text));

        panel.SetActive(false);
        // Genera el nuevo laberinto
        MazeGenerator();
    }
    private void MazeGenerator()
    {
        board = new List<Cell>(); // Inicializar la lista de celdas
        for (int i = 0; i < size.x; i++) // Crear la matriz de posibles celdas del mapa
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell()); // Agregar una nueva celda a la lista
            }
        }

        int currentCell = initPosition;
        Stack<int> path = new Stack<int>(); // Pila para el camino recorrido

        int num = 1; // Contador de habitaciones generadas

        while (num < nRooms)
        {
            num++;
            board[currentCell].visited = true; // Marcar la celda actual como visitada

            List<int> neighbors = CheckNeighbors(currentCell); // Obtener los vecinos no visitados

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break; // Si no hay vecinos y el camino esta vacio, terminar
                }
                else
                {
                    currentCell = path.Pop(); // Retroceder al ultimo punto en el camino
                }
            }
            else // Hay vecinos no visitados
            {
                path.Push(currentCell); // Agregar la celda actual al camino
                int newCell = neighbors[UnityEngine.Random.Range(0, neighbors.Count)]; // Seleccionar un vecino aleatorio

                if (newCell > currentCell) // Nueva celda es derecha o arriba
                {
                    if (newCell - 1 == currentCell) // Derecha
                    {
                        board[currentCell].status[3] = true; // Abrir puerta derecha
                        board[newCell].status[2] = true; // Abrir puerta izquierda
                    }
                    else // Arriba
                    {
                        board[currentCell].status[0] = true; // Abrir puerta arriba
                        board[newCell].status[1] = true; // Abrir puerta abajo
                    }
                }
                else // Izquierda o abajo
                {
                    if (newCell + 1 == currentCell) // Izquierda
                    {
                        board[currentCell].status[2] = true; // Abrir puerta izquierda
                        board[newCell].status[3] = true; // Abrir puerta derecha
                    }
                    else // Abajo
                    {
                        board[currentCell].status[1] = true; // Abrir puerta abajo
                        board[newCell].status[0] = true; // Abrir puerta arriba
                    }
                }
                currentCell = newCell; // Mover a la nueva celda
            }
        }
        DungeonGenerator(); // Generar el dungeon basado en el mapa

        
    }

    private void DungeonGenerator()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                // Instanciar la habitacion en la posicion correspondiente
                var newRoom = Instantiate(room, new Vector3(i * roomSize.x, 0, j * roomSize.y),
                    Quaternion.identity, transform).GetComponent<Room>();
                newRoom.UpdateRoom(board[i + j * size.x].status); // Actualizar las puertas de la habitacion
            }
        }

        try
        {
            Instantiate(player, transform.GetChild(initPosition - 1).GetComponent<Room>().transform);
        } catch(Exception e){
            Debug.Log("ERROR" + e);
        }
        
        Debug.Log("Instancianciado player");
    }

    private List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>(); // Lista de vecinos no visitados

        if (cell - size.x > 0 && !board[cell - size.x].visited) // Comprobar vecino inferior
        {
            neighbors.Add(cell - size.x);
        }
        if (cell + size.x < board.Count && !board[cell + size.x].visited) // Comprobar vecino superior
        {
            neighbors.Add(cell + size.x);
        }
        if (cell % size.x != 0 && !board[cell - 1].visited) // Comprobar vecino izquierdo
        {
            neighbors.Add(cell - 1);
        }
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited) // Comprobar vecino derecho
        {
            neighbors.Add(cell + 1);
        }
        return neighbors; // Devolver la lista de vecinos no visitados
    }

    public void SetNHabitaciones(int n)
    {
        nRooms = n;
    }

    public void SetHabInicial(int i)
    {
        initPosition = i;
    }
}
