using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPrac : MonoBehaviour {

    [SerializeField] private float halfHeightchecking;
    public bool BOnGround;
    [SerializeField] private float groundCheckingRayDistance;


    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpSpeed = 5f;

    private float horizontalInput;
    private float jumpInput;

    public bool BJumping;

    // for swinging purpose
    public bool bSwinging;
    public Vector2 RopeHook;
    [SerializeField] private float swingForce = 4f;


    private Vector2 targetPos;

    public Rigidbody2D PlayerRb { get; set; }
    private SpriteRenderer playerSprite;

    [SerializeField] private Transform faceDirection;

    /*
    [Header("GrapplingHook")]
    [SerializeField] private SpriteRenderer grapplingHookSprite;
    [SerializeField] private Rigidbody2D grapplingHookRb;
    */

    // Use this for initialization
    void Awake ()
    {
        Debug.Log(" -1 is " + Mathf.Atan2(-1, 1) * Mathf.Rad2Deg);
        Debug.Log(" -1 is " + Mathf.Atan2(1, -1) * Mathf.Rad2Deg);

        CircleCollider2D testCircleCollider2D = GetComponent<CircleCollider2D>();
        


        PlayerRb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
	}

    void Start()
    {
        StartCoroutine(PrintSomething());
    }
    
    IEnumerator PrintSomething()
    {
        Debug.Log("Print Something");
        yield return new WaitForSeconds(3f);
        Debug.Log("print after 3 sec");
        
    }
    IEnumerator FireContinuously()
    {
        // instantiate. in while. and in while, yield return.
        float firingPeriod = 0.5f;
        yield return new WaitForSeconds(firingPeriod);
        // end
    }

    void FixedUpdate()
    {
        if (horizontalInput > 0f || horizontalInput < 0f)
        {
            playerSprite.flipX = horizontalInput < 0f;
            if (bSwinging)
            {
                var playerToHookDirection = (RopeHook - (Vector2)transform.position).normalized;

                // inverse the direction to get a perpendicular direction(90')
                Vector2 perpendicularDirection;
                if (horizontalInput < 0)
                {
                    perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
                    var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
                    Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0.5f);
                }
                else
                {
                    perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
                    var rightPerpPos = (Vector2)transform.position - perpendicularDirection * 2f;
                    Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0.5f);
                }

                var force = perpendicularDirection * swingForce;
                PlayerRb.AddForce(force, ForceMode2D.Force);


            }
            else // is not swinging...
            {
                if (BOnGround)
                {
                    var groundForce = moveSpeed * 2f;
                    PlayerRb.AddForce(new Vector2((horizontalInput * groundForce - PlayerRb.velocity.x) * groundForce, 0));
                    PlayerRb.velocity = new Vector2(PlayerRb.velocity.x, PlayerRb.velocity.y);



                    //playerRb.velocity = new Vector2(horizontalInput * moveSpeed, playerRb.velocity.y);
                }
            }



        }
        if (!bSwinging)
        {
            if (!BOnGround)
            {
                return;
            }
            if (jumpInput > 0f)
            {
                //playerRb.AddForce(new Vector2(transform.position.x, transform.position.y + (jumpSpeed * jumpInput)),0);
                PlayerRb.velocity = new Vector2(PlayerRb.velocity.x, jumpSpeed);
            }
        }

        
        



    }
	
	// Update is called once per frame
	void Update ()
    {

        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetAxis("Jump");

        halfHeightchecking = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        BOnGround = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeightchecking),
            Vector2.down,
            groundCheckingRayDistance);

        var worldMousePosition =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        SetFaceDirection(aimAngle);

    }

    private void SetFaceDirection(float aimAngle)
    {

        var faceX = transform.position.x + 0.15f * Mathf.Cos(aimAngle);
        //cosX = Mathf.Cos(aimAngle);
        var faceY = transform.position.y + 0.15f * Mathf.Sin(aimAngle);
        //sinY = Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(faceX, faceY, 0);
        faceDirection.transform.position = crossHairPosition;
    }

}
