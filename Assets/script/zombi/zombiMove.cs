using UnityEngine;
using System.Collections;

public class zombiMove : MonoBehaviour
{
    private Animator anim;
    private AnimatorStateInfo currentZombieAni;
    private Rigidbody rbody;
    private ZombiAction zAction;
    //private ZombiManager zManager;

    public float walkSpeed;
    public GameObject player;

    private Vector3 movement;
    [SerializeField] private bool isWork = false;
    private bool isDie = false;

    public float attackRange;


    public bool Worked
    {
        get { return isWork; }
        set { isWork = value; }
    }

    public bool Died
    {
        get { return isDie; }
        set { isDie = value; }
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        zAction = GetComponent<ZombiAction>();
        //zManager = GetComponent<ZombiManager>();

        attackRange = 2.7f;
        isWork = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((int)iStudentState) );   // int 형으로 형변환 -> 결과 : 0 이 출력
        //Debug.Log(((iStudentState)3).ToString()); // int 형을 enum으로 형변환 -> 결과 : eGoToSchool 이 출력
    }

    private void FixedUpdate()
    {
        if (isDie == false)
        {
            currentZombieAni = anim.GetCurrentAnimatorStateInfo(0);

            if (isWork)
            {
                if (currentZombieAni.IsName("Hit"))
                {
                    if (currentZombieAni.normalizedTime > 1.0f)
                    {
                        isWork = false;
                        anim.SetBool("hit", false);
                        zAction.attackPlayer(false);
                        zAction.Hited = false;
                    }
                }
                else if (currentZombieAni.IsName("attack"))
                {
                    if (currentZombieAni.normalizedTime > 1.0f)
                    {
                        isWork = false;
                    }
                }
                else if(currentZombieAni.IsName("Walk") ) //활성화 상태인데 걷기모션중일 경우
                {
                    isWork = false;
                }
            }


            Vector3 offsetPos = this.transform.position - player.transform.position;
            offsetPos.y = 0;

            if (offsetPos.magnitude > attackRange) //캐릭터 일정거리까지 접근
            {
                //Debug.Log("isWork:" + isWork);
                if (isWork == false)
                {
                    anim.SetBool("isWalk", true);
                    zAction.attackPlayer(false);

                    transform.LookAt(player.transform.position);
                    movement = transform.forward;
                    movement = movement.normalized * walkSpeed * Time.deltaTime;

                    rbody.MovePosition(transform.position + movement);
                }

            }
            else
            {
                if (isWork == false)
                {
                    //Debug.Log("enum:"+zManager.CurrentZombiState);
                    anim.SetBool("isWalk", false);
                    zAction.attackPlayer(true);
                    isWork = true;
                }
            }
        }
        //rbody.velocity = transform.position + movement;
    }
       
}
