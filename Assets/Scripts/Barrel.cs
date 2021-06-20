using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private Explosion barrelExpl;
    [SerializeField] private Health barrelHealth;
    [SerializeField] private GameObject firePS;

    private void Start()
    {
        barrelHealth.deathEntity += BarrelDeath;
    }

    private void BarrelDeath()
    {
        StartCoroutine(BarrelExplosion());
    }

    private IEnumerator BarrelExplosion()
    {
        firePS.SetActive(true);
        yield return new WaitForSeconds(2);
        barrelExpl.Boom();
        Destroy(gameObject);
    }
}
