using UnityEngine;
using UnityEngine.Events;

namespace BuildSystem
{
    [RequireComponent(typeof(Collider))]
    public class BuildZoneController : MonoBehaviour
    {
        [Header("Zone Model")]
        public BuildZone zone;

        [Header("Collider (Trigger)")]
        public Collider zoneCollider;

        [Header("Behavior")]
        public bool autoFuse = true;
        public UnityEvent onFuse;

        void Reset()
        {
            zoneCollider = GetComponent<Collider>();
            if (zoneCollider != null) zoneCollider.isTrigger = true;
        }

        void Start()
        {
            if (zone == null) zone = new BuildZone(id: 0, capacity: 5);
            if (onFuse == null) onFuse = new UnityEvent();

            if (zoneCollider == null) zoneCollider = GetComponent<Collider>();
            if (zoneCollider != null) zoneCollider.isTrigger = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (zone == null || other == null) return;

            GameObject target = ResolvePieceRoot(other);
            if (target == null) return;

            if (!zone.IsComplete && zone.Count < zone.Capacity)
            {
                zone.Accept(target);

                if (autoFuse && zone.CanFuse())
                    TryFuse();
            }
        }

        public void TryFuse()
        {
            if (zone == null) return;
            if (!zone.CanFuse()) return;

            var fused = zone.FuseAll();
            if (fused != null)
                onFuse?.Invoke();
        }


        GameObject ResolvePieceRoot(Collider c)
        {
            if (c == null) return null;


            if (c.attachedRigidbody != null)
                return c.attachedRigidbody.gameObject;

            /* --TODO-- Descomentar lo siguiente cuando PieceManipulator esté disponible
            var manip = c.GetComponentInParent<PieceManipulator>();
            if (manip != null)
                return manip.gameObject;
            */

            // Fallback: raíz del transform
            return c.transform.root.gameObject;
        }
    }
}
