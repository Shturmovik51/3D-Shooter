using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private Transform turrelStand;
    [SerializeField] private Transform turrelBody;
    [SerializeField] private Transform bulletStartPos;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private Transform tankTover;
    [SerializeField] private Explosion engineExpl;
    [SerializeField] private Health tankHealth;
    [SerializeField] private int shootForce;
    [SerializeField] private int shootDamage;
    [SerializeField] private Bullet bullet;
    [SerializeField] private int tankAmmoCount;
    [SerializeField] private GameObject tankWFXExpl;
    [SerializeField] private AudioSource tankExplSound;

    private Transform target;
    private List<Bullet> bullets;
    private bool isOnDelayShoot;
    private bool isTargetVisible;
    private bool isDead;

    private void Start()
    {
        tankHealth.deathEntity += TankExplosion;

        bullets = new List<Bullet>();
        while (bullets.Count != tankAmmoCount)
        {
            var bulletSample = Instantiate(bullet, bulletContainer.transform.position, Quaternion.identity);
            bulletSample.transform.parent = bulletContainer.transform;
            bullet.gameObject.SetActive(false);
            bullets.Add(bulletSample);
        }        
    }
    private void Update()
    {
        if (isDead) return;

        if (target != null)
        {
            TurrelLook();
            TowerLook();
            TargetVisibilityCheck();
            if (isTargetVisible && !isOnDelayShoot)
            {
                TurrelShoot();
            }
        }
    }
    private void TurrelLook()
    {
        var standPos = new Vector3(target.position.x, turrelStand.position.y, target.position.z) - turrelStand.position;
        var standDir = Vector3.RotateTowards(turrelStand.forward, standPos, 1f * Time.deltaTime, 0.0f);
        turrelStand.rotation = Quaternion.LookRotation(standDir);

        var bodypos = target.position - turrelBody.position;
        var bodydir = Vector3.RotateTowards(turrelBody.forward, bodypos, 1f * Time.deltaTime, 0.0f);
        turrelBody.rotation = Quaternion.LookRotation(bodydir);
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

    private void TurrelShoot()
    {
        isOnDelayShoot = true;
        var bullet = bullets[0];
        bullets.Remove(bullet);
        bullet.transform.position = bulletStartPos.transform.position;
        bullet.transform.rotation = turrelBody.rotation;
        bullet.transform.parent = null;
        bullet.BulletShoot(shootDamage);
        bullet.gameObject.SetActive(true);
        bullet.bulletRigidbody.velocity = Vector3.zero;
        bullet.bulletRigidbody.AddForce(bullet.transform.forward * shootForce, ForceMode.Impulse);
        StartCoroutine(BulletLifeTime(bullet));
        StartCoroutine(ShootDelay());
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
    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(0.2f);
        isOnDelayShoot = false;
        yield break;
    }
    private void TowerLook()
    {       
        var pos = new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position;
        var dir = Vector3.RotateTowards(tankTover.forward, pos, 0.2f * Time.deltaTime, 0.0f);
        tankTover.rotation = Quaternion.LookRotation(dir);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))        
            target = col.transform;        
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))       
            target = null;
    }

    private void TankExplosion()
    {
        engineExpl.Boom();
        tankExplSound.Play();
        tankWFXExpl.SetActive(true);
        Destroy(engineExpl.gameObject);
        isDead = true;
    }
}
