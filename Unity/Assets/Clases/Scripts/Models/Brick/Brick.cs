using UnityEngine;

public class Brick : MonoBehaviour
{
    public enum State
    {
        Spawned,
        PickedUp,
        Free,
        InAssembly,
        Merged,
        ThrowAway
    }

    public State Current { get; private set; } = State.Spawned;

    public bool TryPickUp() => TryGo(State.PickedUp);                                                                    // Spawned→PickedUp, Free→PickedUp
    public bool TryDropToFree() => Current == State.PickedUp && TryGo(State.Free);                                       // PickedUp→Free
    public bool TryEnterAssembly() => (Current == State.PickedUp || Current == State.Free) && TryGo(State.InAssembly);  // PickedUp/Free→InAssembly
    public bool TryMerge() => (Current == State.InAssembly || Current == State.PickedUp) && TryGo(State.Merged);        // InAssembly/PickedUp→Merged
    public bool TryThrowAway() => Current == State.PickedUp && TryGo(State.ThrowAway);                                  // PickedUp→ThrowAway

    bool TryGo(State target)
    {
        if (!CanGo(target)) return false;
        Current = target;
        return true;
    }

    bool CanGo(State target)
    {
        switch (Current)
        {
            case State.Spawned:
                return target == State.PickedUp || target == State.Free;

            case State.PickedUp:
                return target == State.Free || target == State.InAssembly || target == State.ThrowAway;

            case State.Free:
                return target == State.PickedUp || target == State.InAssembly;

            case State.InAssembly:
                return target == State.Merged || target == State.PickedUp;

            case State.Merged:
            case State.ThrowAway:
                return false;
        }
        return false;
    }
}