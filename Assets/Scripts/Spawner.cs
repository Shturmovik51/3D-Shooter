using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private EnemyUI spawnObject;
    [SerializeField] private Transform patroslWPParent;
    [SerializeField] private Transform safesWPParent;
    [SerializeField] private Transform spawnPoint;
    private List<Transform> patrolPoints;
    private List<Transform> safePoints;
    private int enemyCount;
    private bool isSpawning;
    [SerializeField] private int enemyMaxCount;
    private void Start()
    {
        patrolPoints = new List<Transform>();
        safePoints = new List<Transform>();
        WPinitialyser(patroslWPParent, safesWPParent);
        isSpawning = true;
        enemyCount = 0;
    }
    private void Update()
    {
        if(enemyCount < enemyMaxCount && isSpawning)
        {
            StartCoroutine(EnemySpawner());
        }
    }
    public void WPinitialyser(Transform patrol, Transform safe)
    {      
        foreach (Transform child in patrol)
        {
            patrolPoints.Add(child);            
        }
        
        foreach (Transform child in safe)
        {
            safePoints.Add(child);
        }
    }
    private IEnumerator EnemySpawner()
    {
        isSpawning = false;
        yield return new WaitForSeconds(3);
        var enemySample = Instantiate(spawnObject, spawnPoint.transform.position, Quaternion.identity);
        enemySample.transform.parent = null;
        var enemyUI = enemySample.GetComponent<EnemyUI>();
        enemyUI.WPinitialyser(patrolPoints, safePoints);
        isSpawning = true;
        enemyCount++;
    }
}
