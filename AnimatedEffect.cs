using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Pools;

namespace Common
{
    public class AnimatedEffect : MonoBehaviour
    {
        Animator animator;
        PoolObject poolObject;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            poolObject = GetComponent<PoolObject>();
        }

        void Update()
        {
            var normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (normTime >= 1)
            {
                poolObject.Destroy();
            }
        }
    }
}