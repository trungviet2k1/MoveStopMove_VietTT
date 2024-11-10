using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    [HideInInspector] public enum CharacterAnimState { Attack, Dance, Idle, Death, Run, Win, Ulti }

    [Header("Animation State")]
    public CharacterAnimState lastState;
    private Animator animator;

    void Awake()
    {
        InitializeVariables();
    }

    private void InitializeVariables()
    {
        animator = GetComponent<Animator>();
        lastState = CharacterAnimState.Idle;
    }

    #region Set Character Animation
    public void SetAnim(CharacterAnimState _CharacterAnimation)
    {
        if (_CharacterAnimation != lastState)
        {
            switch (_CharacterAnimation)
            {
                case CharacterAnimState.Attack:
                    animator.SetTrigger(Constants.ATTACK);
                    break;
                case CharacterAnimState.Dance:
                    animator.SetTrigger(Constants.DANCE);
                    break;
                case CharacterAnimState.Idle:
                    animator.SetTrigger(Constants.IDLE);
                    break;
                case CharacterAnimState.Death:
                    animator.SetTrigger(Constants.DEATH);
                    break;
                case CharacterAnimState.Run:
                    animator.SetTrigger(Constants.RUN);
                    break;
                case CharacterAnimState.Win:
                    animator.SetTrigger(Constants.WIN);
                    break;
                case CharacterAnimState.Ulti:
                    animator.SetTrigger(Constants.ULTI);
                    break;
            }
            lastState = _CharacterAnimation;
        }
    }
    #endregion


    #region Play Character Animation
    public void AttackAnimation()
    {
        SetAnim(CharacterAnimState.Attack);
    }

    public void DanceAnimation()
    {
        SetAnim(CharacterAnimState.Dance);
    }

    public void IdleAnimation()
    {
        SetAnim(CharacterAnimState.Idle);
    }

    public void DeathAnimation()
    {
        SetAnim(CharacterAnimState.Death);
    }

    public void RunAnimation()
    {
        SetAnim(CharacterAnimState.Run);
    }

    public void WinAnimation()
    {
        SetAnim(CharacterAnimState.Win);
    }

    public void UltiAnimation()
    {
        SetAnim(CharacterAnimState.Ulti);
    }

    public void ResetAllTriggerAnim()
    {
        animator.ResetTrigger(Constants.ATTACK);
        animator.ResetTrigger(Constants.DANCE);
        animator.ResetTrigger(Constants.IDLE);
        animator.ResetTrigger(Constants.DEATH);
        animator.ResetTrigger(Constants.RUN);
        animator.ResetTrigger(Constants.WIN);
        animator.ResetTrigger(Constants.ULTI);
    }
    #endregion
}