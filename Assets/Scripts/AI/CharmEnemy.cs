using UnityEngine;
using BT.Abilities;

namespace BT.AI
{

    public class CharmEnemy : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) 
        {
            if ((other.GetComponent<AIController>() != null) && (other.tag == "Enemy"))
                other.GetComponent<AIController>().BecomeCharmed();
        }
    }
}