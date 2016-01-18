using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class move : MonoBehaviour {

    private Animator anim;
    private AnimatorStateInfo currentPlayereAni;
    private Rigidbody rbody;
    private Collider playerWeaponCol;
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    [SerializeField] float m_GroundCheckDistance = 0.1f;

    public Transform target;
    public GameObject playerWeapon;

    Vector3 m_GroundNormal;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;

    public float walkSpeed;
    public float jumpPower = 3.5f;
    public float turnSpeed = 10.0f;
    public float gravity = 10.0f;

    private bool isAttackChk;
    private bool attackMoveOnece;

    private bool m_Jump;

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
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        playerWeaponCol = playerWeapon.GetComponent<Collider>();

        isAttackChk = false;
        attackMoveOnece = false;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
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

        //Debug.Log("test:" + Input.GetAxis("Horizontal"));

        CheckGroundStatus();

        if ( currentPlayereAni.IsName("Attack") == false)
        {

            //rbody.velocity = Vector3.ProjectOnPlane(rbody.velocity, m_GroundNormal);

            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                anim.SetBool("isRun", true);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg, transform.eulerAngles.z);
                rbody.AddForce(this.transform.forward * walkSpeed *Time.deltaTime, ForceMode.Impulse);
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
                //Invoke("playerAttackMoveOnce", 0.2f);
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
        //rbody.AddForce(this.transform.forward * walkSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    void HandleGroundedMovement(bool dodge, bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && !dodge)
        {
            // jump!
            Debug.Log("jump");
            anim.SetFloat("Jump", rbody.velocity.y);
            if (m_Jump == false)
            {
                m_Jump = true;
                anim.SetTrigger("jump");
                anim.applyRootMotion = false;
                rbody.velocity = new Vector3(rbody.velocity.x, jumpPower, rbody.velocity.z);
            }
            
            //rbody = false;
            //anim.applyRootMotion = false;
            //m_GroundCheckDistance = 0.1f;
        }
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            Debug.Log("jump_onGround"+hitInfo.distance);
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            anim.applyRootMotion = true;
            m_Jump = false;
            anim.SetBool("isJump", false);
            anim.SetTrigger("land");
        }
        else
        {
            Debug.Log("jump_onAir:"+hitInfo.distance);
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            anim.applyRootMotion = false;
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
