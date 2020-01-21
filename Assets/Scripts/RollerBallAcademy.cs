using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerBallAcademy : Academy
{ //what does this do?
    public override void InitializeAcademy()
    {
        FloatProperties.RegisterCallback("gravity", f => { Physics.gravity = new Vector3(0, -10f, 0); });

    }
}
