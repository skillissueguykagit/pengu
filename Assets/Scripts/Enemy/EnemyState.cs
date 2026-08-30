using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    public enum EnemyState
    {
        Alive,
        Staggered,
        Spared,
        Dead
    }

    [SerializeField] private EnemyState currentState = EnemyState.Alive;

    public EnemyState GetState()
    {
        return currentState;
    }

    public void SetState(EnemyState newState)
    {
        currentState = newState;
    }
}