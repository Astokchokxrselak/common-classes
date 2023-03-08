using Parkour3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public PlayerWeapon weapon;
    private PlayerManager manager;
    private Transform pivot;
    private void Start()
    {
        manager = GetComponent<PlayerManager>();
    }
    private void Update()
    {
        weapon.transform.localPosition = weapon.WeaponLocalPosition;
    }
    private void OnEnable()
    {
        weapon.enabled = true;
    }
    private void OnDisable()
    {
        weapon.enabled = false;
    }
}
