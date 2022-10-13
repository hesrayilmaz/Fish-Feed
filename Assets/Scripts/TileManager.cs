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

    // Start is called before the first frame update
    void Start()
    {
        //currentTile = firstTile;
        tileIndex = 0;
        tilePos = new Vector3(0, 0, 150);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > spawnTime)
        {
            GenerateTile();
            spawnTime = Time.time + timeBetweenSpawn;
        }
    }

    public void GenerateTile()
    {
        /*if (_currentTile != null)
        {
            Destroy(_currentTile);
        }*/
        tileIndex = RandomIntExcept(tilePrefabs.Length, tileIndex, tileIndex-1);
        Debug.Log(tileIndex);
        currentTile = Instantiate(tilePrefabs[tileIndex]);
        currentTile.transform.position = tilePos;
        tilePos.z += 100;
    }

    public GameObject GetCurrentTile()
    {
        return currentTile;
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

