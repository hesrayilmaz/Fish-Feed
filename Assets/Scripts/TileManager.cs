using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;
    private int tileIndex;
    private Vector3 tilePos;
    [SerializeField] private float timeBetweenSpawn;
    private float spawnTime;
    private GameObject currentTile;
    [SerializeField] private GameObject firstTile;
    [SerializeField] private Transform playerTransform;
    private List<GameObject> activeTiles;
    private float tileLength = 120;
    private float destroyPoint;

    // Start is called before the first frame update
    void Start()
    {
        activeTiles = new List<GameObject>();
        activeTiles.Add(firstTile);
        tileIndex = 0;
        tilePos = new Vector3(0, 45, 190);
        destroyPoint = playerTransform.position.z + 300;
    }

    // Update is called once per frame
    void Update()
    {

        if (playerTransform.position.z - (tilePrefabs.Length * tileLength)/3 > tilePos.z - ((tilePrefabs.Length) * tileLength))
        {
            GenerateTile();
        }

        if (playerTransform.position.z >= destroyPoint)
        {
            DestroyTile();
            destroyPoint += tileLength;
        }
    }

    public void GenerateTile()
    {
        tileIndex = RandomIntExcept(tilePrefabs.Length, tileIndex, tileIndex-1);
        currentTile = Instantiate(tilePrefabs[tileIndex],tilePos,Quaternion.identity);
        activeTiles.Add(currentTile);
        tilePos.z += tileLength;
    }

    public void DestroyTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    public GameObject GetCurrentTile()
    {
        return currentTile;
    }

    public float GetDestroyPoint()
    {
        return destroyPoint;
    }

    public int RandomIntExcept(int n, params int[] excepts)
    {
        int result = Random.Range(1, n - excepts.Length);

        for (int i = 0; i < excepts.Length; i++)
        {
            if (result < excepts[i])
                return result;
            result++;
        }
        return result;
    }

}

