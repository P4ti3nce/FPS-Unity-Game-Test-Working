using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public CharacterController charController;
    public float speed = 7f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    

    [Header("Player Camera Settings")]
    public float turnSmoothing = 0.05f;
    public Transform cam;
    float turnSmoothingVelocity;
    [Header("Ground Checking")]
    public Transform groundCheck;
    public float groundDisttance = 0.4f;
    public LayerMask groundMask;

    [Header("Gun and bullet")]
    public GameObject bulletPrefab;
    public Transform barrelTransform;
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletMissDistance = 25;

    [Header("Action Controller")]
    [SerializeField]
    private PlayerInput playerInput;
    private InputAction shootAction;
    private InputAction moveAction;

    [Header("Animator")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float animationSmoothTime = 0.01f;
    [SerializeField]
    private float animationPlayerTransition = 0.15f;

    [Header("Aiming")]
    [SerializeField]
    private Transform aimTarget;
    [SerializeField]
    private float aimDistance = 1f;

    [Header("Scene Setup")]
    public GameObject canvas;

    int jumpAnimation;
    int recoilAnimation;
    Vector2 currentAnimationBlend;
    Vector2 animationVelcoity;
    float currentGravStore;

    Vector3 velocity;
    bool isGround;
    // Start is called before the first frame update
    void Awake()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        shootAction = playerInput.actions["Shoot"];
        moveAction = playerInput.actions["Move"];
        jumpAnimation = Animator.StringToHash("Pistol Jump");
        recoilAnimation = Animator.StringToHash("PistolShootRecoil");
        currentGravStore = gravity;

    }
    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }
    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }
    private void ShootGun()
    {
        //shoot a ray forward until it hits point aimed at
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cam.position, cam.forward, out hit, Mathf.Infinity))
        {
            //make bullets and project them in direction calculated 

            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cam.position + cam.forward * bulletMissDistance;
            bulletController.hit = false;
        }
        animator.CrossFade(recoilAnimation, animationPlayerTransition);
    }
    // Update is called once per frame
    void Update()
    {
        aimTarget.position = cam.position + cam.forward * aimDistance;
        //checks the ground to see if player is on the ground (check sphere creates invisible circle to see if it connects with ground)
        isGround = Physics.CheckSphere(groundCheck.position, groundDisttance, groundMask);
        //if on ground dont increase acceleration
        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlend = Vector2.SmoothDamp(currentAnimationBlend, input, ref animationVelcoity, animationSmoothTime);
        if (isGround&&velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //Jumping (using physics equation to calculate velocity needed to jump certain height)
        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.CrossFade(jumpAnimation, animationPlayerTransition);
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            canvas.SetActive(true);
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        charController.Move(velocity * Time.deltaTime);
        // -1 or 1 depinding on if you press left arrow, right arrow (Also works for A and D)
        float horizontal = Input.GetAxisRaw("Horizontal");
        // same principle but with up and down arrow (or W and S)
        float vertical = Input.GetAxisRaw("Vertical");
        //dont want movement on y Axis (normalise to stop extra speeed when pressing 2 keys)
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //changing animation based on direction
        animator.SetFloat("MoveX", currentAnimationBlend.x);
        animator.SetFloat("MoveZ", currentAnimationBlend.y);
        if (direction.magnitude >= 0.1f)
        {
            //return angle between x axis and vector starting at 0 and terminating a x,y (finding direciton player should point in)
            float targetOrientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //Updated move system


            //smoothing the turning so it doesnt snap
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetOrientation, ref turnSmoothingVelocity, turnSmoothing);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetOrientation, 0f) * Vector3.forward;
            //Vector3 moveDirection = new Vector3(input.x,0,input.y);
            charController.Move(moveDirection * speed * Time.deltaTime);
            
        }
        
        
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Here");
        if (collision.gameObject.CompareTag("LowGrav")) {
            gravity = -5;
            jumpHeight = 8;
        };
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("LowGrav"))
        {
            gravity = currentGravStore;
            jumpHeight = 3;
        };
    }
}
