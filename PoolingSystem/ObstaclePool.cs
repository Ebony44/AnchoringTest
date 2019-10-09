using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour//MyGenericObjectPool<Obstacle>
{
    [SerializeField] Obstacle[] obstaclePrefabs;
    public static ObstaclePool ObstacleInstance { get; private set; }
    protected Queue<Obstacle> objects = new Queue<Obstacle>();

    private void Awake()
    {
        // for testing, checking length of sington
        Debug.Log("count of Instance's objects' " + objects.Count);
        Debug.Log("count of obstaclePrefabs.Length " + obstaclePrefabs.Length);
        if (ObstacleInstance == null)
        {
            ObstacleInstance = this;
        }
        else if (ObstacleInstance != this)
        {
            Destroy(gameObject);
        }
    }
    public Obstacle Get()
    {
        Debug.Log("Get from Pool");
        int index = Random.Range(0, obstaclePrefabs.Length);
        //Debug.Log(obstaclePrefabs.Length);
        if (objects.Count == 0)
        {
            AddToPoolFromArray(1,index);
        }
        
        return objects.Dequeue();
    }
    public void AddToPoolFromArray(int count, int index)
    {
        for (int i = 0; i < count; ++i)
        {
            var objectToEnqueue = Instantiate(obstaclePrefabs[index]);
            objectToEnqueue.gameObject.SetActive(false);
            objects.Enqueue(objectToEnqueue);
        }
    }
    public void ReturnToPool(Obstacle objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        objects.Enqueue(objectToReturn);
    }
}
