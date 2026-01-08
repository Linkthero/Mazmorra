using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Puertas")]
    [SerializeField] private GameObject[] wallDoor; // 0-Top, 1-Down, 2-Left, 3-Right
    private bool connected; // Indica si la habitacion esta conectada a otra

    // Metodo para activar las puertas según el estado
    public void UpdateRoom(bool[] status)
    {
        connected = false; // Inicialmente no está conectada

        // Recorrer cada puerta
        for (int i = 0; i < status.Length; i++)
        {
            // Activar o desactivar la puerta física
            wallDoor[i].SetActive(!status[i]);

            // Si la puerta está abierta, la habitación está conectada
            if (status[i])
            {
                connected = true;
            }
        }

        // Si ninguna puerta está conectada, destruir la habitación
        if (!connected)
        {
            Destroy(gameObject);
        }
    }
}
