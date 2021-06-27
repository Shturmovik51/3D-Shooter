using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityAction deathEntity;
    [SerializeField] private int thisHealth;
    public int ThisHealth { get { return thisHealth; } set { thisHealth = value;} }

    private void Start()
    {
        GameManager.instance.healthContainer.Add(gameObject, this);
    }    

    private void Update()
    {
        if(thisHealth <= 0f)
        {
            GameManager.instance.healthContainer.Remove(gameObject);
            deathEntity?.Invoke();
            Destroy(this);
        }

        //if(thisHealth <= 0 && gameObject.CompareTag("Player") ||
        //    thisHealth <= 0 && gameObject.CompareTag("Enemy")   )
        //{
        //    GameManager.instance.healthContainer.Remove(gameObject);
        //    deathEntity();
        //    Destroy(this);
        //}
        //else if (thisHealth <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    public void TakeDamage(int damage)
    {
        thisHealth -= damage;
    }
    public void HealthUp(int bonusHealth)
    {
        thisHealth += bonusHealth;
    }
}
