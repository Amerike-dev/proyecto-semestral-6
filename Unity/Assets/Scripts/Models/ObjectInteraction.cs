using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject handPoint;
    private GameObject pickedObject = null;
    private bool isPicking = false;

    void Update()
    {
        
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            isPicking = true;
        }
        
        if (Keyboard.current.rKey.wasPressedThisFrame && pickedObject != null)
        {
            DropObject();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            if (isPicking && pickedObject == null)
            {
                PickUpObject(other.gameObject);
                isPicking = false;
            }
        }
    }

    private void PickUpObject(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().useGravity = false;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.position = handPoint.transform.position;
        obj.transform.SetParent(handPoint.transform);
        pickedObject = obj;
    }

    private void DropObject()
    {
        pickedObject.GetComponent<Rigidbody>().useGravity = true;
        pickedObject.GetComponent<Rigidbody>().isKinematic = false;
        pickedObject.transform.SetParent(null);
        pickedObject = null;
    }
}