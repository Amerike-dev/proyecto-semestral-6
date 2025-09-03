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
        go.AddComponent<BoxCollider>();
        go.AddComponent<Rigidbody>(); 
        go.AddComponent<MeshRenderer>();    
        brick = go.AddComponent<Brick>();           
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
    }

    [Test]
    public void Brick_Constructor_IsInstanceOfBrick()
    {
        Assert.IsInstanceOf<Brick>(brick);
    }

    [Test]
    public void Brick_DefaultState_IsRaw()
    {
        Assert.AreEqual(Brick.BrickState.Raw, brick.CurrentState);
    }

    [Test]
    public void Brick_Cut_ChangesStateToCut()
    {
        brick.Cut();
        Assert.AreEqual(Brick.BrickState.Cut, brick.CurrentState);
    }

    [Test]
    public void Brick_Paint_ChangesStateToPainted()
    {
        brick.Paint();
        Assert.AreEqual(Brick.BrickState.Painted, brick.CurrentState);
    }

    [Test]
    public void Brick_CutAfterPaint_ChangesStateToCutAndPainted()
    {
        brick.Paint();
        brick.Cut();
        Assert.AreEqual(Brick.BrickState.CutAndPainted, brick.CurrentState);
    }

    [Test]
    public void Brick_PaintAfterCut_ChangesStateToCutAndPainted()
    {
        brick.Cut();
        brick.Paint();
        Assert.AreEqual(Brick.BrickState.CutAndPainted, brick.CurrentState);
    }

    [Test]
    public void Brick_Reset_RestoresStateToRaw()
    {
        brick.Cut();
        brick.ResetBrick();
        Assert.AreEqual(Brick.BrickState.Raw, brick.CurrentState);
    }
}