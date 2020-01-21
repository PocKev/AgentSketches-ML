using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgentScript : Agent
{
    Rigidbody rb;

    public Transform Target;
    public float speed = 10;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    public override void AgentReset()
    {
        this.rb.angularVelocity = Vector3.zero;
        this.rb.velocity = Vector3.zero;
        this.transform.position = new Vector3(0f, 0.5f, 0f);
        
        Target.position = new Vector3(Random.value * 8 - 4, 
            0.5f, 
            Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Target.position);
        AddVectorObs(this.transform.position);

        AddVectorObs(rb.velocity.x);
        AddVectorObs(rb.velocity.z);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rb.AddForce(controlSignal * speed);

        float distanceToTarget = Vector3.Distance(this.transform.position, 
            Target.position);
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if (this.transform.position.y < 0)
        {
            Done();
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
