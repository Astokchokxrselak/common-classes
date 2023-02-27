using Common.Extensions;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Common.Pools
{
    /// <summary>
    /// A pool that assigns random GameObjects from a list to the array of pool objects.
    /// </summary>
    /// <returns></returns>
    public class MultiObjectPool : Pool
    {
        protected override bool CanTakeMultipleObjects => true;
        public override GameObject ChooseObject()
        {
            return objectReferences.Choice();
        }
    }
}