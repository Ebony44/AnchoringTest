using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGenericObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T prefab;

    public static MyGenericObjectPool<T> Instance { get; private set; }
    protected Queue<T> objects = new Queue<T>();

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        
    }


    public T Get()
    {
        if (objects.Count == 0)
        {
            AddToPool(1);
        }
        return objects.Dequeue();
    }
    public void AddToPool(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            var objectToEnqueue = Instantiate(prefab);
            objectToEnqueue.gameObject.SetActive(false);
            objects.Enqueue(objectToEnqueue);
        }
        

    }
    public void ReturnToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
