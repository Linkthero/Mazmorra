using UnityEngine;

public class Room : MonoBehaviour
{
    //Declaracion de variables
    [SerializeField] private GameObject[] wallDoor; //0 - top; 1- down ; 2 - left; 3 - right


    bool connected;

    public void UpdateRoom(bool[] status)
    {
        connected = false;

        for(int i = 0; i < status.Length; i++)
        {
            wallDoor[i].SetActive(!status[i]);
            if(status[i])
            {
                connected = true;
            }

            if(!connected)
            {
                Destroy(gameObject);
            }
        }
    }

}
