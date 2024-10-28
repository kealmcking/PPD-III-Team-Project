using UnityEngine;

public class IceSurface : MonoBehaviour
{

        public float slipperyFactor = 0.05f; // Lower value means more slippery

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerController controller = other.GetComponent<playerController>();
                if (controller != null)
                {
                    controller.slipperyFactor = slipperyFactor;
                    controller.isSlipping = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerController controller = other.GetComponent<playerController>();
                if (controller != null)
                {
                    controller.slipperyFactor = 1f; 
                    controller.isSlipping = false;
                }
            }
        }
    

}
