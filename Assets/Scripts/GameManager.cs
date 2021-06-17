using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Dictionary<GameObject, Health> healthContainer;
   // public Dictionary<GameObject, EnemyUI> enemyUIContainer;

    private void Awake()
    {
        instance = this;
        
        healthContainer = new Dictionary<GameObject, Health>();
       // enemyUIContainer = new Dictionary<GameObject, EnemyUI>();
    }   


}
