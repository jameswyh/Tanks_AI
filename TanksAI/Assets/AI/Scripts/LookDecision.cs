using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TANKS;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        RaycastHit hit;
        ShellExplosion shellExplosion;
        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.enemyStats.lookRange, Color.green);

        if (Physics.SphereCast(controller.eyes.position, controller.enemyStats.lookSphereCastRadius, controller.eyes.forward, out hit, controller.enemyStats.lookRange)
            && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            controller.stateFlag = true;
            controller.navMeshAgent.speed = 5;
            return true;
        }
        else if (controller.stateFlag)
        {
            controller.stateFlag = !controller.CheckIfCountDownElapsed(controller.enemyStats.searchDuration);
            if(controller.CheckIfCountDownElapsed(controller.enemyStats.searchDuration))
                controller.navMeshAgent.speed = 3.5f;
            return !controller.CheckIfCountDownElapsed(controller.enemyStats.searchDuration);
        }
        else
        {
            controller.navMeshAgent.speed = 3.5f;
            return false;
        }
    }
}