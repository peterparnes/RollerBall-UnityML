﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerAgent : Agent {

    bool fell = false; 

    Rigidbody rBody;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;
    public Transform floor; 

    public override void AgentReset()
    {
        // if (this.transform.position.y < -1.0)
        if(fell)
        {
            // The agent fell
            Vector3 pos = Vector3.zero;
            pos.y = 0.5f;
            this.transform.position = pos;
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;

            fell = false; 
        }
        else
        {
            float distanceToTarget = 0;

            do
            {
                // Move the target to a new spot
                Target.position = new Vector3(Random.value * 8 - 4,
                                              0.5f,
                                              Random.value * 8 - 4);

                distanceToTarget = Vector3.Distance(this.transform.position,
                                                          Target.position);

            } while (distanceToTarget < 2.0f);
        }
    }

    public override void CollectObservations()
    {
        // Calculate relative position
        Vector3 relativePosition = Target.position - this.transform.position;

        // Relative position
        AddVectorObs(relativePosition.x);
        AddVectorObs(relativePosition.z);

        // Distance to center of platform
        AddVectorObs(this.transform.position.x - floor.position.x);
        AddVectorObs(this.transform.position.z - floor.position.z);

        // Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);

        /*
        // Relative position
        AddVectorObs(relativePosition.x / 5);
        AddVectorObs(relativePosition.z / 5);

        // Distance to edges of platform
        AddVectorObs((this.transform.position.x + 5) / 5);
        AddVectorObs((this.transform.position.x - 5) / 5);
        AddVectorObs((this.transform.position.z + 5) / 5);
        AddVectorObs((this.transform.position.z - 5) / 5);

        // Agent velocity
        AddVectorObs(rBody.velocity.x / 5);
        AddVectorObs(rBody.velocity.z / 5); */
    }

    public float speed = 10;
    private float previousDistance = float.MaxValue;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
                                                  Target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            Done();
            AddReward(1.0f);

            GetComponent<Score>().incScore();
        }

        // Getting closer
        if (distanceToTarget < previousDistance)
        {
            Debug.Log("dist smaller *************");
            AddReward(0.1f);
        } else {
            // AddReward(-0.05f);

        }

        // Time penalty
        // AddReward(-0.05f);

        // Fell off platform
        if (this.transform.position.y < -1.0)
        {
            Done();
            AddReward(-1.0f);
            fell = true;

            GetComponent<Score>().resetScore();

        }
        previousDistance = distanceToTarget;

        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = Mathf.Clamp(vectorAction[0], -1, 1);
        controlSignal.z = Mathf.Clamp(vectorAction[1], -1, 1);
        rBody.AddForce(controlSignal * speed);

        // Debug.Log(GetReward());
        // Debug.Log(GetCumulativeReward());
        // Debug.Log(distanceToTarget);
    }
}
