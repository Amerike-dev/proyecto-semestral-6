using NUnit.Framework;
using UnityEngine;
using BuildSystem; 
public class TrashZoneTests
{
    private TrashZone trashZone;

    [SetUp]
    public void Setup()
    {
        trashZone = new TrashZone(Vector3.zero, 5f);
    }

    [Test]
    public void Constructor_SetsPositionAndRadius()
    {
        // Arrange
        var pos = new Vector3(1, 2, 3);
        float radius = 10f;

        // Act
        var zone = new TrashZone(pos, radius);

        // Assert
        Assert.AreEqual(pos, zone.position);
        Assert.AreEqual(radius, zone.radius);
    }

    [Test]
    public void Constructor_NegativeRadius_SetsToZero()
    {
        var zone = new TrashZone(Vector3.zero, -5f);
        Assert.AreEqual(0f, zone.radius);
    }

    [Test]
    public void Accept_AddsPiece_AndDestroysIt()
    {
        // Arrange
        var piece = new GameObject("Piece");

        // Act
        trashZone.Accept(piece);

        // Assert
        Assert.AreEqual(1, trashZone.Count);
        Assert.IsTrue(piece == null); // Debe destruirse
    }

    [Test]
    public void Accept_NullPiece_DoesNothing()
    {
        trashZone.Accept(null);
        Assert.AreEqual(0, trashZone.Count);
    }

    [Test]
    public void Accept_DuplicatePiece_OnlyOnce()
    {
        var piece = new GameObject("Piece");

        trashZone.Accept(piece);
        trashZone.Accept(piece);

        Assert.AreEqual(1, trashZone.Count);
    }

    [Test]
    public void Remove_ExistingPiece_RemovesIt()
    {
        var piece = new GameObject("Piece");

        trashZone.Accept(piece);
        trashZone.Remove(piece);

        Assert.AreEqual(0, trashZone.Count);
    }

    [Test]
    public void Remove_NullPiece_DoesNothing()
    {
        trashZone.Remove(null);
        Assert.AreEqual(0, trashZone.Count);
    }

    [Test]
    public void ClearDestroyedPieces_RemovesNullEntries()
    {
        var piece = new GameObject("Piece");
        trashZone.Accept(piece);

        Object.DestroyImmediate(piece); // Simula destrucción inmediata
        trashZone.ClearDestroyedPieces();

        Assert.AreEqual(0, trashZone.Count);
    }

    [Test]
    public void DiscardSinglePiece_CallsAccept()
    {
        var piece = new GameObject("Piece");

        trashZone.DiscardSinglePiece(piece);

        Assert.AreEqual(1, trashZone.Count);
    }

    [Test]
    public void DiscardMergedPiece_CallsAccept()
    {
        var piece = new GameObject("MergedPiece");

        trashZone.DiscardMergedPiece(piece);

        Assert.AreEqual(1, trashZone.Count);
    }
}
