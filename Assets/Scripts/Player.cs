using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Vector3 moveDirection;
    private float gravity = -19.81f;
    public Vector3 gravitation;
    public LayerMask groundMask;
    public bool isGrounded;
    private int playerVelosity;
    private Vector3 oldPos;
    private Vector3 newPos;
    [SerializeField] private int jumpForse;
    [SerializeField] private int ammoCount;
    [SerializeField] private Animator playerAnimator;
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
    [SerializeField] private Transform bombStartPos;
    public List<Bomb> bombs;
    private bool isBombReload;

    [SerializeField] private Bullet bullet;
    [SerializeField] private int bulletCount;
    [SerializeField] private int shootForce;
    [SerializeField] private int shootDamage;
    [SerializeField] private GameObject bulletStartPos;
    [SerializeField] private GameObject bulletContainer;
    private List<Bullet> bullets;

    private bool isShooting;
    private bool isOnReloading;
    public bool isWinner = false;

    private float xRotation = 0f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //newPos = oldPos = transform.position;

        isGrounded = false;
        bullets = new List<Bullet>();
        while (bullets.Count != bulletCount)
        {
            var bulletSample = Instantiate(bullet, bulletContainer.transform.position, Quaternion.identity);
            bulletSample.transform.parent = bulletContainer.transform;
            bulletSample.gameObject.SetActive(false);
            bullets.Add(bulletSample);
        }

        BombCountFiller();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (isWinner) return;

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

        oldPos = transform.position;
        isGrounded = Physics.CheckSphere(groundDetector.position, 0.3f, groundMask);

        shiftedSpeed = 0;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftedSpeed = playerSpeed;
        }

        moveDirection = transform.right*x + transform.forward*z;
        
        if (isGrounded)
        {
            gravitation.y = -2f;
        }
        
        if (Input.GetButton("Jump") && isGrounded)
        {
            gravitation.y = Mathf.Sqrt(jumpForse * -2 * gravity);
        }

        gravitation.y += gravity * Time.deltaTime; 

        charControl.Move(moveDirection * (playerSpeed + shiftedSpeed) * Time.deltaTime);
        charControl.Move(gravitation * Time.deltaTime);

        newPos = transform.position;
        playerVelosity = (int)(Vector3.Magnitude(newPos - oldPos) / Time.deltaTime);

        if (playerVelosity != 0)
        {
            playerAnimator.SetBool("IsWalking",true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
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

    private void BombCountFiller()
    {
        bombs = new List<Bomb>();
        while (bombs.Count != bombCount)
        {
            var bombSample = Instantiate(bomb, bombStartPos.position, Quaternion.identity);
            bombSample.transform.parent = bombStartPos;
            bombSample.gameObject.SetActive(false);
            bombs.Add(bombSample);
        }
    }

    private void BombThrow()
    {
        var bomb = bombs[0];
        bombs.Remove(bomb);
        bomb.Throw(bomb, throwForce, bombStartPos);
        StartCoroutine(ThrowReloader());
    }
    private IEnumerator ThrowReloader()
    {
        isBombReload = true;
        yield return new WaitForSeconds(3);
        isBombReload = false;
        yield break;
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
        bullet.bulletRigidbody.AddForce(rightArm.forward*shootForce, ForceMode.Impulse);
        ammoCount--;
        playerAnimator.SetTrigger("Shoot");
        StartCoroutine(BulletLifeTime(bullet));
        yield return new WaitForSeconds(0.1f);
        isOnReloading = false;
        isShooting = false;

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
    
}

