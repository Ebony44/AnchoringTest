using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAnimationScript : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.CrossFade("GrapplingHookSpread", 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.CrossFade("Idle", 0.5f);
        }
    }
}
