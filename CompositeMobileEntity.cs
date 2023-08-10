using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.Rendering;

using Common.Extensions;

namespace Common
{
    public class CompositeMobileEntity : MobileEntity
    {
        SortingGroup _myGroup;
        new public int SortingOrder
        {
            get => _myGroup.sortingOrder;
            set => _myGroup.sortingOrder = value;
        }
        public void SetSortingOrderOffset(int nOrder)
        {
            _myGroup.sortingOrder = nOrder;
        }

        SpriteRenderer[] myRenderers;
        public void ToggleAllRenderers()
        {
            for (int i = 0; i < myRenderers.Length; i++)
            {
                myRenderers[i].enabled = !myRenderers[i].enabled;
            }
        }

        public void EnableAllRenderers()
        {
            for (int i = 0; i < myRenderers.Length; i++)
            {
                myRenderers[i].enabled = true;
            }
        }

        public void DisableAllRenderers()
        {
            for (int i = 0; i < myRenderers.Length; i++)
            {
                myRenderers[i].enabled = false;
            }
        }

        public void SetMaterials(Material material)
        {
            for (int i = 0; i < myRenderers.Length; i++)
            {
                myRenderers[i].material = material;
            }
        }

        Collider2D[] myColliders;
        public void ToggleAllColliders()
        {
            for (int i = 0; i < myColliders.Length; i++)
            {
                myColliders[i].enabled = !myColliders[i].enabled;
            }
        }
        public void DisableAllColliders()
        {
            for (int i = 0; i < myColliders.Length; i++)
            {
                myColliders[i].enabled = false;
            }
        }
        public void EnableAllColliders()
        {
            for (int i = 0; i < myColliders.Length; i++)
            {
                myColliders[i].enabled = true;
            }
        }
        public override void OnAwake()
        {
            _myGroup = this.GetOrAddComponent<SortingGroup>();
            _myGroup.sortAtRoot = true;

            myRenderers = GetComponentsInChildren<SpriteRenderer>();
            myColliders = GetComponentsInChildren<Collider2D>();
        }
    }
}
