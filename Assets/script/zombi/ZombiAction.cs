using UnityEngine;
using System.Collections;

public class ZombiAction : MonoBehaviour {

    private ZombiManager zManager;
    private Animator anim;
    private Rigidbody rbody;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        zManager = GetComponent<ZombiManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void attackPlayer(bool isAttack)
    {
        anim.SetBool("isZombiAttack", isAttack);
    }
}
