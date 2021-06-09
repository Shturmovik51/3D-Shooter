using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 moveDirection;
    private float mouseLook;
    [SerializeField] private CharacterController charControl;
    [SerializeField] private int playerSpeed;
    [SerializeField] private int sensitivity;

    [SerializeField] private Bomb bomb;
    [SerializeField] private int bombCount;
    [SerializeField] private int throwForce;
    [SerializeField] private GameObject bombStartPos;
    [SerializeField] private GameObject bombContainer;
    private List<Bomb> bombs;
    private bool isBombReload;

    [SerializeField] private Bullet bullet;
    [SerializeField] private int bulletCount;
    [SerializeField] private int shootForce;
    [SerializeField] private GameObject bulletStartPos;
    [SerializeField] private GameObject bulletContainer;
    private List<Bullet> bullets;
    private void Start()
    {
        bullets = new List<Bullet>();
        while (bullets.Count != bulletCount)
        {
            var bulletSample = Instantiate(bullet, bulletContainer.transform.position, Quaternion.identity);
            bulletSample.transform.parent = bulletContainer.transform;
            bullet.gameObject.SetActive(false);
            bullet.bulletRigidbody.isKinematic = true;
            bullets.Add(bulletSample);
        }

        bombs = new List<Bomb>();
        while (bombs.Count != bombCount)
        {
            var bombSample = Instantiate(bomb, bombContainer.transform.position, Quaternion.identity);
            bombSample.transform.parent = bombContainer.transform;
            bomb.gameObject.SetActive(false);
            bomb.bombRigidbody.isKinematic = true;
            bombs.Add(bombSample);
        }

        Cursor.visible = false;
    }
    void Update()
    {
        PlayerMovement();
        PlayerLook();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isBombReload)
        {
            BombThrow();
        }

    }
        
    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        moveDirection = transform.right*x + transform.forward*z;
        
        charControl.Move(moveDirection * playerSpeed * Time.deltaTime);
       // transform.Translate(moveDirection * playerSpeed * Time.deltaTime);
    }

    private void PlayerLook()
    {        
        mouseLook = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseLook * sensitivity * Time.deltaTime, 0);
    }

    private void Shoot()
    {
        var bullet = bullets[0];
        bullets.Remove(bullet);
        bullet.gameObject.SetActive(true);
        bullet.bulletRigidbody.isKinematic = false;
        bullet.transform.position = bulletStartPos.transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.bulletRigidbody.AddForce(transform.forward*shootForce, ForceMode.Impulse);        
        bullet.transform.parent = null;
        StartCoroutine(BulletLifeTime(bullet));
    }

    private void BombThrow()
    {
        var bomb = bombs[0];
        bombs.Remove(bomb);
        bomb.gameObject.SetActive(true);
        bomb.bombRigidbody.isKinematic = false;
        bomb.transform.position = bombStartPos.transform.position;
        bomb.transform.rotation = bombStartPos.transform.rotation;
        bomb.bombRigidbody.AddForce(bombStartPos.transform.forward * throwForce, ForceMode.Impulse);
        bomb.transform.parent = null;
        isBombReload = true;
        StartCoroutine(BombLifeTime(bomb));
    }
    private IEnumerator BulletLifeTime(Bullet b)
    {
        yield return new WaitForSeconds(4);
        bullets.Add(b);
        b.transform.position = bulletContainer.transform.position;
        b.transform.parent = bulletContainer.transform;
        b.transform.rotation = Quaternion.identity;
        b.bulletRigidbody.isKinematic = true;
        b.gameObject.SetActive(false);

        yield break;
    }

    private IEnumerator BombLifeTime(Bomb b)
    {
        yield return new WaitForSeconds(2);
        isBombReload = false;
        yield return new WaitForSeconds(2);
        bombs.Add(b);
        b.transform.position = bombContainer.transform.position;
        b.transform.parent = bombContainer.transform;
        b.transform.rotation = Quaternion.identity;
        b.bombRigidbody.isKinematic = true;
        b.gameObject.SetActive(false);

        yield break;
    }
}

