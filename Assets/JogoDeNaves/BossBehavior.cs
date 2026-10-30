using UnityEngine;
public class BossBehavior : StateMachineBehaviour
{
    public int index;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var game=animator.GetComponent<SpaceBossGame>();
        if(game!=null) game.BossStateUpdate(index);
    }
}
