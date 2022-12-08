using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject pipePrefab;
    public float spawnRate = 1.2f;
    private float[] heightRange = { -1.5f, 2.5f };
    public float edgeX = 9.5f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", spawnRate, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {
        float height = Random.Range(heightRange[0], heightRange[1]);
        GameObject pipe = Instantiate(pipePrefab, new Vector3(edgeX, height, 0), Quaternion.identity);
    }
}
