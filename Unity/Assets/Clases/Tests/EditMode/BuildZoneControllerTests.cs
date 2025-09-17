using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using BuildSystem; 

public class BuildZoneControllerTests
{
    private GameObject goController;
    private BuildZoneController controller;

    [SetUp]
    public void SetUp()
    {
        goController = new GameObject("Controller");
        controller = goController.AddComponent<BuildZoneController>();
        controller.zone = new BuildZone(id: 99, capacity: 3);
        controller.autoFuse = true;
        controller.onFuse = new UnityEvent();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(goController);
    }

    [Test]
    public void OnTriggerEnter_AddsObject_AndAutoFuses_WhenPossible()
    {
        // Arrange
        var o1 = new GameObject("P1");
        var o2 = new GameObject("P2");
        var c1 = o1.AddComponent<BoxCollider>();
        var c2 = o2.AddComponent<BoxCollider>();

        int fuseCount = 0;
        controller.onFuse.AddListener(() => fuseCount++);

        // Act: simulamos dos entradas a la zona
        controller.OnTriggerEnter(c1);
        controller.OnTriggerEnter(c2);

        // Assert
        Assert.AreEqual(0, controller.zone.Count);  
        Assert.AreEqual(1, fuseCount);

        Object.DestroyImmediate(o1);
        Object.DestroyImmediate(o2);
    }

    [Test]
    public void TryFuse_DoesNothing_WhenCannotFuse()
    {
        // Arrange
        var o1 = new GameObject("Solo");
        controller.zone.Accept(o1);      

        int fuseCount = 0;
        controller.onFuse.AddListener(() => fuseCount++);

        // Act
        controller.TryFuse();

        // Assert
        Assert.AreEqual(1, controller.zone.Count);   
        Assert.AreEqual(0, fuseCount);

        Object.DestroyImmediate(o1);
    }
}
