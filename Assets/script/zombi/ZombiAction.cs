using UnityEngine;
using System.Collections;

public class ZombiAction : MonoBehaviour {

    private ZombiManager zManager;
    private Animator anim;
    private Collider col;
    private zombiMove zMove;
    private Rigidbody rbody;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        zManager = GetComponent<ZombiManager>();
        col = GetComponent<Collider>();
        zMove = GetComponent<zombiMove>();
        rbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void FixedUpdate()
    {

    }

    public void attackPlayer(bool isAttack)
    {
        if (zMove.Worked == false)
        {
            anim.SetBool("isZombiAttack", isAttack);
        }
    }
    
    void OnTriggerEnter(Collider Get)
    {
        if(Get.gameObject.tag =="Weapon")
        {
            //Debug.Log("hit");
            zMove.Worked = true;
            damageCheck();
        }
    }
   /*
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Weapon")
        {
             Debug.Log("hit");
            zMove.Worked = true;
            damageCheck();
        }
    }
    */

    void damageCheck()
    {
        zManager.helthPoint -= 20;

        if(zManager.helthPoint > 0)
        {
            //Debug.Log("hit");
            //anim.StopPlayback();
            //anim.SetBool("hit", true);
           // anim.SetBool("isZombiAttack", false);
            anim.SetTrigger("Hit");
        }
        else
        {
            //Debug.Log(zMove.Died);
            if (zMove.Died == false)
            {
                zMove.Died = true;
                //anim.SetBool("isZombiAttack", false);
                //anim.SetBool("die", true);
                anim.SetTrigger("Die");

                rbody.isKinematic = true;   //좀비 다이시 고정후 캐릭터와 충돌 안하게
                col.isTrigger = true;

                //yield return new WaitForSeconds(1.0f);
                Invoke("zombieDie", 3.0f);
            }
            
        }
    }


    void zombieDie()
    {
        rbody.isKinematic = false;
        //col.isTrigger = true;
        // Destroy(this.gameObject);
    }
}
