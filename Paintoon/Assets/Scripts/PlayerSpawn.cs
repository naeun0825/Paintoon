using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.identity;
    }
}