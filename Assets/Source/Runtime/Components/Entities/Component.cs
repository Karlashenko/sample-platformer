using UnityEngine;

namespace Sample.Components.Entities
{
    public class Component : MonoBehaviour
    {
        public void Disable()
        {
            enabled = false;
        }

        public void Enable()
        {
            enabled = true;
        }
    }
}
