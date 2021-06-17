using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] private Observer observer;
    [SerializeField] private Health enemyHealth;
    [SerializeField] private Rigidbody enemyRigidbody;
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private List<Transform> safePoints;
    private Transform currentPoint = null;
    private Transform currentSafePoint = null;
    private Transform newPoint;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform body;
    private Transform target;
    public Transform Target { get { return target; } set { target = value; } }
    private float rotationSpeed = 2f;

    [SerializeField] private int shootForce;
    [SerializeField] private int bulletCount;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Transform bulletStartPos;
    [SerializeField] private Transform rightArm;
    private List<Bullet> bullets;
    private bool isShooting;
    private bool isOnReloading;
    public bool isDead;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        bullets = new List<Bullet>();
        while (bullets.Count != bulletCount)
        {
            var bulletSample = Instantiate(bullet, bulletContainer.transform.position, Quaternion.identity);
            bulletSample.transform.parent = bulletContainer.transform;
            bullet.gameObject.SetActive(false);
            bullets.Add(bulletSample);
        }

        patrolPoints = new List<Transform>();
        safePoints = new List<Transform>();
    }
    private void Start()
    {
        enemyHealth.deathEntity += Death;
    }
    private void Update()
    {
        if (!isDead)
        {

            if (target == null)
            {
                Patrol();
            }
            else
            {
                Alarm();
            }

            if (isShooting == true && !isOnReloading)
            {
                StartCoroutine(Shoot());
            }
        }

    }    
    public void WPinitialyser(List<Transform> patrol, List<Transform> safe)
    {
        patrolPoints = patrol;
        safePoints = safe;
    }
    private void Patrol()
    {
        isShooting = false;

        if (currentPoint == null) 
        {
            currentPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
            navMeshAgent.SetDestination(currentPoint.position);
        }

        var bodyDir = Vector3.RotateTowards(body.forward, transform.forward, rotationSpeed * Time.deltaTime, 0.0f);
        body.rotation = Quaternion.LookRotation(bodyDir);

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentPoint = newPoint;
            while (newPoint == currentPoint)
            {
                newPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
            }
            navMeshAgent.destination = (newPoint.position);
        }
    }
        
    public void SafePointRun()
    {
        currentPoint = null;

        if(currentSafePoint == null)
        {
            currentSafePoint = safePoints[Random.Range(0, safePoints.Count)];            
        }
        navMeshAgent.SetDestination(currentSafePoint.position);
        Debug.Log("Alarm!");

        //for (int i = 0; i < safePoints.Length; i++)
        //{           
        //    navMeshAgent.SetDestination(currentSafePoint.position);
        //    dist1 = navMeshAgent.remainingDistance;
        //    navMeshAgent.SetDestination(safePoints[i].position);
        //    dist2 = navMeshAgent.remainingDistance;
        //    Debug.Log("Da");

        //    if (dist2 < dist1)
        //    {
        //        currentSafePoint = safePoints[i];
        //        Debug.Log("DaDa");
        //    }            
        //}
        //navMeshAgent.SetDestination(currentSafePoint.position);
    }

    private void Alarm()
    {
        var bodyPos = new Vector3(target.position.x, body.position.y, target.position.z) - body.position;
        // var bodyPos = new Vector3(target.position.x - body.position.x, 0f, target.position.z - body.position.z);
        var bodyDir = Vector3.RotateTowards(body.forward, bodyPos, rotationSpeed * Time.deltaTime, 0.0f);
        body.rotation = Quaternion.LookRotation(bodyDir);
        isShooting = true;
    }

    private IEnumerator Shoot()
    {
        isOnReloading = true;
        var bullet = bullets[0];
        bullets.Remove(bullet);
        bullet.transform.position = bulletStartPos.transform.position;
        bullet.transform.rotation = rightArm.rotation;
        bullet.transform.parent = null;
        bullet.gameObject.SetActive(true);
        bullet.bulletRigidbody.velocity = Vector3.zero;
        bullet.bulletRigidbody.AddForce(rightArm.forward * shootForce, ForceMode.Impulse);
        StartCoroutine(BulletLifeTime(bullet));
        yield return new WaitForSeconds(1);       
        isOnReloading = false;

        yield break;
    }
    private IEnumerator BulletLifeTime(Bullet b)
    {
        yield return new WaitForSeconds(4);
        bullets.Add(b);
        b.transform.position = bulletContainer.transform.position;
        b.transform.parent = bulletContainer.transform;
        b.transform.rotation = Quaternion.identity;
        b.bulletRigidbody.velocity = Vector3.zero;
        b.gameObject.SetActive(false);

        yield break;
    }

    public void Death()
    {
        Debug.Log("Action");
        isDead = true;

        enemyRigidbody.freezeRotation = false;
        enemyRigidbody.isKinematic = false;
        enemyRigidbody.AddForce(Vector3.forward*50, ForceMode.Impulse);
        Destroy(observer.gameObject);
        Destroy(navMeshAgent);
        Destroy(this);        

    }
}
