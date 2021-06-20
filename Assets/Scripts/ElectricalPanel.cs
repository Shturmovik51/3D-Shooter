using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalPanel : MonoBehaviour
{
    [SerializeField] private Explosion elPanelExpl;
    [SerializeField] private Health elPanelHealth;
    [SerializeField] private BoxCollider elPanelCollider;
    public bool isDestroyed;

    private void Start()
    {
       // elPanelCollider = GetComponent<BoxCollider>();
        elPanelHealth.deathEntity += BarrelDeath;
    }

    private void BarrelDeath()
    {
        elPanelCollider.enabled = false;
        elPanelExpl.Boom();
        isDestroyed = true;
    }
}
