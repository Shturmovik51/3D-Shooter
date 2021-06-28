using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private Explosion barrelExpl;
    [SerializeField] private Health barrelHealth;
    [SerializeField] private GameObject firePS;
    [SerializeField] private GameObject barrelVFXExpl;
    [SerializeField] private GameObject barrelBody;

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
        barrelVFXExpl.SetActive(true);
        barrelVFXExpl.transform.parent = null;
        Destroy(barrelBody);
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
