using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private EnemyUI enemyUI; 
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            enemyUI.Target = col.transform;
            enemyUI.SafePointRun();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            enemyUI.Target = null;
        }
    }
}
