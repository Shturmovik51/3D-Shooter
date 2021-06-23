using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private Enemy enemy; 
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            enemy.Target = col.transform;
            enemy.SafePointRun();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            enemy.Target = null;
        }
    }
}
