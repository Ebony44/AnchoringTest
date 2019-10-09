using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LavaBehavior : MonoBehaviour
{
    [SerializeField] private LerpBehavior lerpBehavior;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovementPrac>() == true)
        {
            // SceneManager.
        }
    }
    // Update is called once per frame
    void Update()
    {
        
            
    }
    private void FixedUpdate()
    {
        if (LevelManager.BGamePaused || LevelManager.BGameEnded)
        {
            return;
        }
        lerpBehavior.YAxisProgessionTimer += Time.deltaTime;
        if (lerpBehavior.YAxisProgessionTimer <= lerpBehavior.MaxYAxisProgessionTimer)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + lerpBehavior.YaxisLerpingValue);
        }
    }
}
