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
    [SerializeField] private CharacterController charControl;
    [SerializeField] private int playerSpeed;
    [SerializeField] private int shiftedSpeed;
    [SerializeField] private int sensitivity;
    [SerializeField] private Transform head;
    [SerializeField] private Transform rightArm;
    [SerializeField] private Transform scope;

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

    private bool isShooting;
    private bool isOnReloading;


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
            bullets.Add(bulletSample);
        }

        bombs = new List<Bomb>();
        while (bombs.Count != bombCount)
        {
            var bombSample = Instantiate(bomb, bombContainer.transform.position, Quaternion.identity);
            bombSample.transform.parent = bombContainer.transform;
            bomb.gameObject.SetActive(false);
            bombs.Add(bombSample);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {      
        PlayerMovement();
        PlayerLook();

        if (Input.GetKey(KeyCode.Mouse0) && ammoCount > 0)
        {
            isShooting = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isBombReload)
        {
            BombThrow();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravitation.y = Mathf.Sqrt(6 * -2 * gravity);
        }

        if (isShooting == true && !isOnReloading)
        {
            StartCoroutine(Shoot());
        }

        ScopeRay();
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
    private void ScopeRay()
    {
       // Ray ray = new Ray(scope.position, transform.forward);
       // Debug.DrawRay(scope.position, scope.forward * 20, Color.red);
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
        bullet.bulletRigidbody.AddForce(rightArm.forward*shootForce, ForceMode.Impulse);
        ammoCount--;
        StartCoroutine(BulletLifeTime(bullet));
        yield return new WaitForSeconds(0.1f);
        isOnReloading = false;
        isShooting = false;

        yield break;
    }

    private void BombThrow()
    {
        var bomb = bombs[0];
        bombs.Remove(bomb);
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
        b.bombRigidbody.velocity = Vector3.zero;
        b.gameObject.SetActive(false);

        yield break;
    }
}

