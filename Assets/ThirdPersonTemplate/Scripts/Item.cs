using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Item : MonoBehaviour, IInteractable
    {
        public void OnBeginOverlap(Player player)
        {
            Debug.Log("OnBeginOverlap");
        }

        public void OnEndOverlap(Player player)
        {
            Debug.Log("OnEndOverlap");
        }

        public void OnInteract(Player player)
        {
            Debug.Log("OnInteract");
        }
    }
}