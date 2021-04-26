using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class DogController : MonoBehaviour
{
    public float distance = 10;
    public Transform ballAttachPoint;

    public float speed = .1f;

    public AudioClip[] barks;

    void Start()
    {
        GetComponent<StateMachine>().ChangeState(new IdleState());

    }

    void Update()
    {

    }

    public void PlayBark()
    {
        GetComponent<AudioSource>().clip = barks[Random.Range(0, barks.Length)];
        GetComponent<AudioSource>().Play();
    }
}

public class IdleState : State
{
    float turnSpeed = 180.0f;
    float ballThrown = 10.0f;

    Vector3  target;

    public override void Enter()
    {
        owner.GetComponent<DogController>().PlayBark();    
        target = Camera.main.transform.position + Camera.main.transform.forward * owner.GetComponent<DogController>().distance;
        target.y = 0;
        owner.GetComponent<Arrive>().targetPosition = target;
        owner.GetComponent<Arrive>().enabled = true;
    }

    public override void Think()
    {
        if (Vector3.Distance(owner.transform.position, target) < 1)
        {
            owner.GetComponent<Arrive>().enabled = false;
            owner.GetComponent<Boid>().enabled = false;  
            Vector3 lookAt = Camera.main.transform.position - owner.transform.position;
            lookAt.y = 0;    
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, 
            Quaternion.LookRotation(lookAt), turnSpeed * Time.deltaTime);
        }
        // Look at the player
        
    }

    public override void Exit()
    {
    }
}

public class FetchingState : State
{
    public Transform ball;
    public Arrive arrive;
    public FetchingState(Transform ball)
    {
        this.ball = ball;
    }

    public override void Enter()
    {
        arrive = owner.GetComponent<Arrive>();
        arrive.enabled = true;
        owner.GetComponent<Boid>().enabled = true;
        owner.GetComponent<DogController>().PlayBark();
    }

    public override void Think()
    {
        //owner.GetComponent<DogController>().AnimateRun();
        Vector3 target = ball.transform.position;
        target.y = 0;
        arrive.targetPosition = target;


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //change state to back to owner

        if (Vector3.Distance(owner.transform.position, ball.transform.position) < 1)
        {
            owner.GetComponent<StateMachine>().ChangeState(new BackToOwnerState(ball));
        }
    }

    public override void Exit()
    {
        //owner.GetComponent<DogController>().DisableOtherAnimation(DogController.RUN_ANIMATION_BOOL);
    }
}

public class BackToOwnerState : State
{
    Transform ball;

    public BackToOwnerState(Transform ball)
    {
        this.ball = ball;
    }

    public override void Enter()
    {
        ball.transform.position = owner.GetComponent<DogController>().ballAttachPoint.position;
        ball.transform.parent = owner.GetComponent<DogController>().ballAttachPoint;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        Vector3 target = Camera.main.transform.position + Camera.main.transform.forward * owner.GetComponent<DogController>().distance;
        target.y = 0;
        owner.GetComponent<Arrive>().targetPosition = target;
        owner.GetComponent<Arrive>().enabled = true;
        owner.GetComponent<Boid>().enabled = true;
    }

    public override void Think()
    {
        Vector3 target = Camera.main.transform.position + Camera.main.transform.forward * owner.GetComponent<DogController>().distance;
        target.y = 0;
        owner.GetComponent<Arrive>().targetPosition = target;
        //owner.GetComponent<DogController>().AnimateRun();
        if (Vector3.Distance(owner.transform.position, target) < 1)
        {
            owner.GetComponent<StateMachine>().ChangeState(new IdleState());
        }

    }

    public override void Exit()
    {
        //owner.GetComponent<Boid>().velocity = Vector3.zero;
        //owner.GetComponent<Boid>().acceleration = Vector3.zero;
        owner.GetComponent<Arrive>().enabled = false;
        //////////////////////////////////////////////////////////////////////////////////////////////
        // Drop the ball
        ball.GetComponent<Rigidbody>().isKinematic = false;
        //owner.GetComponent<DogController>().DisableOtherAnimation(DogController.RUN_ANIMATION_BOOL);
        ball.parent = null;
    }

}