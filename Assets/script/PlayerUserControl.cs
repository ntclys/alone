using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerUserControl : MonoBehaviour {

    private playerCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
    private Vector3 m_Move;
    private bool m_Jump;                 // the world-relative desired move direction, calculated from the camForward and user input.


    private void Start()
    {
        m_Character = GetComponent<playerCharacter>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool dodge = Input.GetKey(KeyCode.LeftShift);

        m_Move = v * Vector3.forward + h * Vector3.right;
/*
#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif
*/
        // pass all parameters to the character control script
        m_Character.Move(m_Move, dodge, m_Jump);
        m_Jump = false;
    }
}
