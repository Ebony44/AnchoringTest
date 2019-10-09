using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    public float lifeTime = 0f;
    public float maxLifeTime = 5f;

    [SerializeField] private Rigidbody2D rb;


    //[SerializeField] private GameObject obstacle;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        MovingObastacle();
        //lifeTime += Time.deltaTime;
        //if (lifeTime > maxLifeTime)
        //{
        //    lifeTime = 0f;
        //    //ObstaclePool.Instance.ReturnToPool(this);
        //    ObstaclePool.ObstacleInstance.ReturnToPool(this);

        //}
    }
    void MovingObastacle()
    {
        rb.velocity = Vector2.down * moveSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 16)
        {
            
            ObstaclePool.ObstacleInstance.ReturnToPool(this);
        }
        else
        {
            return;
        }
    }
}
