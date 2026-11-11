using UnityEngine;
using UnityEngine.AI;

public class NPCContext
{
    public Transform Transform;
    public NPCData Data => Transform.GetComponent<NPCData>();
    public Transform Target = null;
    public NavMeshAgent Agent => Transform.GetComponent<NavMeshAgent>();

    public NPCContext(Transform transform)
    {
        Transform = transform;
    }

    #region Distance
    public bool CheckReachDestination()
    {
        if (!Agent.hasPath && !Agent.pathPending) return true;
        else return false;
    }
    public float CheckDistanceToPlayer()
    {
        return Vector3.Distance(Agent.transform.position, Target.position);
    }
    public float GetDistanceToInterestPoint()
    {
        return Vector3.Distance(Transform.position, Data.TargetPosition);
    }
    public bool InterestPointWithinRange()
    {
        return GetDistanceToInterestPoint() <= Data.InterestPointMinDistance;
    }
    #endregion

    #region Movement
    public void MoveNPC()
    {
        // move position
        Transform.position = Vector3.MoveTowards(Transform.position, Data.TargetPosition, Data.MoveSpeed * Time.deltaTime);

        // clamp position
        Transform.position = ClampPosition(Transform.position, Data.WorldCenter.position, 5);
    }
    public Vector3 ClampPosition(Vector3 pos, Vector3 target, float offset)
    {
        return new(
           Mathf.Clamp(pos.x, target.x - offset, target.x + offset),
           pos.y,
           Mathf.Clamp(pos.z, target.z - offset, target.z + offset)
           );
    }
    #endregion

    #region Rotation
    public void LookAtPlayer()
    {
        Vector3 direction = (Target.position + Data.YOffset) - Transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Transform.up);
            targetRotation.x = 0;
            targetRotation.z = 0;
            Data.transform.rotation =
                Quaternion.RotateTowards(Transform.rotation, targetRotation, Data.TurnStep * Time.deltaTime);
        }
    }
    public void LookAtInterestPoint()
    {
        Vector3 direction = Data.TargetPosition - Transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Transform.up);
            targetRotation.x = 0;
            targetRotation.z = 0;
            Data.transform.rotation =
                Quaternion.RotateTowards(Transform.rotation, targetRotation, Data.TurnStep * Time.deltaTime);
        }
    }
    #endregion

    #region Frustration
    //public bool CheckFrustration()
    //{
    //    if (Data.FrustrationMeter >= Data.MaxFrustration)
    //    {
    //        Data.IsFrustrated = true;
    //    }
    //    else
    //    {
    //        Data.IsFrustrated = false;

    //        if (Data.FrustrationMeter <= 0)
    //        {
    //            Data.IsAvoiding = false;
    //        }

    //    }
    //    return Data.IsFrustrated;
    //}
    //public void CalculateFrustration()
    //{
    //    if (GetDistanceToPlayer() > Data.PlayerMaxDistance)
    //    {
    //        if (Data.GoToWaitState)
    //        {
    //            if (Data.FrustrationMeter < Data.MaxFrustration)
    //            {
    //                Data.FrustrationMeter += Time.deltaTime;
    //            }
    //        }
    //        else if (Data.IsAvoiding)
    //        {
    //            if (Data.FrustrationMeter > 0)
    //            {
    //                Data.FrustrationMeter -= Time.deltaTime;
    //            }
    //        }
    //    }
    //}
    #endregion

    public Vector3 SetTargetPosition()
    {
        Target.position = Target.position;
        Vector3 newPos = Target.position + Data.YOffset;
        Vector3 direction = (Target.position - Agent.transform.position).normalized;
        direction.y = 0;
        Data.TargetPosition = newPos - (direction * 1.5f);
        return Data.TargetPosition;
    }

    public void ResetPartner()
    {
        Agent.isStopped = false;
        Data.LastPartner = Data.CurrentPartner;
        Data.CurrentPartner = null;
        Target = null;
    }
}
