using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    [SerializeField] PlayerController player;

    public void AnimationTrigger()
    {
        if (player != null)
        {
            player.AttackRaycast();
        }
    }
}
