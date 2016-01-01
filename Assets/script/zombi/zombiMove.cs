using UnityEngine;
using System.Collections;

public class zombiMove : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private ZombiAction zAction;
    private ZombiManager zManager;

    public float walkSpeed;
    public GameObject player;

    private Vector3 movement;
    private bool isRunChk;

    public float attackRange;

    public enum State
    {
        Idel,
        Move,
        Attack,
        Hit,
        Die
    };


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        zAction = GetComponent<ZombiAction>();
        zManager = GetComponent<ZombiManager>();

        attackRange = 2.7f;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((int)iStudentState) );   // int 형으로 형변환 -> 결과 : 0 이 출력
        //Debug.Log(((iStudentState)3).ToString()); // int 형을 enum으로 형변환 -> 결과 : eGoToSchool 이 출력
    }

    private void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
        Vector3 offsetPos = this.transform.position - player.transform.position;
        offsetPos.y = 0;

        State isZombieState = State.Idel;

        if (offsetPos.magnitude > attackRange && isZombieState == State.Idel )
        {
            
            anim.SetBool("isWalk", true);
            zAction.attackPlayer(false);

            movement = transform.forward;
            movement = movement.normalized * walkSpeed * Time.deltaTime;

            rbody.MovePosition(transform.position + movement);
        }
        else
        {
            anim.SetBool("isWalk", false);
            zAction.attackPlayer(true);
        }
        

        //rbody.velocity = transform.position + movement;
    }
       
}
