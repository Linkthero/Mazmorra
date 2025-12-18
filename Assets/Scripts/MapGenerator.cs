using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //Declaracion de clase interna
    public class Cell
    {
        public bool visited = false; //Indica si la celda ha sido visitada
        public bool[] status = new bool[4]; //0 - top; 1- down ; 2 - left; 3 - right
    }

    [SerializeField] private Vector2Int size;
    [SerializeField] private GameObject room;
    [SerializeField] private int initPosition = 0;
    [SerializeField] private Vector2 roomSize;
    [SerializeField] private int nRooms;

    List<Cell> board;

    private void Start()
    {
        MazeGenerator();
    }

    private void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = initPosition;

        Stack<int> path = new Stack<int>(); //Pila 

        int num = 1;
        while (num < nRooms)
        {
            board[currentCell].visited = true;

            //Comprobar vecinos
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            { //no hay vecinos
                if (path.Count == 0)
                {
                    break; //no hay mas celdas en el camino
                }
                else
                {
                    currentCell = path.Pop(); //Pop saca 1 elemento de la fila
                }
            }
            else //si hay vecinos
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)]; //elegir vecino aleatorio

                if (newCell > currentCell) //derecha o arriba
                {
                    if (newCell - 1 == currentCell) //derecha
                    {
                        board[currentCell].status[3] = true;
                        board[newCell].status[2] = true;
                    }
                    else //arriba
                    {
                        board[currentCell].status[0] = true;
                        board[newCell].status[1] = true;
                    }
                } 
                else
                {
                    if (newCell + 1 == currentCell) //izquierda
                    {
                        board[currentCell].status[2] = true;
                        board[newCell].status[3] = true;
                    }
                    else //abajo
                    {
                        board[currentCell].status[1] = true;
                        board[newCell].status[0] = true;
                    }
                }
                currentCell = newCell;
                num++;
            }
        }
        DungeonGenerator();
    }

    private List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        if(cell - size.x > 0 && !board[cell - size.x].visited)
        {
            neighbors.Add(cell - size.x); //comprobamos down
        }

        if (cell + size.x < board.Count && !board[cell +size.x].visited)// comprobamos top
        {
            neighbors.Add(cell + size.x); //comprobamos up
        }

        if (cell % size.x != 0 && !board[cell - 1].visited) // comprobamos left
        {
            neighbors.Add(cell - 1); //comprobamos left
        }

        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited) // comprobamos right
        {
            neighbors.Add(cell + 1); //comprobamos right
        }

        return neighbors;
    }

    private void DungeonGenerator()
    {
        for(int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                var newRoom = Instantiate(room, new Vector3(i * roomSize.x, 0, j * roomSize.y), Quaternion.identity).GetComponent<Room>();
                newRoom.UpdateRoom(board[i + j * size.x].status);
            }
        }
    }
}
