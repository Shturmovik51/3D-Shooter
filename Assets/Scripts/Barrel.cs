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
    [SerializeField] private AudioSource barrelSound;

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
        barrelSound.Play();
        barrelExpl.Boom();
        barrelVFXExpl.SetActive(true);
        barrelVFXExpl.transform.parent = null;
        Destroy(barrelBody);
        yield return new WaitForSeconds(10);
        Destroy(barrelVFXExpl);
        Destroy(gameObject);
    }
}
