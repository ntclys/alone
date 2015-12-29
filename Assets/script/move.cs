using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class move : MonoBehaviour {

    public Animator anim;
    public Rigidbody rbody;

    public Transform target;

    private Vector3 moveDeirection = Vector3.zero;
    private CharacterController crController;
    private bool isGraounded = false;

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

        isAttackChk = false;
        attackMoveOnece = false;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && isAttackChk == false)
        {
            isAttackChk = true;
            attackMoveOnece = true;
            anim.SetBool("isRun", false);
            anim.SetBool("isAttack", true);
            //Invoke("endAttackAni", 0.1f);  //일정시간후 관련함수 호출
        }
    }

    private void FixedUpdate()
    {

        //Debug.Log("test:" + Input.GetAxis("Horizontal"));

        if (isAttackChk == false)
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
            if (attackMoveOnece == true)
            {
                //Debug.Log("test:" + Time.deltaTime);
                Invoke("playerAttackMoveOnce", 0.2f);

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

    public void aniParameterFalse(string clip)
    {
        anim.SetBool(clip, false);
        isAttackChk = false;
    }
}
