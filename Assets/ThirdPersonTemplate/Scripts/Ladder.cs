using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonTemplate
{
    public class Ladder : MonoBehaviour, IInteractable
    {
        public void OnBeginOverlap(Player player)
        {

        }

        public void OnEndOverlap(Player player)
        {

        }

        public void OnInteract(Player player)
        {
            player.GetComponent<Movement>().StartLadderClimb();
        }
    }
}