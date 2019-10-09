using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float backgroundScrollSpeed;
    [SerializeField] private float startY;
    [SerializeField] private float endY;
    // second
    // private float length;
    // private float startPos;
    // public GameObject cam;
    // public float parallaxEffect;

    // third
    public GameObject[] levels;
    private Camera mainCamera;
    private Vector2 screenBounds;

    // fourth
    private Rigidbody2D rb2d;
    public float scrollSpeed = -1.5f;

    private BoxCollider2D wallCollider;
    private float verticalLength;

    // Use this for initialization
    void Start () {
        // second
        // startPos = transform.position.y;
        // length = GetComponent<SpriteRenderer>().bounds.size.y;
        // third
        // mainCamera = gameObject.GetComponent<Camera>();
        // screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.rotation.z));

        //foreach (GameObject obj in levels)
        //{
        //    loadChildObjects(obj);
        //}

        // fourth
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0, scrollSpeed);

        wallCollider = GetComponent<BoxCollider2D>();
        verticalLength = wallCollider.size.y;

	}

    private void RepositionBackground()
    {
        Vector2 wallOffset = new Vector2(0, verticalLength * 2f);
        transform.position = (Vector2)transform.position + wallOffset;
    }

    void loadChildObjects (GameObject obj)
    {
        Debug.Log(obj.name);
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.y;
        int childsNeeded = (int) Mathf.Ceil( screenBounds.y * 2 / objectWidth);
        GameObject clone = Instantiate(obj) as GameObject;
        for (int i=0;i<=childsNeeded;i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }
        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }
	
	// Update is called once per frame
	void Update ()
    {



        /*
        transform.Translate(Vector2.down * backgroundScrollSpeed * Time.deltaTime);

        if (transform.position.y <= endY)
        {
            
            transform.position = new Vector2(transform.position.x, startY);
        }
        */
        // second is in fixedupdate
        // fourth
        /*
        if (gameOver)
        {
            rb2d.velocity = Vector2.zero;
        }
        
        */
        if (transform.position.y < -verticalLength)
        {
            RepositionBackground();
        }


    }
    private void FixedUpdate()
    {
        /* second
        float temp = (cam.transform.position.y * (1 - parallaxEffect));
        float dist = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(transform.position.x, startPos + dist, transform.rotation.z);

        if(temp > startPos + length)
        {
            startPos += length;
        }
        else if(temp < startPos - length)
        {
            startPos -= length;
        }
        */
    }
    // third
    /*
    void repositionChildObjects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.y;
            if (transform.position.y + screenBounds.y > lastChild.transform.position.y + halfObjectWidth)
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(lastChild.transform.position.x,
                    lastChild.transform.position.y + halfObjectWidth * 2,
                    lastChild.transform.position.z);
            }
            else if(transform.position.y - screenBounds.y < firstChild.transform.position.y + halfObjectWidth )
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(firstChild.transform.position.x,
                    firstChild.transform.position.y - halfObjectWidth * 2,
                    firstChild.transform.position.z);
            }
        }
    }
    private void LateUpdate()
    {
        foreach(GameObject obj in levels)
        {
            repositionChildObjects(obj);
        }
    }
    */
}
