using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform mainPropeller;
    [SerializeField] private Transform backPropeller;
    [SerializeField] private Health bossHealth;
    [SerializeField] private List<Transform> forWardPoss;
    [SerializeField] private List<Transform> backWardPoss;
    [SerializeField] private List<Transform> attakPoints;
    [SerializeField] private Transform missilePos;
    [SerializeField] private Transform bodyTR;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int changePosSpeed;
    [SerializeField] private Missile missile;
    [SerializeField] private Explosion bossExpl;
    private Transform target;

    private bool isForWardSide = false;
    private bool isReloading = false;
    private bool isOnPosition = false;
    private bool isDead = false;
    public bool isBossAlarm;
    
    private Vector3 movePos;

    private void Start()
    {
        bossHealth.deathEntity += BossDeath;
        StartCoroutine(BossLogic());
        target = Player.instance.transform;
    }

    private void Update()
    {
        if (!isBossAlarm) return;

        mainPropeller.Rotate(Vector3.up, 4200*Time.deltaTime);
        backPropeller.Rotate(Vector3.up, 4200 * Time.deltaTime);

        if (isDead) return;

        BossMove();
        BossLook();

        isOnPosition = ((movePos - transform.position).magnitude < 40) ? true : false;

        if (isOnPosition && !isReloading)
            StartCoroutine(BossShoot());

    }

    private void BossMove()
    {
        var pos = Vector3.Lerp(transform.position, movePos, moveSpeed * Time.deltaTime);
        transform.position = pos;
    }

    private void BossLook()
    {
        var bodyPos = target.position - transform.position;
        var bodyDir = Vector3.RotateTowards(bodyTR.forward, bodyPos, rotationSpeed * Time.deltaTime, 0.0f);
        bodyTR.rotation = Quaternion.LookRotation(bodyDir);
    }

    private IEnumerator BossLogic()
    {
        if (!isForWardSide)
        {
            movePos = forWardPoss[Random.Range(0, forWardPoss.Count)].position;           
        }
        if (isForWardSide)
        {
            movePos = backWardPoss[Random.Range(0, backWardPoss.Count)].position;           
        }
        isForWardSide = (isForWardSide == true) ? false : true;
        yield return new WaitForSeconds(changePosSpeed);
        StartCoroutine(BossLogic());
    }

    private IEnumerator BossShoot()
    {
        isReloading = true;

        List<Transform> points = new List<Transform>();
        for (int i = 0; i < 3; i++)
        {
            var point = attakPoints[Random.Range(0, attakPoints.Count)];
            point.gameObject.SetActive(true);
            points.Add(point);
        }        

        yield return new WaitForSeconds(3);

        foreach (var point in points)
        {
            var currentMisile = Instantiate(missile);
            currentMisile.InitMissile(point, missilePos);
            point.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(6);
        isReloading = false;
    }
    private void BossDeath()
    {
        bossExpl.Boom();
        Destroy(bossExpl.gameObject);
        isDead = true;
        UIController.instanse.WinGame();
        Player.instance.isWinner = true;
    }
}
