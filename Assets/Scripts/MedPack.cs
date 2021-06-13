using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedPack : MonoBehaviour
{
    [SerializeField] private int bonusHealth;
    public int Health { get; set; }

    private void OnTriggerEnter(Collider col)
    {
        if (GameManager.instance.healthContainer.ContainsKey(col.gameObject))
        {
            var health = GameManager.instance.healthContainer[col.gameObject];
            health.HealthUp(bonusHealth);

            Destroy(gameObject);
        }
    }
}
