using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public GameObject dog;
   
    public GameObject ballPrefab;
    public float maxPower = 1000;

    private int index;
    public void Start()
    {
        //dog = FindObjectOfType<DogController>().gameObject;
        index = PlayerPrefs.GetInt("CharacterSelected");
        dog = dog.transform.GetChild(index).gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject ball = GameObject.Instantiate<GameObject>(ballPrefab);

            ball.transform.position = transform.position + transform.forward;

            ball.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * maxPower);

            //call on fetching state
            

            dog.GetComponent<StateMachine>().ChangeState(new FetchingState(ball.transform));
        
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                GameObject ball = GameObject.Instantiate<GameObject>(ballPrefab);

                ball.transform.position = transform.position + transform.forward;

                ball.GetComponent<Rigidbody>().AddForce(transform.forward * maxPower);

                //call on fetching state
                dog.GetComponent<StateMachine>().ChangeState(new FetchingState(ball.transform));
            }
        }
    }
}
