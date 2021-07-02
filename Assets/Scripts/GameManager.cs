using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dictionary<GameObject, Health> healthContainer;
    // public Dictionary<GameObject, EnemyUI> enemyUIContainer;
    public List<Savepoint> savepoints;

    private void Awake()
    {
        instance = this;
        savepoints = new List<Savepoint>();
        healthContainer = new Dictionary<GameObject, Health>();
       // enemyUIContainer = new Dictionary<GameObject, EnemyUI>();
    }   

    public void ResetSavePoints()
    {
        foreach (var point in savepoints)
        {
            point.isActive = false;
        }
    }


}
