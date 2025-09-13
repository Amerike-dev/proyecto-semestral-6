using NUnit.Framework;
using UnityEngine;
using BuildSystem;

public class BuildZoneTests
{
    private BuildZone zone;

    [SetUp]
    public void SetUp()
    {
        zone = new BuildZone(id: 1, capacity: 3);
    }

    [TearDown]
    public void TearDown()
    {
        zone.Clear();
    }

    [Test]
    public void Add_AddsUpToCapacity_AndCountsCorrectly()
    {
        // Arrange
        var a = new GameObject("A");
        var b = new GameObject("B");
        var c = new GameObject("C");

        // Act
        var ok1 = zone.Add(a);
        var ok2 = zone.Add(b);
        var ok3 = zone.Add(c);

        // Assert
        Assert.IsTrue(ok1 && ok2 && ok3);
        Assert.AreEqual(3, zone.Count());
        Assert.IsFalse(zone.CanAccept()); // está lleno

        Object.DestroyImmediate(a);
        Object.DestroyImmediate(b);
        Object.DestroyImmediate(c);
    }

    [Test]
    public void Add_ReturnsFalse_WhenZoneIsFull()
    {
        // Arrange
        var a = new GameObject("A");
        var b = new GameObject("B");
        var c = new GameObject("C");
        var d = new GameObject("D");

        zone.Add(a);
        zone.Add(b);
        zone.Add(c);

        // Act
        var added = zone.Add(d);

        // Assert
        Assert.IsFalse(added);
        Assert.AreEqual(3, zone.Count());

        Object.DestroyImmediate(a);
        Object.DestroyImmediate(b);
        Object.DestroyImmediate(c);
        Object.DestroyImmediate(d);
    }

    [Test]
    public void CanFuse_IsTrue_WhenThereAreAtLeastTwoPieces()
    {
        // Arrange
        var a = new GameObject("A");
        var b = new GameObject("B");
        zone.Add(a);
        zone.Add(b);

        // Act + Assert
        Assert.IsTrue(zone.CanFuse());

        Object.DestroyImmediate(a);
        Object.DestroyImmediate(b);
    }

    [Test]
    public void FuseAll_ClearsZone_SetsIsComplete_AndReturnsFusedObject()
    {
        // Arrange
        var a = new GameObject("A");
        var b = new GameObject("B");
        zone.Add(a);
        zone.Add(b);

        // Act
        var fused = zone.FuseAll();

        // Assert
        Assert.IsNotNull(fused);
        Assert.AreEqual(0, zone.Count());
        Assert.IsTrue(zone.IsComplete);

        Object.DestroyImmediate(fused);
        Object.DestroyImmediate(a);
        Object.DestroyImmediate(b);
    }
}