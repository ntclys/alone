﻿using UnityEngine;
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

    private bool isAttackChk;
    private bool attackMoveOnece;


    [SerializeField] private bool m_Jump;
    [SerializeField] private float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.


    private Vector3 movePlayer;
    // the world-relative desired move direction, calculated from the camForward and user input.

    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera
    private bool jump; // whether the jump button is currently pressed


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

        GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
        playerWeaponCol = playerWeapon.GetComponent<Collider>();

        isAttackChk = false;
        attackMoveOnece = false;
    }

    private void Update()
    {
        currentPlayereAni = anim.GetCurrentAnimatorStateInfo(0);

        if (Input.GetMouseButtonDown(0) && currentPlayereAni.IsName("Attack") == false)
        {
            isAttackChk = true;
            attackMoveOnece = true;
            anim.SetTrigger("Attack");
            anim.SetBool("isRun", false);
            playerWeaponCol.enabled = true;

            //Invoke("endAttackAni", 0.1f);  //일정시간후 관련함수 호출
        }

        if (Input.GetButtonDown("Jump"))
        {
            HandleGroundedMovement(false, true);
        }
    }

    private void FixedUpdate()
    {

        if ( currentPlayereAni.IsName("Attack") == false)
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

                // calculate move direction
                /*
                if (cam != null)
                {
                    // calculate camera relative direction to move:
                    camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
                    movePlayer = (v * camForward + h * cam.right).normalized;
                }
                else
                {
                    // we use world-relative directions in the case of no main camera
                    movePlayer = (v * Vector3.forward + h * Vector3.right).normalized;
                }
                */
                transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y + offsetAngles, 0);
                //transform.eulerAngles = new Vector3(0, offsetAngles, 0);

                movePlayer = transform.TransformDirection(movePlayer);
                rbody.velocity += movePlayer * Time.deltaTime *walkSpeed ; 
                //rbody.AddForce(movePlayer * walkSpeed);

            }
            else
            {
                anim.SetBool("isRun", false);
                rbody.velocity = new Vector3(0, rbody.velocity.y, 0 );
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
            
            if (m_Jump == false)
            {
                m_Jump = true;
                anim.SetTrigger("jump");
                anim.applyRootMotion = false;
                rbody.velocity = new Vector3(rbody.velocity.x, jumpPower, rbody.velocity.z);
            }
            
        }
    }


    void OnCollisionStay()
    {
        if(m_Jump)
        {
            m_Jump = false;
            anim.SetBool("isJump", false);
            //anim.SetTrigger("land");
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
