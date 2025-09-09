using UnityEngine;

public class Conveyor
{
    private Collider trigger;
    private float speed;
    private Vector3 direction;

    public Collider Trigger => trigger;
    public float Speed => speed;
    public Vector3 Direction => direction;

    public Conveyor(Collider trigger, float initialSpeed, Vector3 initialDirection)
    {
        this.trigger = trigger;
        this.speed = Mathf.Abs(initialSpeed);
        this.direction = initialDirection.sqrMagnitude > 0.0001f ? initialDirection.normalized : Vector3.right;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = Mathf.Max(0f, newSpeed);
    }

    public void InvertDirection()
    {
        direction = -direction;
    }

    
    public void ApplyConveyorEffect(Rigidbody body)
    {
        if (body == null) return;
        Vector3 v = body.linearVelocity;
        v.x = direction.x * speed;
        v.z = direction.z * speed;
        body.linearVelocity = v;
    }
}
