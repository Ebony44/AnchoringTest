using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystemPrac : MonoBehaviour
{

    [Header("Direct Reference")]
    [SerializeField] private RopePoint mRopePointscript;

    private Vector2 mPlayerPos;
    private LineRenderer mLineRenderer;
    [SerializeField] private LayerMask mRopeLayerMask;

    public bool BRopeAttached { get; set; }
    public bool BRopeFiring { get; set; }

    [Header("RopePointVariables")]
    [SerializeField] private GameObject mRopePoint;
    [SerializeField] private Rigidbody2D mRopePointrb;
    [SerializeField] private DistanceJoint2D mRopeJoint;
    public List<Vector2> RopePointPositions = new List<Vector2>();
    private Dictionary<Vector2, int> wrapPointMap = new Dictionary<Vector2, int>();

    [Header("RopePointSpeed")]
    [SerializeField] private float mFiringMultiplier = 20f;
    [SerializeField] private float mMaxRopeDistance = 30f;


    // to check rope attached obstacle is moving...
    // and check it's position to move all ropePointPositions..
    public bool BAttachedObjectMoving { get; set;}
    public Vector2 AttachedObstaclePos;

    // to control swinging ability
    [SerializeField] private PlayerMovementPrac playerMovementPrac;
    
    // to control rappeling Rope
    public float ClimbSpeed = 3f;
    [SerializeField] private bool bColliding;

    // for CooldownTimeManager
    public bool BRopeReady;
    public float CooldownTime;
    public float MaxCooldownTime = 1f;
    

    // Use this for initialization
    void Awake()
    {
        
        // set active status after awake
        mRopePoint.transform.position = transform.position;
        mRopePoint.SetActive(false);
        
        mLineRenderer = GetComponent<LineRenderer>();
        /*
        if (mLineRenderer.enabled == false)
        {
            mLineRenderer.enabled = true;
        }
        */
        /*
        if (mRopePoint.activeSelf == false)
        {
            mRopePoint.SetActive(true);
        }
        */
        BAttachedObjectMoving = false;
        BRopeFiring = false;
        //mRopePointscript = FindObjectOfType<RopePoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.BGamePaused || LevelManager.BGameWon)
        {
            return;
        }
        if (LevelManager.BEndConditionMet)
        {
            mRopePoint.SetActive(false);
            mLineRenderer.enabled = false;
            return;
        }
        if (!BRopeReady)
        {
            CooldownTime += Time.deltaTime;
        }
        if (CooldownTime >= MaxCooldownTime)
        {
            BRopeReady = true;
            CooldownTime = 0f;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPlayerPos = transform.position;
        // update rope status
        if (BRopeFiring || BRopeAttached)
        {
            if (RopePointPositions.Count > 0)
            {

                // 2 of parameter were here.
                var lastRopePos = RopePointPositions.Last();
                int indexLast = RopePointPositions.IndexOf(RopePointPositions.Last());
                RopePointPositions[indexLast] = mRopePoint.transform.position;


                var currentNextHit = Physics2D.Raycast(mPlayerPos, (lastRopePos - mPlayerPos).normalized, Vector2.Distance(mPlayerPos, lastRopePos) - 0.5f, mRopeLayerMask);

                if (currentNextHit)
                {
                    var colliderWithVertices = currentNextHit.collider as PolygonCollider2D;
                    //Collider2D colliderBasic = colliderWithVertices;
                    if (colliderWithVertices != null)
                    {
                        var closestPointToHit = GetClosestColliderPointFromLine(currentNextHit, colliderWithVertices);

                        //Debug.Log("it's vector2 is " + GetClosestColliderPointFromLine(currentNextHit, colliderWithVertices));
                        if (wrapPointMap.ContainsKey(closestPointToHit))
                        {
                            ResetRope();
                            return;
                        }

                        RopePointPositions.Add(closestPointToHit);
                        wrapPointMap.Add(closestPointToHit, 0);
                        if (!mRopePointscript.BAttachedMovingObject)
                        {
                            
                        }
                        

                    }

                }
            }

            mLineRenderer.SetPosition(0, transform.position);
            mLineRenderer.SetPosition(1, mRopePoint.transform.position);
            
            //MEdgeCollider.points[0] = transform.position;
            //MEdgeCollider.points[1] = ropePointProjectilePos;
            


        }
        if (!BRopeAttached)
        {
            playerMovementPrac.bSwinging = false;
        }
        else
        {
            playerMovementPrac.bSwinging = true;
            playerMovementPrac.RopeHook = RopePointPositions.Last();
        }


        
        HandleRopeInput();
        UpdateRopePositions();
        HandleRopeLength();
    }

    private void HandleRopeInput()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (BRopeReady)
        {
            if (Input.GetMouseButtonDown(0) && !BRopeAttached)
            {
                // set rope is NOT ready.
                BRopeReady = false;

                // set active and make it dynamic.
                
                mRopePointrb.bodyType = RigidbodyType2D.Dynamic;
                mRopePoint.transform.position = transform.position;
                mRopePoint.SetActive(true);
                

                float xDistance = mousePos.x - transform.position.x;
                float yDistance = mousePos.y - transform.position.y;

                var aimAngle = Mathf.Atan2(yDistance, xDistance);
                

                if (aimAngle < 0f)
                {
                    aimAngle = aimAngle + (Mathf.PI * 2);
                }
                
                float xVeloc = Mathf.Cos(aimAngle) * mFiringMultiplier;
                float yVeloc = Mathf.Sin(aimAngle) * mFiringMultiplier;


                //mRopePointRb.velocity = new Vector2(xVeloc, yVeloc);
                //mRopePointscript.mRopePointRb.velocity = new Vector2(xVeloc, yVeloc);
                mRopePointrb.velocity = new Vector2(xVeloc, yVeloc);

                BRopeFiring = true;

                // set lineRenderer enabled
                mLineRenderer.enabled = true;

            }
            else if (!BRopeFiring && !BRopeAttached)
            {
                //mLineRenderer.enabled = false;
            }
        }
        

        if (Input.GetMouseButtonDown(1))
        {
            //MbRopeAttached = false;
            //MbRopeFired = false;
            ResetRope();
            
        }
    }
    void ResetRope()
    {
        // disable distanceJoint2D, LineRenderer
        BRopeAttached = false;
        BRopeFiring = false;
        mLineRenderer.positionCount = 2;
        mLineRenderer.SetPosition(0, transform.position);
        mLineRenderer.SetPosition(1, transform.position);

        
        mRopePoint.SetActive(false);

        //mLineRenderer.gameObject.SetActive(false);
        mLineRenderer.enabled = false;

        //mLineRenderer.clear
        RopePointPositions.Clear();


    }

    private Vector2 GetClosestColliderPointFromLine(RaycastHit2D ropeHit, Collider2D collider)
    {
        //var closestVector = collider.ClosestPoint(ropePointHit);
        var closestVectorTell = collider.ClosestPoint(ropeHit.point);
        return closestVectorTell;
    }
    private void UpdateRopePositions()
    {
        if (!BRopeAttached)
        {
            return;
        }

         //RopePointProjectilePos = mRopePoint.transform.position;

        
        // if moving obstacle overwrap the ropePositions..... (excluding last one, which is the present rope point position.)

        AttachedObstaclePos = mRopePointscript.ObstacleObject.transform.position;
        if (AttachedObstaclePos != null)
        {
            for (int i = 0; i < RopePointPositions.Count - 1; ++i)
            {
                //if (transform.position.)
                if (RopePointPositions[i].x < AttachedObstaclePos.x)
                {
                    if(RopePointPositions[i].y > AttachedObstaclePos.y)
                    {
                        
                        RopePointPositions.RemoveAt(i);
                    }
                }
                else
                {
                    if (RopePointPositions[i].y > AttachedObstaclePos.y)
                    {
                        
                        RopePointPositions.RemoveAt(i);
                    }
                }
                
            }
        }
        
        


        // to here.



        mLineRenderer.positionCount = RopePointPositions.Count + 1;
        for (var i = mLineRenderer.positionCount - 1; i >= 0; --i)
        {
            
            if (i != mLineRenderer.positionCount - 1)
            {
                mLineRenderer.SetPosition(i, RopePointPositions[i]);

                // only 1 rope there or first rope of many rope point
                if (i == RopePointPositions.Count - 1 || RopePointPositions.Count == 1)
                {
                    var ropePosition = RopePointPositions[RopePointPositions.Count - 1];
                    if (RopePointPositions.Count == 1)
                    {
                        mRopePointrb.transform.position = ropePosition;
                        mRopeJoint.distance = Vector2.Distance(mPlayerPos, ropePosition);

                    }
                    else
                    {
                        mRopePoint.transform.position = ropePosition;
                        mRopeJoint.distance = Vector2.Distance(mPlayerPos, ropePosition);
                    }

                }
                // index of Last's Vector and if it's same as i - 1(which could be previous' previous..?
                // if there is only 1 rope
                // at first loop,  i - 1 =  0
                // 0 == last is indexof 0, 
                // second-to-last looping prevent...
                else if (i - 1 == RopePointPositions.IndexOf(RopePointPositions.Last()))
                {
                    var ropePosition = RopePointPositions.Last();
                    mRopePoint.transform.position = ropePosition;
                    mRopeJoint.distance = Vector2.Distance(mPlayerPos, ropePosition);
                }


            }
            else
            {
                mLineRenderer.SetPosition(i, transform.position);
            }

        }

    }

    private void HandleRopeLength()
    {
        if (Input.GetAxis("Vertical") >= 1f && BRopeAttached && !bColliding)
        {
            mRopeJoint.distance -= Time.deltaTime * ClimbSpeed;
        }
        else if(Input.GetAxis("Vertical") < 0f && BRopeAttached)
        {
            mRopeJoint.distance += Time.deltaTime * ClimbSpeed;
        }
    }

    // below 2 methods are checking if player touching obstacles, which used for checking rappel up or not..
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bColliding = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        bColliding = false;
    }


    Vector2 CalculateProjectileMotion(Vector2 mousePos)
    {
        float xDistance = mousePos.x - transform.position.x;
        float yDistance = mousePos.y - transform.position.y;


        //yDistanceAppliedFloat = 2f;

        float throwAngle = Mathf.Atan((yDistance + 4.905f) / xDistance);
        //float throwAngle = Mathf.Atan((yDistance + yDistanceAppliedFloat) / xDistance);
        float totalVeloc = xDistance / Mathf.Cos(throwAngle);

        float xVeloc = totalVeloc * Mathf.Cos(throwAngle);
        float yVeloc = totalVeloc * Mathf.Sin(throwAngle);

        return new Vector2(xVeloc, yVeloc);


        //ropePointRb.velocity
        //ropePointProjectile
    }


}
