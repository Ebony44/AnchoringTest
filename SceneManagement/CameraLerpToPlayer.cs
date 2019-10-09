using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerpToPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float lerpingSpeed;
    public float cameraDepth = -10f;
    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;

    //public float YAxisProgession = 0.02f; get it from other gameobject..
    [SerializeField] private LerpBehavior lerpBehavior;

    public float YAxisProgessionTimer = 0f;
    public float MaxYAxisProgessionTimer = 10f;

    [SerializeField] private GameObject mLava;

    //private float lava

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.BGamePaused || LevelManager.BGameEnded)
        {
            return;
        }
        var newPosition = Vector2.Lerp(transform.position, playerTransform.position, Time.deltaTime * lerpingSpeed);
        var camPosition = new Vector3(newPosition.x, newPosition.y, cameraDepth);
        var camPositionForClamp = camPosition;

        var newX = Mathf.Clamp(camPositionForClamp.x, minX, maxX);
        var newY = Mathf.Clamp(camPositionForClamp.y, minY, maxY);
        transform.position = new Vector3(newX, newY, cameraDepth);

        

        
    }
    private void FixedUpdate()
    {
        lerpBehavior.YAxisProgessionTimer += Time.deltaTime;
        if (lerpBehavior.YAxisProgessionTimer <= lerpBehavior.MaxYAxisProgessionTimer)
        {
            ProgessOfYAxis();
        }
    }
    private void ProgessOfYAxis()
    {
        minY += lerpBehavior.YaxisLerpingValue;
        maxY += lerpBehavior.YaxisLerpingValue;
        // mLava.transform.position = new Vector2(mLava.transform.position.x, mLava.transform.position.y + YAxisProgession);
    }
}
