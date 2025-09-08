using UnityEngine;
using UnityEngine.Events;

namespace BuildSystem
{
    // Controlador de la zona (sí es MonoBehaviour)
    public class BuildZoneController : MonoBehaviour
    {
        public BuildZone zone;
        public Collider zoneCollider;
        public bool autoFuse = true;
        public UnityEvent onFuse;

        void Start()
        {
            if (zone == null) zone = new BuildZone(id: 0, capacity: 5);
            if (onFuse == null) onFuse = new UnityEvent();
            if (zoneCollider != null) zoneCollider.isTrigger = true;
        }

        // CAMBIO: Cambiado de private void a protected void
        public void OnTriggerEnter(Collider other)
        {
            if (zone == null || other == null) return;

            if (zone.CanAccept())
            {
                zone.Add(other.gameObject);

                if (autoFuse && zone.CanFuse())
                {
                    TryFuse();
                }
            }
        }

        public void TryFuse()
        {
            if (zone == null) return;
            if (!zone.CanFuse()) return;

            var fused = zone.FuseAll();
            if (fused != null)
            {
                onFuse?.Invoke();
            }
        }
    }
}