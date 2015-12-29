using UnityEngine;
using System.Collections;

public class zombiMove : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rbody;
    private ZombiAction zAction;

    public float walkSpeed;
    public GameObject player;

    private Vector3 movement;
    private bool isRunChk;

    public float attackRange;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        zAction = GetComponent<ZombiAction>();

        attackRange = 2.7f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.LookAt(player.transform.position);
        Vector3 offsetPos = this.transform.position - player.transform.position;
        offsetPos.y = 0;


        if (offsetPos.magnitude > attackRange)
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
