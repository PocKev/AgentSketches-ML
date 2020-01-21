using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class GoToAgent : Agent
{
    public float speed = 10f;
    public Transform target;

    private Transform originalTransform;
    private Rigidbody rb;
    private bool onGoal, outOfBounds;
    private float originalDistance;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        onGoal = false;
        outOfBounds = false;
        originalDistance = Vector3.Distance(target.localPosition, this.transform.localPosition);
        originalTransform = this.transform;
    }

    public override void AgentReset()
    {
        this.rb.angularVelocity = Vector3.zero;
        this.rb.velocity = Vector3.zero;
        
        this.transform.localPosition = new Vector3(
            Random.value * 8 - 4,
            0.232f,
            Random.value * 8 - 4);

        this.transform.rotation = originalTransform.rotation;

        onGoal = false;
        outOfBounds = false;
        originalDistance = Vector3.Distance(target.localPosition, this.transform.localPosition);
    }

    public override void CollectObservations()
    {
        AddVectorObs(target.localPosition);
        AddVectorObs(this.transform.localPosition);

        AddVectorObs(rb.velocity.x);
        AddVectorObs(rb.velocity.z);
        AddVectorObs(rb.angularVelocity);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = new Vector3(vectorAction[0], 0f, vectorAction[1]);
        rb.AddForce(controlSignal * speed, ForceMode.VelocityChange);
        float distanceFromGoal = Vector3.Distance(target.localPosition, this.transform.localPosition);
        
        //SetReward((originalDistance - distanceFromGoal) / originalDistance);

        if (onGoal)
        {
            SetReward(1f);
            Done();
        }
        if(outOfBounds)
        {
            SetReward(-1f);
            Done();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Goal")
        {
            onGoal = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.parent.name == "Bounds")
        {
            outOfBounds = true;
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
