using UnityEngine;
using System.Collections;

public class CrystalItem : MonoBehaviour
{
    public float autoCollectTime = 1.5f;

    private CrystalManager manager;
    private bool collected = false;

    void Start()
    {
        StartCoroutine(AutoCollect());
    }

    public void SetManager(CrystalManager cm)
    {
        manager = cm;
    }

    IEnumerator AutoCollect()
    {
        yield return new WaitForSeconds(autoCollectTime);
        Collect();
    }

    void Collect()
    {
        if (collected) return;

        collected = true;

        if (manager == null)
            manager = FindObjectOfType<CrystalManager>();

        if (manager != null)
            manager.AddCrystal();

        Destroy(gameObject);
    }
}