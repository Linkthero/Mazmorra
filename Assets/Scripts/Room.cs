using UnityEngine;

public class Room : MonoBehaviour
{
    ////Declaracion de variables
    //[SerializeField] private GameObject[] wallDoor; //0 - top; 1- down ; 2 - left; 3 - right


    //bool connected;

    //public void UpdateRoom(bool[] status)
    //{
    //    connected = false;

    //    for(int i = 0; i < status.Length; i++)
    //    {
    //        wallDoor[i].SetActive(!status[i]);
    //        if(status[i])
    //        {
    //            connected = true;
    //        }

    //        if(!connected)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}
    [Header("Puertas")]
    [SerializeField] private GameObject[] wallDoor; // 0-Top, 1-Down, 2-Left, 3-Right
    private bool connected; // Indica si la habitacion esta conectada a otra

    //[Header("Luces para puertas")]
    //[SerializeField] private GameObject doorLightPrefab; // Prefab de la luz que se instanciara
    //[SerializeField] private Transform[] doorLightPoints; // Puntos donde se colocaran las luces: 0-Top, 1-Down, 2-Left, 3-Right

    // Metodo para activar las puertas y luces según el estado
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

                // Instanciar luz en la puerta si existe prefab y punto asignado
                //if (doorLightPrefab != null && doorLightPoints != null && doorLightPoints.Length > i && doorLightPoints[i] != null)
                //{
                //    Instantiate(doorLightPrefab, doorLightPoints[i].position, Quaternion.identity, transform);
                //}
            }
        }

        // Si ninguna puerta está conectada, destruir la habitación
        if (!connected)
        {
            Destroy(gameObject);
        }
    }
}
