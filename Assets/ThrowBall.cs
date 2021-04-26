using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public StateMachine dogStateMachine;
    public GameObject ballPrefab;
    public float maxPower = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ball = GameObject.Instantiate<GameObject>(ballPrefab);

            ball.transform.position = transform.position + transform.forward;

            ball.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * maxPower);

            dogStateMachine.ChangeState(new FetchingState(ball.transform));
        
        }
    }
}
