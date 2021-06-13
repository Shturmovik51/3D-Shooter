using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int thisHealth;
    public int ThisHealth { get { return thisHealth; } set { thisHealth = value;} }

    private void Start()
    {
        GameManager.instance.healthContainer.Add(gameObject, this);
    }    

    private void Update()
    {
        if(thisHealth <= 0)
        {
            Destroy(gameObject);
        }
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
