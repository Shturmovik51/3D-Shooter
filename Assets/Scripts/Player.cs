using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Vector3 moveDirection;
    private float gravity = -9.81f;
    public Vector3 gravitation;
    public LayerMask groundMask;
    public bool isGrounded;
    [SerializeField] private int ammoCount;
    public int AmmoCount { get { return ammoCount; } set { ammoCount = value; } }
    [SerializeField] private Health playerHealth;
    public Health PlayerHealth { get { return playerHealth; } set { playerHealth = value; } }

    [SerializeField] private Transform groundDetector;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private CharacterController charControl;
    [SerializeField] private int playerSpeed;
    [SerializeField] private int shiftedSpeed;
    [SerializeField] private int sensitivity;
    [SerializeField] private Transform head;
    [SerializeField] private Transform rightArm;

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


    private float xRotation = 0f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isGrounded = false;
        bullets = new List<Bullet>();
        while (bullets.Count != bulletCount)
        {
            var bulletSample = Instantiate(bullet, bulletContainer.transform.position, Quaternion.identity);
            bulletSample.transform.parent = bulletContainer.transform;
            bullet.gameObject.SetActive(false);
            //bullet.bulletRigidbody.isKinematic = true;
            bullets.Add(bulletSample);
        }

        bombs = new List<Bomb>();
        while (bombs.Count != bombCount)
        {
            var bombSample = Instantiate(bomb, bombContainer.transform.position, Quaternion.identity);
            bombSample.transform.parent = bombContainer.transform;
            bomb.gameObject.SetActive(false);
            //bomb.bombRigidbody.isKinematic = true;
            bombs.Add(bombSample);
        }
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    void Update()
    {      
        PlayerMovement();
        PlayerLook();

        if (Input.GetKeyDown(KeyCode.Mouse0) && ammoCount > 0)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isBombReload)
        {
            BombThrow();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravitation.y = Mathf.Sqrt(6 * -2 * gravity);
        }
    }
        
    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        shiftedSpeed = 0;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftedSpeed = playerSpeed;
        }

        isGrounded = Physics.CheckSphere(groundDetector.position, 0.7f, groundMask);
        
        moveDirection = transform.right*x + transform.forward*z;
        gravitation.y += gravity * Time.deltaTime; 
        
        charControl.Move(moveDirection * (playerSpeed + shiftedSpeed) * Time.deltaTime);

        if (isGrounded)
        {
            gravitation.y = 0;
        }
        else
        {
            charControl.Move(gravitation * Time.deltaTime);
        }   
    }

    private void PlayerLook()
    {        
        var mouseLookX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        var mouseLookY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                
        xRotation -= mouseLookY; 
        xRotation = Mathf.Clamp(xRotation, -45f, 45f);
        transform.Rotate(0f, mouseLookX, 0f);       
        head.localRotation = Quaternion.Euler(xRotation, 0, 0);

        rightArm.localRotation = head.localRotation;
    }

    private void Shoot()
    {
        var bullet = bullets[0];
        bullets.Remove(bullet);
        //bullet.bulletRigidbody.isKinematic = false;
        bullet.transform.position = bulletStartPos.transform.position;
        bullet.transform.rotation = rightArm.rotation;
        bullet.transform.parent = null;
        bullet.gameObject.SetActive(true);
        bullet.bulletRigidbody.velocity = Vector3.zero;
        bullet.bulletRigidbody.AddForce(rightArm.forward*shootForce, ForceMode.Impulse);
        ammoCount--;
        StartCoroutine(BulletLifeTime(bullet));
    }

    private void BombThrow()
    {
        var bomb = bombs[0];
        bombs.Remove(bomb);
        //bomb.bombRigidbody.isKinematic = false;
        bomb.transform.position = bombStartPos.transform.position;
        bomb.transform.rotation = bombStartPos.transform.rotation;
        bomb.transform.parent = null;
        bomb.gameObject.SetActive(true);
        bomb.bombRigidbody.AddForce(bombStartPos.transform.forward * throwForce, ForceMode.Impulse);
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
        //b.bulletRigidbody.isKinematic = true;
        b.bulletRigidbody.velocity = Vector3.zero;
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
        //b.bombRigidbody.isKinematic = true;
        b.bombRigidbody.velocity = Vector3.zero;
        b.gameObject.SetActive(false);

        yield break;
    }
}

