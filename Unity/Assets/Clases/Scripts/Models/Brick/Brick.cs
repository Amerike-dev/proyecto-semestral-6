// Brick.cs  (MODEL)
public class Brick
{
    public bool IsHeld { get; private set; } = false;
    public PlayerController CurrentHolder { get; private set; }

    public void PickUp(PlayerController player)
    {
        IsHeld = true;
        CurrentHolder = player;
    }

    public void Drop()
    {
        IsHeld = false;
        CurrentHolder = null;
    }
}
