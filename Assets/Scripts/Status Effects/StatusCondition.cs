using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatus", menuName = "Game/Status")]
public class StatusCondition : ScriptableObject
{
    public Sprite icon;
    public bool hidden;

    public List<StatusStates> states;
    public List<DamageOverTime> damageOverTimes;
    public List<StatEffector> statEffectors;

    [HideInInspector] public float remainingDuration;
    public float duration;
    public float delay;
    public bool delayUsesDuration;
    public int maxStacks;

    public StatusCondition sequentialEffect;
    [HideInInspector] public CharacterController owner;

    public StatusCondition Clone()
    {
        StatusCondition clone = ScriptableObject.CreateInstance<StatusCondition>();

        clone.name = this.name;
        clone.icon = this.icon;
        clone.hidden = this.hidden;
        clone.remainingDuration = this.remainingDuration;
        clone.duration = this.duration;
        clone.delay = this.delay;
        clone.delayUsesDuration = this.delayUsesDuration;
        clone.maxStacks = this.maxStacks;

        clone.states = states != null ? new List<StatusStates>(states) : new List<StatusStates>();
        clone.damageOverTimes = damageOverTimes.Select(d => d.Clone()).ToList();
        clone.statEffectors = statEffectors.Select(e => e.Clone()).ToList();

        clone.sequentialEffect = sequentialEffect != null ? sequentialEffect.Clone() : null;

        return clone;
    }
}

