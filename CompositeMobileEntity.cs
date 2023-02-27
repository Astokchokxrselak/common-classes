using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Common
{
    public class CompositeMobileEntity : MobileEntity
    {
        int sortingOrderOffset = 0;
        public void SetSortingOrderOffset(int nOrder)
        {
            sortingOrderOffset = nOrder;
            for (int i = 0; i < myRenderers.Length; i++)
            {
                myRenderers[i].sortingOrder += nOrder - sortingOrderOffset;
            }
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
            myRenderers = GetComponentsInChildren<SpriteRenderer>();
            myColliders = GetComponentsInChildren<Collider2D>();
        }
    }
}
