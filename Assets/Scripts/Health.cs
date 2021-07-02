using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityAction deathEntity;
    public UnityAction<int> updateHP;
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
    }

    public void TakeDamage(int damage)
    {
        thisHealth -= damage;
        updateHP?.Invoke(thisHealth);
    }
    public void HealthUp(int bonusHealth)
    {
        thisHealth += bonusHealth;
        updateHP?.Invoke(thisHealth);
    }
}
