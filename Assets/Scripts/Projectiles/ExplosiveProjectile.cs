using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ExplosionController : ProjectileController
{
    void Start()
    {
        animator = GetComponent<Animator>();
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Explosion")
            {
                stats.duration = clip.length;
            }
        }
    }

    void Update()
    {

    }
}
