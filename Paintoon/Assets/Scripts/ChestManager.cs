using UnityEngine;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    public Chest[] chests;

    public int crystalCount = 5;

    void Start()
    {
        List<int> selected = new List<int>();

        while (selected.Count < crystalCount)
        {
            int rand = Random.Range(0, chests.Length);

            if (!selected.Contains(rand))
            {
                selected.Add(rand);
                chests[rand].hasCrystal = true;
            }
        }
    }
}