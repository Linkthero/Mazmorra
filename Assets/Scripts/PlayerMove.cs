using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 input;

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }
    

}
