using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ConveyorTests
{
    [Test]
    public void Conveyor_IsInstanceOfConveyor()
    {
        // Arrange
        var collider = new GameObject("Trigger").AddComponent<BoxCollider>();
        collider.isTrigger = true;

        var conveyor = new Conveyor(collider, 5f, Vector3.forward);

        // Assert
        Assert.IsInstanceOf<Conveyor>(conveyor);
    }

    [Test]
    public void Conveyor_SetSpeed_WorksCorrectly()
    {
        // Arrange
        var collider = new GameObject("Trigger").AddComponent<BoxCollider>();
        collider.isTrigger = true;

        var conveyor = new Conveyor(collider, 0f, Vector3.forward);

        // Act
        conveyor.SetSpeed(2f);

        // Assert
        Assert.AreEqual(2f, conveyor.Speed);
    }

    [Test]
    public void Conveyor_InvertDirection_WorksCorrectly()
    {
        // Arrange
        var collider = new GameObject("Trigger").AddComponent<BoxCollider>();
        collider.isTrigger = true;

        var conveyor = new Conveyor(collider, 3f, Vector3.forward);

        // Act
        conveyor.InvertDirection();

        // Assert
        Assert.AreEqual(Vector3.back, conveyor.Direction);
    }

    [Test]
    public void Conveyor_AffectsRigidBodyVelocity()
    {
        // Arrange
        var collider = new GameObject("Trigger").AddComponent<BoxCollider>();
        collider.isTrigger = true;

        var conveyor = new Conveyor(collider, 5f, Vector3.forward);

        var obj = new GameObject("TestObject");
        var rb = obj.AddComponent<Rigidbody>();

        // Act → simula que el conveyor le aplica movimiento
        conveyor.ApplyConveyorEffect(rb);

        // Assert
        Assert.AreEqual(Vector3.forward * 5f, rb.linearVelocity);
    }
}