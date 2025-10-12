
public interface IInteractable
{
    void Interact(PlayerController player);
    void OnPickedUp(PlayerController player);
    void OnDropped();
}
