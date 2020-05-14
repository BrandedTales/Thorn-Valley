using UnityEngine;

namespace BT.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IEnemyAction currentAction;

        public void StartAction(IEnemyAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
            
        
    }
}