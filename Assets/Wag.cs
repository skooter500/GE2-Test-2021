using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wag : MonoBehaviour
{
    public float frequency = 90;
    public float amplitude = 20;
    public Boid b;
    float theta = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = amplitude * Mathf.Sin(theta);
        transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        theta += Mathf.PI * 2.0f * frequency * Time.deltaTime * b.velocity.magnitude;
    }
}
