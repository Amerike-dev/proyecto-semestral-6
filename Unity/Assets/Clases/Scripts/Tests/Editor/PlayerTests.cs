using NUnit.Framework;

[TestFixture]
public class PlayerTests
{
    [Test]
    public void Player_Constructor_IsInstanceOfPlayer()
    {
        // Arrange & Act
        var player = new Player(1);

        // Assert
        Assert.IsInstanceOf<Player>(player);
    }

    [Test]
    public void Player_Constructor_WithoutName_AssignsDefault()
    {
        // Arrange
        int id = 7;

        // Act
        var player = new Player(id);

        // Assert
        Assert.AreEqual($"Player {id}", player.PlayerName, "El nombre por defecto debe ser 'Player {id}'.");
    }

    [Test]
    public void Player_Constructor_WithName_AssignsGivenName()
    {
        // Arrange
        int id = 14;
        string givenName = "Tralalero";

        // Act
        var player = new Player(id, givenName);

        // Assert
        Assert.AreEqual(givenName, player.PlayerName, "El nombre asignado en el constructor debe conservarse.");
    }
}