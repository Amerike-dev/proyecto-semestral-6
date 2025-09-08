using NUnit.Framework;
using UnityEngine;

public class BrickTests
{
    private GameObject go;
    private Brick brick;

    [SetUp]
    public void SetUp()
    {
        go = new GameObject("Brick-Test");
        brick = go.AddComponent<Brick>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    [Test]
    public void DefaultState_Is_Spawned()
    {
        Assert.AreEqual(Brick.State.Spawned, brick.Current);
    }

    [Test]
    public void Spawned_To_PickedUp()
    {
        Assert.IsTrue(brick.TryPickUp());
        Assert.AreEqual(Brick.State.PickedUp, brick.Current);
    }

    [Test]
    public void PickedUp_To_Free()
    {
        brick.TryPickUp();
        Assert.IsTrue(brick.TryDropToFree());
        Assert.AreEqual(Brick.State.Free, brick.Current);
    }

    [Test]
    public void Free_To_PickedUp()
    {
        brick.TryPickUp();
        brick.TryDropToFree();
        Assert.IsTrue(brick.TryPickUp());
        Assert.AreEqual(Brick.State.PickedUp, brick.Current);
    }

    [Test]
    public void PickedUp_To_InAssembly()
    {
        brick.TryPickUp();
        Assert.IsTrue(brick.TryEnterAssembly());
        Assert.AreEqual(Brick.State.InAssembly, brick.Current);
    }

    [Test]
    public void Free_To_InAssembly()
    {
        brick.TryPickUp();
        brick.TryDropToFree();
        Assert.IsTrue(brick.TryEnterAssembly());
        Assert.AreEqual(Brick.State.InAssembly, brick.Current);
    }

    [Test]
    public void InAssembly_To_Merged()
    {
        brick.TryPickUp();
        brick.TryEnterAssembly();
        Assert.IsTrue(brick.TryMerge());
        Assert.AreEqual(Brick.State.Merged, brick.Current);
    }

    [Test]
    public void PickedUp_To_ThrowAway()
    {
        brick.TryPickUp();
        Assert.IsTrue(brick.TryThrowAway());
        Assert.AreEqual(Brick.State.ThrowAway, brick.Current);
    }

    [Test]
    public void Merged_Is_Terminal()
    {
        brick.TryPickUp();
        brick.TryEnterAssembly();
        brick.TryMerge();
        Assert.AreEqual(Brick.State.Merged, brick.Current);

        Assert.IsFalse(brick.TryPickUp());
        Assert.IsFalse(brick.TryDropToFree());
        Assert.IsFalse(brick.TryEnterAssembly());
        Assert.IsFalse(brick.TryMerge());
        Assert.IsFalse(brick.TryThrowAway());
        Assert.AreEqual(Brick.State.Merged, brick.Current);
    }
}