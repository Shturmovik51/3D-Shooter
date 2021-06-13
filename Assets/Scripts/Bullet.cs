using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRigidbody;
    //public Rigidbody BulletRigidbody { get; set; }
    private void OnTriggerEnter(Collider col)
    {
        if (GameManager.instance.healthContainer.ContainsKey(col.gameObject))
        {
            Health health = GameManager.instance.healthContainer[col.gameObject];
            health.TakeDamage(5);
        }
        
    }

}
