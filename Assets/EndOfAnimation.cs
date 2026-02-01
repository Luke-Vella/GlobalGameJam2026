using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndOfAnimation : MonoBehaviour
{
    public UnityEvent AnimationEnded;


    public void OnAnimationEnd()
    {
        AnimationEnded.Invoke();
    }
}
