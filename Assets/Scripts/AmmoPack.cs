using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    [SerializeField] private int bonusAmmo;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Player.instance.AmmoCount += bonusAmmo;
            Destroy(gameObject);
        }
    }
}
