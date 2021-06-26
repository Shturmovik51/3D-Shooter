using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody bulletRigidbody;
    [SerializeField] private int bulletDamage;
    //public Rigidbody BulletRigidbody { get; set; }
    private void OnEnable()
    {
        SoundManager.instance.shoot.Play();
    }
    public void BulletShoot(int damage)
    {
        bulletDamage = damage;
    }
    private void OnCollisionEnter(Collision col)
    {
        if (GameManager.instance.healthContainer.ContainsKey(col.gameObject))
        {
            Health health = GameManager.instance.healthContainer[col.gameObject];
            health.TakeDamage(bulletDamage);
        }
        bulletDamage = 0;
    }


}
