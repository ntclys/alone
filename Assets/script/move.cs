using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class move : MonoBehaviour {

    private Animator anim;
    private AnimatorStateInfo currentPlayereAni;
    private Rigidbody rbody;
    private Collider playerWeaponCol;
    private CharacterController charController;
    //[SerializeField] float m_GroundCheckDistance = 0.1f;

    public Transform target;
    public GameObject playerWeapon;


    public float walkSpeed;
    public float jumpPower = 3.5f;
    public float turnSpeed = 10.0f;
    public float gravity = 10.0f;
    private float offsetAngles = 0.0f;
    private float currentRotation = 0.0f;
    private float courseNum = 0.0f;

    private bool isAttackChk;
    private bool attackMoveOnece;


    private Vector3 movePlayer;
    // the world-relative desired move direction, calculated from the camForward and user input.

    private Transform cam; // A reference to the main camera in the scenes transform


    enum PlayerState
    {
        Idle,
        Move
    };

    private void Awake()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Ball needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use world-relative controls in this case, which may not be what the user wants, but hey, we warned them!
        }
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        charController = GetComponent<CharacterController>();
        playerWeaponCol = playerWeapon.GetComponent<Collider>();

        isAttackChk = false;
        attackMoveOnece = false;
    }

    private void Update()
    {
        currentPlayereAni = anim.GetCurrentAnimatorStateInfo(0);

        if (Input.GetMouseButtonDown(0) && currentPlayereAni.IsName("Attack") == false && charController.isGrounded)
        {
            isAttackChk = true;
            attackMoveOnece = true;
            anim.SetTrigger("Attack");
            anim.SetBool("isRun", false);
            playerWeaponCol.enabled = true;

            //currentRotation = target.transform.eulerAngles.y;

            //Invoke("endAttackAni", 0.1f);  //일정시간후 관련함수 호출
        }

        if (Input.GetButtonDown("Jump"))
        {
            HandleGroundedMovement(false, true);
        }
    }

    private void FixedUpdate()
    {

        if (charController.isGrounded == true)
        {
            if (currentPlayereAni.IsName("Attack") == false)
            {
                if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                {
                    anim.SetBool("isRun", true);
                    //transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg, transform.eulerAngles.z);
                    //rbody.AddForce(this.transform.forward * walkSpeed *Time.deltaTime, ForceMode.Impulse);



                    if (CrossPlatformInputManager.GetAxis("Vertical") > .1)
                    {

                        offsetAngles = 0;

                        if (CrossPlatformInputManager.GetAxis("Horizontal") > .1)
                        {
                            offsetAngles += 45;
                        }
                        else if (CrossPlatformInputManager.GetAxis("Horizontal") < -.1)
                        {
                            offsetAngles -= 45;
                        }
                        movePlayer = new Vector3(0, movePlayer.y, CrossPlatformInputManager.GetAxis("Vertical"));
                    }
                    else if (CrossPlatformInputManager.GetAxis("Vertical") < -.1)
                    {

                        offsetAngles = 180;

                        if (CrossPlatformInputManager.GetAxis("Horizontal") > .1)
                        {
                            offsetAngles -= 45;
                        }
                        else if (CrossPlatformInputManager.GetAxis("Horizontal") < -.1)
                        {
                            offsetAngles += 45;
                        }

                        movePlayer = new Vector3(0, movePlayer.y, -CrossPlatformInputManager.GetAxis("Vertical"));
                    }
                    else if (CrossPlatformInputManager.GetAxis("Horizontal") > .1)
                    {
                        offsetAngles = 90;

                        if (CrossPlatformInputManager.GetAxis("Vertical") > .1)
                        {
                            offsetAngles += 45;
                        }
                        else if (CrossPlatformInputManager.GetAxis("Vertical") < -.1)
                        {
                            offsetAngles -= 45;
                        }

                        movePlayer = new Vector3(0, movePlayer.y, CrossPlatformInputManager.GetAxis("Horizontal"));
                    }
                    else if (CrossPlatformInputManager.GetAxis("Horizontal") < -.1)
                    {

                        offsetAngles = 270;

                        if (CrossPlatformInputManager.GetAxis("Vertical") > .1)
                        {
                            offsetAngles += 45;
                        }
                        else if (CrossPlatformInputManager.GetAxis("Vertical") < -.1)
                        {
                            offsetAngles -= 45;
                        }

                        movePlayer = new Vector3(0, movePlayer.y, -CrossPlatformInputManager.GetAxis("Horizontal"));
                    }

                    transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y + offsetAngles, 0);
                }
                else
                {
                    anim.SetBool("isRun", false);
                    rbody.velocity = new Vector3(0, rbody.velocity.y, 0);
                }               

            }
            else
            {
                currentPlayereAni = anim.GetCurrentAnimatorStateInfo(0);

                if (currentPlayereAni.normalizedTime > 0.5f)
                {
                    playerWeaponCol.enabled = false;
                }


                if (attackMoveOnece == true)
                {
                    Invoke("playerAttackMoveOnce", 0.2f);
                    attackMoveOnece = false;
                }
            }

            movePlayer = transform.TransformDirection(movePlayer);
            movePlayer.y -= gravity * Time.deltaTime;
            charController.Move(movePlayer * (Time.deltaTime * walkSpeed));
        }
        else
        {


            if (Input.GetAxis("Vertical") > .1)
            {
                offsetAngles = 0;

                if (Input.GetAxis("Horizontal") > .1)
                {
                    offsetAngles += 45;
                }
                else if (Input.GetAxis("Horizontal") < -.1)
                {
                    offsetAngles -= 45;
                }

                currentRotation += offsetAngles;
                courseNum = Input.GetAxis("Vertical");
            }
            else if (Input.GetAxis("Vertical") < -.1)
            {
                offsetAngles = 180;

                if (Input.GetAxis("Horizontal") > .1)
                {
                    offsetAngles -= 45;
                }
                else if (Input.GetAxis("Horizontal") < -.1)
                {
                    offsetAngles += 45;
                }

                currentRotation +=  offsetAngles;
                courseNum = -Input.GetAxis("Vertical");
            }
            else if (Input.GetAxis("Horizontal") > .1)
            {
                currentRotation  += 90;
                courseNum = movePlayer.z + Input.GetAxis("Horizontal");
            }
            else if (Input.GetAxis("Horizontal") < -.1)
            {
                currentRotation += 270;
                courseNum = -Input.GetAxis("Horizontal");
            }
            else
            {
                movePlayer.y += 0.03f;
            }

            transform.eulerAngles = new Vector3(0, currentRotation, 0);
            movePlayer = new Vector3(0, movePlayer.y, courseNum);

        }

        movePlayer = transform.TransformDirection(movePlayer);
        movePlayer.y -= gravity * Time.deltaTime;
        charController.Move(movePlayer * (Time.deltaTime * walkSpeed));
    }


    void endAttackAni()
    {

        if (isAttackChk == true)
        {
            //Debug.Log("end");
            anim.SetBool("isAttack", false);
            isAttackChk = false;
        }
    }

    private void playerAttackMoveOnce()
    {
        rbody.AddForce(this.transform.forward * walkSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    void HandleGroundedMovement(bool dodge, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !dodge)
        {
            // jump!
            Debug.Log("jump");
            
            if (charController.isGrounded )
            {
                anim.SetTrigger("jump");
                anim.applyRootMotion = false;
                currentRotation = cam.transform.eulerAngles.y;
                //rbody.velocity = new Vector3(rbody.velocity.x, jumpPower, rbody.velocity.z);
                movePlayer.y = jumpPower;
            }
            
        }
    }



    /*
    //애니메이션 프레임에서 이벤트 호출 함수
    public void aniParameterFalse(string clip)
    {
        anim.SetBool(clip, false);
        isAttackChk = false;
    }
    */
}
