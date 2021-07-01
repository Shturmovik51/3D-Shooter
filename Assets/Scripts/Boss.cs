using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform mainPropeller;
    [SerializeField] private Transform backPropeller;
    [SerializeField] private Health bossHealth;
    [SerializeField] private Collider bodyCollider;
    [SerializeField] private List<Transform> forWardPoss;
    [SerializeField] private List<Transform> backWardPoss;
    [SerializeField] private List<Transform> attakPoints;
    [SerializeField] private Transform firstMeetPos;
    [SerializeField] private Transform missilePos;
    [SerializeField] private Transform bodyTR;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int changePosSpeed;
    [SerializeField] private Missile missile;
    [SerializeField] private Explosion bossExpl;
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private int countOfShoots;
    [SerializeField] private Explosion rightGunExpl;
    [SerializeField] private GameObject bossWFXExpl;
    [SerializeField] private GameObject bossWFXHalfDeath;
    [SerializeField] private GameObject bossWFXNearDeath;
    [SerializeField] private Image bossHPBar;
    [SerializeField] private GameObject bossPanel;
    [SerializeField] private AudioSource helicopterExplsound;

    private Transform target;
    private bool isForWardSide = false;
    private bool isReloading = false;
    private bool isOnPosition = false;
    private bool isDead = false;
    public bool isFirstMeet = false;
    private float maxHealth;

    private Vector3 movePos;

    private void Start()
    {
        bossHealth.deathEntity += BossDeath;
        bossHealth.updateHP += UpdateHPBar;
        //target = Player.instance.transform;
        maxHealth = bossHealth.ThisHealth;
    }

    private void Update()
    {
        mainPropeller.Rotate(Vector3.up, 4200 * Time.deltaTime);
        backPropeller.Rotate(Vector3.up, 4200 * Time.deltaTime);

        if (isDead) return;

        BossMove();
        BossLook();
        BossCondition();

        isOnPosition = ((movePos - transform.position).magnitude < 40) ? true : false;

        if (isOnPosition && isFirstMeet)
            gameObject.SetActive(false);

        if (isOnPosition && !isReloading && !isFirstMeet)
            StartCoroutine(BossShoot());
    }
    private void UpdateHPBar(int hp)
    {
        bossHPBar.fillAmount = (hp / maxHealth);
    }
    private void BossMove()
    {
        var pos = Vector3.Lerp(transform.position, movePos, moveSpeed * Time.deltaTime);
        transform.position = pos;
    }

    private void BossLook()
    {
        if (target != null)
        {
            var bodyPos = target.position - transform.position;
            var bodyDir = Vector3.RotateTowards(bodyTR.forward, bodyPos, rotationSpeed * Time.deltaTime, 0.0f);
            bodyTR.rotation = Quaternion.LookRotation(bodyDir);
        }
    }
    private void BossCondition()
    {
        if(bossHealth.ThisHealth < maxHealth * 0.5f && bossHealth.ThisHealth > maxHealth * 0.3f)
        {
            bossWFXHalfDeath.SetActive(true);
        }

        if (bossHealth.ThisHealth < maxHealth * 0.3f)
        {
            rightGunExpl.Boom();
            bossWFXNearDeath.SetActive(true);
        }
    }
    public void FirstActivateBoss()
    {
        movePos = firstMeetPos.position;
        isFirstMeet = true;
        gameObject.SetActive(true);
        engineSound.Play();
    }
    public void SecondActivateBoss()
    {
        if(target != null)
        {
            target = Player.instance.transform;
            return;
        }

        target = Player.instance.transform;
        isFirstMeet = false;
        bossHealth.ThisHealth = (int)maxHealth;
        UpdateHPBar((int)maxHealth);
        bossPanel.SetActive(true);
        gameObject.SetActive(true);
        StartCoroutine(BossLogic());
        engineSound.Play();
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
        for (int i = 0; i < countOfShoots; i++)
        {
            var point = attakPoints[Random.Range(0, attakPoints.Count)];
            point.gameObject.SetActive(true);
            points.Add(point);
        }        

        yield return new WaitForSeconds(2);

        foreach (var point in points)
        {
            var currentMisile = Instantiate(missile);
            currentMisile.InitMissile(point, missilePos);
            point.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(2);
        isReloading = false;
    }
    private void BossDeath()
    {
        bossExpl.Boom();
        helicopterExplsound.Play();
        Destroy(bossExpl.gameObject);
        bodyCollider.enabled = false;
        bossWFXHalfDeath.SetActive(false);
        bossWFXNearDeath.SetActive(false);
        isDead = true;
        bossWFXExpl.SetActive(true);
        UIController.instanse.WinGame();
        Player.instance.isWinner = true;
    }
}
