using UnityEngine;
using BT.Core;
using BT.Variables;

namespace BT.Enemies
{
    public class Fighter : MonoBehaviour, IEnemyAction
    {
        [SerializeField] FloatReference timeBetweenAttacks;
        [SerializeField] FloatReference attackRange;

        Health target;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start() 
        {

        }

        private void Update()
        {
            
            if (target == null) return;
            if (target.IsDead()) return;

            if (!IsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();

            }

            timeSinceLastAttack += Time.deltaTime;

        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
                //This will trigger the "Hit" event from the animation.

            }
        }

        public Health GetTarget()
        {
            return target;
        }

        private void TriggerAttack()
        {
            //GetComponent<Animator>().ResetTrigger("stopAttack");
            //GetComponent<Animator>().SetTrigger("attack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return ((targetToTest != null) && (!targetToTest.IsDead()));
        }

        //Animation Event
        void Hit()
        {
            //Replace this text once I get damage into the mix.
            //target.TakeDamage(gameObject, currentAbility.GetDamage());
        }
        

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < attackRange;
        }

        public void Cancel()
        {
            //GetComponent<Animator>().ResetTrigger("attack");
            //GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

    }
}