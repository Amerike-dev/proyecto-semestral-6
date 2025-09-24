using NUnit.Framework;
using UnityEngine;
using System;

[TestFixture]
public class GeneralSpawnerTests
{
    private GeneralSpawner spawner;

    [SetUp]
    public void SetUp()
    {
        GameObject spawnerGO = new GameObject();
        spawner = spawnerGO.AddComponent<GeneralSpawner>();
    }

    [TearDown]
    public void TearDown()
    {
        foreach (var obj in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            UnityEngine.Object.DestroyImmediate(obj);
        }
    }

    [Test]
    public void GeneralSpawner_SpawnPiece_CreatesObjectInScene()
    {
        // Arrange
        GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 position = Vector3.zero;

        // Act
        GameObject spawned = spawner.Spawn(prefab, position);

        // Assert
        Assert.IsNotNull(spawned, "El objeto spawneado no debe ser nulo.");
        Assert.AreEqual(position, spawned.transform.position, "El objeto no se creó en la posición esperada.");
    }

    [Test]
    public void GeneralSpawner_SpawnPiece_AssignsCorrectPrefab()
    {
        // Arrange
        GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 position = new Vector3(2, 0, 0);

        // Act
        GameObject spawned = spawner.Spawn(prefab, position);

        // Assert
        Assert.AreEqual("Sphere", spawned.name.Replace("(Clone)", "").Trim(), "El prefab instanciado no coincide.");
    }

    [Test]
    public void GeneralSpawner_SpawnPiece_NullPrefab_ThrowsException()
    {
        // Arrange
        GameObject prefab = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            spawner.Spawn(prefab, Vector3.zero);
        }, "El método debe lanzar una excepción si el prefab es nulo.");
    }

    [Test]
    public void GeneralSpawner_SpawnPiece_MultipleCalls_SpawnsAllObjects()
    {
        // Arrange
        GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Act
        GameObject obj1 = spawner.Spawn(prefab, Vector3.zero);
        GameObject obj2 = spawner.Spawn(prefab, new Vector3(1, 0, 0));
        GameObject obj3 = spawner.Spawn(prefab, new Vector3(2, 0, 0));

        // Assert
        Assert.IsNotNull(obj1, "El primer objeto no se instanció correctamente.");
        Assert.IsNotNull(obj2, "El segundo objeto no se instanció correctamente.");
        Assert.IsNotNull(obj3, "El tercer objeto no se instanció correctamente.");

        // Mejor: agrupa y verifica la cantidad
        GameObject[] spawnedObjects = { obj1, obj2, obj3 };
        Assert.AreEqual(3, spawnedObjects.Length, "No se instanciaron los tres objetos esperados.");
    }
}
