using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Puertas")]
    [SerializeField] private GameObject[] wallDoor; // 0-Top, 1-Down, 2-Left, 3-Right
    private bool connected; // Indica si la habitacion esta conectada a otra

    
    public void UpdateRoom(bool[] status)
    {
        connected = false;

        for (int i = 0; i < status.Length; i++)
        {
            wallDoor[i].SetActive(!status[i]);

            if (status[i])
            {
                connected = true;
            }
        }

        if (!connected)
        {
            Destroy(gameObject);
        }
    }
}
