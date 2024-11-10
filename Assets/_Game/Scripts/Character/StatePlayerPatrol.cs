public class StatePlayerPatrol : IStatePlayer
{
    public void OnEnter(PlayerController player)
    {
        player.OnRun();
    }
    public void OnExecute(PlayerController player)
    {
        player.Move();
        player.CheckPatrolToIdle();
    }
    public void OnExit(PlayerController player)
    {
        player.OnResetAllTrigger();
    }
}