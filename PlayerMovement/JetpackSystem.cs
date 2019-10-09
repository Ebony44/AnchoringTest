using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.VFX;



public class JetpackSystem : MonoBehaviour
{
    [SerializeField] private PlayerMovementPrac playerMovementPrac;
    [SerializeField] private RopeSystemPrac ropeSystemPrac;
    [SerializeField] private VisualEffect jetpackVisualEffect;
    public float MaxJetpackBoostGauge = 2f;
    public float JetpackBoostGauge = 2f;
    public bool BJetpackReady = false;

    [SerializeField] private float jetpackBoostSpeed = 8f;

    
    // Start is called before the first frame update
    void Awake()
    {
        if (jetpackVisualEffect != null)
        {
            jetpackVisualEffect.Stop();
        }
        else
        {
            jetpackVisualEffect.Stop();
            jetpackVisualEffect.gameObject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovementPrac.BOnGround || ropeSystemPrac.BRopeAttached)
        {
            BJetpackReady = true;
            JetpackBoostGauge = MaxJetpackBoostGauge;
        }
        
        
        
    }
    private void FixedUpdate()
    {
        Boost();
    }
    private void Boost()
    {
        if (/*!playerMovementPrac.BJumping ||*/ ropeSystemPrac.BRopeAttached || playerMovementPrac.BOnGround)
        {
            //Debug.Log("not jumping, or rope attached on something.. or player is on ground");
            jetpackVisualEffect.Stop();
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float jumpInput = Input.GetAxis("Jump");
        if (BJetpackReady && jumpInput > 0f && !playerMovementPrac.BOnGround)
        {
            Debug.Log("Jetpack Boost on!");
            JetpackBoostGauge -= Time.deltaTime;

            // TODO: limit boost to Y axis (too fast)
            // does it work???? ....hmmm
            if (playerMovementPrac.PlayerRb.velocity.y < 6f)
            {
                playerMovementPrac.PlayerRb.AddForce(new Vector2(horizontalInput, jumpInput) * jetpackBoostSpeed);
            }
            
            //jetpackVisualEffect.transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f);
            jetpackVisualEffect.Play();
            
            
            if (JetpackBoostGauge <= 0)
            {
                BJetpackReady = false;
                jetpackVisualEffect.Stop();
            }
        }
        if (jumpInput <= 0f)
        {
            jetpackVisualEffect.Stop();
        }


    }
}
