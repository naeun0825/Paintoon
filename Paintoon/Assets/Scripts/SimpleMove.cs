using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float speed = 3f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0, z);

        controller.Move(move * speed * Time.deltaTime);
    }
}