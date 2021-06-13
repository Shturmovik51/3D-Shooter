using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform turrel;
    [SerializeField] private Transform bulletStartPos;
    [SerializeField] private Transform bulletContainer;
    [SerializeField] private int shootForce;
    [SerializeField] private Bullet bullet;
    private List<Bullet> bullets;
    private bool isHaveTarget;
    private void Start()
    {
        bullets = new List<Bullet>();
        while (bullets.Count != 50)
        {
            var bulletSample = Instantiate(bullet, bulletContainer.transform.position, Quaternion.identity);
            bulletSample.transform.parent = bulletContainer.transform;
            bullet.gameObject.SetActive(false);
            bullets.Add(bulletSample);
        }
    }
    private void TurrelLook()
    {     
        var bodyPos = target.position - turrel.position;
        var bodyDir = Vector3.RotateTowards(turrel.forward, bodyPos, 0.5f * Time.deltaTime, 0.0f); 
        turrel.rotation = Quaternion.LookRotation(bodyDir);
    }

    private void TurrelShoot()
    {
        if (isHaveTarget)
        {
            var bullet = bullets[0];
            bullets.Remove(bullet);        
            bullet.transform.position = bulletStartPos.transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.parent = null;
            bullet.gameObject.SetActive(true);
            bullet.bulletRigidbody.AddForce(transform.forward * shootForce, ForceMode.Impulse);       
            StartCoroutine(BulletLifeTime(bullet));
            StartCoroutine(ShootDelay());
        }
        
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
        yield return new WaitForSeconds(1);
        TurrelShoot();
        yield break;
    }


    private void TowerLook()
    {
        var pos = target.position - transform.position;
        var dir = Vector3.RotateTowards(transform.forward, pos, 0.2f * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(dir);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TurrelLook();
            TowerLook();
        }       
    }
}
