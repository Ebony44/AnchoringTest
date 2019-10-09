using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.VFX;



public class JetpackSystem : MonoBehaviour
{
    [SerializeField] private PlayerMovementPrac playerMovementPrac;
    [SerializeField] private RopeSystemPrac ropeSystemPrac;
    [SerializeField] private VisualEffect jetpackVisualEffect;
    public float MaxJetpackBoostGauge = 3f;
    public float JetpackBoostGauge = 3f;
    public bool BJetpackReady = false;

    [SerializeField] private float jetpackBoostSpeed = 22f;

    
    // Start is called before the first frame update
    void Awake()
    {
        if (jetpackVisualEffect)
        {
            
            jetpackVisualEffect.Stop();
            jetpackVisualEffect.gameObject.SetActive(true);
        }
        else
        {
            jetpackVisualEffect = FindObjectOfType<JetpackSystem>().GetComponentInChildren<VisualEffect>(true);
            
            jetpackVisualEffect.Stop();
            jetpackVisualEffect.gameObject.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.BGamePaused || LevelManager.BEndConditionMet || LevelManager.BGameWon)
        {
            return;
        }
        if (playerMovementPrac.BOnGround || ropeSystemPrac.BRopeAttached)
        {
            BJetpackReady = true;
            JetpackBoostGauge = MaxJetpackBoostGauge;
        }
        
        
        
    }
    private void FixedUpdate()
    {
        if (LevelManager.BGamePaused || LevelManager.BEndConditionMet || LevelManager.BGameWon)
        {
            return;
        }
        Boost();
    }
    private void Boost()
    {
        if (ropeSystemPrac.BRopeAttached || playerMovementPrac.BOnGround)
        {
            //Debug.Log("not jumping, or rope attached on something.. or player is on ground");
            jetpackVisualEffect.Stop();
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        float jumpInput = Input.GetAxis("Jump");
        if (BJetpackReady && jumpInput > 0f && !playerMovementPrac.BOnGround)
        {
            
            JetpackBoostGauge -= Time.deltaTime;

            // TODO: limit boost to Y axis (too fast)
            // does it work???? ....hmmm
            if (playerMovementPrac.PlayerRb.velocity.y < 7f)
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
