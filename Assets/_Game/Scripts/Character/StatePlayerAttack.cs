public class StatePlayerAttack : IStatePlayer
{
    public void OnEnter(PlayerController player)
    {
        player.OnAttack();
    }
    public void OnExecute(PlayerController player)
    {
        player.Attack();
        player.CheckIdleToPatrol();
    }
    public void OnExit(PlayerController player)
    {
        player.OnResetAllTrigger();
    }
}