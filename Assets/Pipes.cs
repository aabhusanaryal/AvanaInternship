using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public float speed = 5f;
    private float leftX;
    // Start is called before the first frame update
    void Start()
    {
        leftX = Camera.main.ScreenToWorldPoint(Vector3.zero).x -1 ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if(transform.position.x < leftX)
        {
            Destroy(gameObject);
        }
    }
}
