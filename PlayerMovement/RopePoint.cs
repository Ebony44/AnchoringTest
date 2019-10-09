using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePoint : MonoBehaviour {

    public Rigidbody2D RopePointRb { get; set; }
    private SpriteRenderer mRopePointRenderer;

    //[SerializeField] private DistanceJoint2D ropeJoint;
    // player's members.. hmm it's dirty
    [SerializeField] private RopeSystemPrac mRopeSystemPrac;
    //[SerializeField] GameObject RopeSystemPrac;
    private DistanceJoint2D mRopeJoint;
    [SerializeField] private Vector2 mPlayerPos;
    private LineRenderer mLineRenderer;

    public bool BAttachedMovingObject { get; set; }
    // for RopeSystemPrac...
    public GameObject ObstacleObject;

    // Setting For animator
    //[Header("Header")]
    //[SerializeField] private Animator mAnim;

    //

    // Use this for initialization
    void Awake () {
        RopePointRb = GetComponent<Rigidbody2D>();
        mRopePointRenderer = GetComponent<SpriteRenderer>();
        //mRopeSystemPrac = FindObjectOfType<RopeSystemPrac>();
        BAttachedMovingObject = false;

        //mAnim = GetComponent<Animator>();
        
	}

    void OnCollisionEnter2D(Collision2D col)
    {
         if (BAttachedMovingObject || col.gameObject.layer == 8)
        {
            
            return;
        }
        ObstacleObject = col.gameObject;
        Debug.Log("ropePoint Collide!");
        mRopeSystemPrac.BRopeAttached = true;
        mRopeSystemPrac.BRopeFiring = false;

        //mRopeJoint = RopeSystemPrac.mRopeJoint;
        //mPlayerPos = RopeSystemPrac.mPlayerPos;
        mRopeJoint = mRopeSystemPrac.GetComponent<DistanceJoint2D>();
        mPlayerPos = mRopeSystemPrac.GetComponent<Transform>().position;
        mLineRenderer = mRopeSystemPrac.GetComponent<LineRenderer>();


        // is it dirty...?.. i guess
        // instantly make player hop to air for grappling.
        mRopeSystemPrac.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
        // dynamic to kinematic for free from gravity.
        RopePointRb.velocity = new Vector2(0, 0);
        RopePointRb.bodyType = RigidbodyType2D.Kinematic;
        

        // connect player's joint to this rigidbody...

        mRopeJoint.connectedBody = RopePointRb;
        
        // set distance between player and anchored position.
        mRopeJoint.distance = Vector2.Distance(mPlayerPos, transform.position);
        if (!mLineRenderer.enabled)
        {
            mLineRenderer.enabled = true;
        }
        mRopeJoint.enabled = true;


        // Add a position to List of Vector2 in RopeSystem.cs ..for once.
        mRopeSystemPrac.RopePointPositions.Add(transform.position);

        if (col.gameObject.layer == 15)
        {
            //mRopePointRb.velocity = new Vector2(-col.transform.position.x + 3.5f, col.transform.position.y);
            //mRopePointRb.velocity = new Vector2(-col.transform.position.x + 3.5f, col.transform.position.y);
            mRopeSystemPrac.BMoving = true;
            var colliderRb = col.gameObject.GetComponent<Rigidbody2D>();
            RopePointRb.velocity = colliderRb.velocity;
            return;
        }



        //ropePositions.Add(hit.point);
        //ropeJoint.distance = Vector2.Distance(playerPosition, hit.point); // distance from player to rope point
        //ropeJoint.enabled = true;
        //ropeHingeAnchorSprite.enabled = true;

    }
    


    // Update is called once per frame
    void Update () {
         // mRopeSystemPrac.RopePointProjectilePos = transform.position;

        if (mRopeSystemPrac.BRopeFiring)
        {
            //mAnim.Play("GrapplingHookSpread");
            //mAnim.CrossFade("GrapplingHookSpread", 0.5f);
        }

	}

    public void ResetRopeJoint()
    {

    }

}
