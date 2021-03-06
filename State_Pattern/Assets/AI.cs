using UnityEngine;
using UnityEngine.AI;

namespace Assets
{
    public class AI : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Animator anim;
        public Transform player;
        private State currentState;

        // Start is called before the first frame update
        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();
            anim = this.GetComponent<Animator>();
            currentState = new Idle(this.gameObject, agent, anim, player);
        }

        // Update is called once per frame
        void Update()
        {
            currentState = currentState.Process();
        }
    }
}
