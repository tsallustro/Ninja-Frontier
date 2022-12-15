using UnityEngine;
using UnityEngine.Events;
namespace TeamNinja
{
    public class HintTrigger : MonoBehaviour
    {
        [SerializeField] string message;
        [SerializeField] GameObject hintParent;
        private HintManager hintManagerScript;

        public void Start()
        {
            hintManagerScript = hintParent.GetComponent<HintManager>();
        }
        private void OnTriggerEnter(Collider player)
        {
            hintManagerScript.ShowHint(message);
           
            
        }
    }
}
