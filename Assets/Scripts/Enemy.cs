using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Observer observer;
    [SerializeField] private Health enemyHealth;
    [SerializeField] private Rigidbody enemyRigidbody;
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private List<Transform> safePoints;
    [SerializeField] private Animator enemyAnimator;
    private Transform currentPoint = null;
    private Transform currentSafePoint = null;
    private Transform newPoint;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform body;
    private Transform target;
    public Transform Target { get { return target; } set { target = value; } }
    private float rotationSpeed = 2f;
    [SerializeField] private int shootDamage;
    [SerializeField] private int shootForce;
    [SerializeField] private int bulletCount;
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Transform bulletStartPos;
    [SerializeField] private Transform rightArm;
    private List<Bullet> bullets;
    private bool isShooting;
    private bool isOnReloading;
    private bool isTargetVisible;
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
        enemyAnimator.SetBool("IsWalking", true);
    }
    private void Update()
    {
        if (isDead) return;        

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
            TargetVisibilityCheck();
            if (isTargetVisible)
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
        rightArm.rotation = Quaternion.LookRotation(bodyDir);

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

        if (currentSafePoint == null)
        {
            currentSafePoint = safePoints[Random.Range(0, safePoints.Count)];
        }
        navMeshAgent.SetDestination(currentSafePoint.position);

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
        var bodyDir = Vector3.RotateTowards(body.forward, bodyPos, rotationSpeed * Time.deltaTime, 0.0f);
        body.rotation = Quaternion.LookRotation(bodyDir);

        var rArmPos = new Vector3(target.position.x, rightArm.position.y, target.position.z) - rightArm.position;
        var rArmDir = Vector3.RotateTowards(body.forward, rArmPos, rotationSpeed * Time.deltaTime, 0.0f);
        body.rotation = Quaternion.LookRotation(rArmDir);
        isShooting = true;
    }

    private void TargetVisibilityCheck()
    {
        RaycastHit hit;       
        var rayCast = Physics.Raycast(bulletStartPos.position, bulletStartPos.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Default"));
       
        if (rayCast)
        {
            if (hit.collider.CompareTag("Player"))
            {
                isTargetVisible = true;
            }
            else isTargetVisible = false;
        }
    }

    private IEnumerator Shoot()
    {
        isOnReloading = true;
        var bullet = bullets[0];
        bullets.Remove(bullet);
        bullet.transform.position = bulletStartPos.transform.position;
        bullet.transform.rotation = rightArm.rotation;
        bullet.transform.parent = null;
        bullet.BulletShoot(shootDamage);
        bullet.gameObject.SetActive(true);
        bullet.bulletRigidbody.velocity = Vector3.zero;
        bullet.bulletRigidbody.AddForce(rightArm.forward * shootForce, ForceMode.Impulse);
        enemyAnimator.SetTrigger("Shoot");
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

        enemyAnimator.SetBool("IsWalking", false);
        isDead = true;
        navMeshAgent.enabled = false;
        enemyRigidbody.freezeRotation = false;
        enemyRigidbody.isKinematic = false;
        enemyRigidbody.AddForce(Random.insideUnitSphere * 400, ForceMode.Impulse);
        UIController.instanse.enemyKillsCount++;
        Destroy(observer.gameObject);
        Destroy(this);

    }
}
