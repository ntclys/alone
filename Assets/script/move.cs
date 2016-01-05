using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class move : MonoBehaviour {

    public Animator anim;
    private AnimatorStateInfo currentPlayereAni;
    public Rigidbody rbody;
    public GameObject playerWeapon;
    private Collider playerWeaponCol;

    public Transform target;

    private Vector3 moveDeirection = Vector3.zero;
    private CharacterController crController;

    public float walkSpeed = 8.0f;
    public float jumpSpeed = 3.5f;
    public float turnSpeed = 10.0f;
    public float gravity = 10.0f;

    private bool isAttackChk;
    private bool attackMoveOnece;

    private float inputH;
    private float inputV;


    enum PlayerState
    {
        Idle,
        Move
    };
    // Use this for initialization
    void Start()
    {
        crController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
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
    }

    private void FixedUpdate()
    {

        //Debug.Log("test:" + Input.GetAxis("Horizontal"));

        if ( currentPlayereAni.IsName("Attack") == false)
        {
            moveDeirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + Input.GetAxis("Horizontal") * turnSpeed, transform.eulerAngles.z);

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                anim.SetBool("isRun", true);
            }
            else
            {
                anim.SetBool("isRun", false);
            }

            moveDeirection = transform.TransformDirection(moveDeirection);
            moveDeirection.y -= gravity * Time.deltaTime * 6;
            moveDeirection *= Time.deltaTime * walkSpeed;
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
                //Invoke("playerAttackMoveOnce", 0.2f);
   
                attackMoveOnece = false;

               // Debug.Log("pos" + moveDeirection);
            }

           moveDeirection = new Vector3(0, -0.2f, 0);
        }

        crController.Move(moveDeirection);

        /*
      inputH = Input.GetAxis("Horizontal");
      inputV = Input.GetAxis("Vertical");

      if (inputH != 0 || inputV != 0)
      {
          anim.SetBool("isRun", true);
      }
      else
      {
          anim.SetBool("isRun", false);
      }

      float moveX = inputH * walkSpeed * Time.deltaTime;
      float moveZ = inputV * walkSpeed * Time.deltaTime;

      rbody.velocity = new Vector3(moveX, 0f, moveZ);
     */
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
        this.transform.position += transform.forward * 1.2f;
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
