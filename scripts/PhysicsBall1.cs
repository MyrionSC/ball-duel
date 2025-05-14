using Godot;
using System;

public partial class PhysicsBall1 : RigidBody2D
{
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        
        Vector2 analogInput = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Console.WriteLine("analogInput " + analogInput);

        var constantForce = GetConstantForce();
        Console.WriteLine("constantForce " +  constantForce);

        ApplyForce(analogInput * 100);
    }

}
