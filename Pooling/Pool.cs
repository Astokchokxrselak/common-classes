using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Common.Extensions;
using UnityEditor;

namespace Common.Pools
{
    /// <summary>
    /// A general purpose pool that supports respawning objects and accessing them. Can be used for effects. 
    /// </summary>
    /// <returns></returns>
    public class Pool : BasePool
    {
        public GameObject Object => objectReferences[0];
    }
}