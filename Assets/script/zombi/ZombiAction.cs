using UnityEngine;
using System.Collections;

public class ZombiAction : MonoBehaviour {

    private ZombiManager zManager;
    private Animator anim;
    private Collider col;
    private zombiMove zMove;

    private bool isZombiHit = false;


    public bool Hited
    {
        get { return isZombiHit; }
        set { isZombiHit = value; }
    }
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        zManager = GetComponent<ZombiManager>();
        col = GetComponent<Collider>();
        zMove = GetComponent<zombiMove>();
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
        if (isZombiHit == false)
        {
            anim.SetBool("isZombiAttack", isAttack);
        }
    }

    void OnTriggerEnter(Collider Get)
    {
        if(Get.gameObject.tag =="Weapon")
        {
           // Debug.Log("hit");
            isZombiHit = true;
            zMove.Worked = true;
            damageCheck();
        }
    }


    void damageCheck()
    {
        zManager.helthPoint -= 20;

        if(zManager.helthPoint > 0)
        {
            anim.SetBool("isZombiAttack", false);
            anim.SetBool("hit", true);
        }
        else
        {
            Debug.Log(zMove.Died);
            zMove.Died = true;
            anim.SetBool("isZombiAttack", false);
            anim.SetBool("die", true);

            //yield return new WaitForSeconds(1.0f);

            col.isTrigger = true;
            // Destroy(this.gameObject);
        }
    }
}
