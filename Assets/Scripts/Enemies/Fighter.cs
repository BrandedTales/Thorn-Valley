using UnityEngine;
using BT.Core;
using BT.Variables;
using BT.Abilities;

namespace BT.Enemies
{
    public class Fighter : MonoBehaviour, IEnemyAction
    {

        [SerializeField] FloatReference attackRange;
        public Ability attackAbility;

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
            if (timeSinceLastAttack >= attackAbility.cooldown.value)
            {
                transform.LookAt(target.transform);
                TriggerAttack();
                timeSinceLastAttack = 0;
                //This will trigger the "Hit" event from the animation.

            }
        }

        public Health GetTarget()
        {
            return target;
        }

        public void TriggerAttack()
        {
            attackAbility.Execute(this.gameObject);

            //Once the animator is set up and working, in theory the actually triggering of damage would take place in the "Hit" phase.
            //Coupled with that, the Execute above would be wrapped into "hit" and we'd have the if/else to account for ranged vs melee.
            //GetComponent<Animator>().ResetTrigger("stopAttack");
            //GetComponent<Animator>().SetTrigger("attack");
        }

        public void CharmFindTarget()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
            Debug.Log(gameObject.name + " searching for potential new targets.");
            foreach (Collider collider in hitColliders)
            {
                if ((collider.tag == "Enemy")&&(collider.gameObject != this.gameObject))
                {
                    target = collider.GetComponent<Health>();
                    return;
                }
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            States playerStates = GameObject.FindWithTag("Player").GetComponent<States>();
            if (combatTarget == null) return false;
            if (playerStates.GetState((int)PlayerPassive.Invisible)) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return ((targetToTest != null) && (!targetToTest.IsDead()));
        }

        //Animation Event
        void Hit()
        {
            //Replace this text once we add animations.  This should either fire off the ability noted in "TriggerAttack" or it should deal melee damage.
            //If it deals melee, make sure to account for invulnerability.
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