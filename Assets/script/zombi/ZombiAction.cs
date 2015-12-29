using UnityEngine;
using System.Collections;

public class ZombiAction : MonoBehaviour {

    private ZombiManager zManager;
    private Animator anim;
    private Rigidbody rbody;
    private Collider col;

    bool isZombiHit = false;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        zManager = GetComponent<ZombiManager>();
        col = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void attackPlayer(bool isAttack)
    {
        if (isZombiHit != false)
        {
            anim.SetBool("isZombiAttack", isAttack);
        }
        
    }

    void OnTriggerEnter(Collider Get)
    {
        if(Get.gameObject.tag =="Weapon")
        {

            isZombiHit = true;
            damageCheck();

        }
    }


    void damageCheck()
    {
        zManager.helthPoint -= 20;

        if(zManager.helthPoint > 0)
        {
            Debug.Log("hit");
            anim.SetBool("isZombiAttack", false);
            anim.SetBool("hit", true);
        }
        else
        {
            col.isTrigger = true;
           // Destroy(this.gameObject);
        }
    }
}
