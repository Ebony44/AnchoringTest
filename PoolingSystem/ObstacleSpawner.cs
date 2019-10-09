using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] float minSpawnDelay = 0.5f;
    [SerializeField] float maxSpawnDelay = 1.2f;
    bool MBSpawning = true;
    private float gameTime;

    // store children's Position.
    [SerializeField] private GameObject parentPath;
    List<Transform> childrenSpawnPoints = new List<Transform>();

    // [SerializeField] Obstacle[] obstaclePrefabs;

    // Start is called before the first frame update


    void Awake()
    {
        foreach (Transform child in parentPath.transform)
        {
            childrenSpawnPoints.Add(child);
        }
    }

    private IEnumerator Start()
    {
        while (MBSpawning)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnObstacle();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //var spawnObstacle = SpawnObstacle();

        gameTime += Time.deltaTime;
        //StartCoroutine(spawnObstacle);
        if (gameTime > 20f)
        {
            MBSpawning = false;
        }

    }
    void SpawnObstacle()
    {

        //var obstacle = ObstaclePool.Instance.Get();
        var obstacle = ObstaclePool.ObstacleInstance.Get();

        

        int index = Random.Range(0, childrenSpawnPoints.Count);

        //obstacle.transform.position = transform.position;
        obstacle.transform.position = childrenSpawnPoints[index].position;
        //obstacle.transform.rotation = transform.rotation;
        obstacle.transform.rotation = childrenSpawnPoints[index].rotation;
        obstacle.gameObject.SetActive(true);

        


    }

    private void GetSpawnPoints()
    {
        //spawnPoints

        foreach(Transform child in parentPath.transform)
        {
            childrenSpawnPoints.Add(child);
        }
        //return childrenSpawnPoints;
    }

}
