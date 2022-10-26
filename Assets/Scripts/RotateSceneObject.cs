using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSceneObject : MonoBehaviour
{
    float random = 0.0f;
    Vector3 rot;
    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0.4f, 0.8f);
        rot = new Vector3(Random.Range(0.2f, 1.0f), Random.Range(0.2f, 1.0f), Random.Range(0.2f, 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rot, random);
    }
}
