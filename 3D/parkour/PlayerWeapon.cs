using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Parkour3D 
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        private PlayerManager _playerManager;
        public PlayerManager PlayerManager => _playerManager;
        private void Awake()
        {
            _playerManager = FindObjectOfType<PlayerManager>();
        }
        public abstract Vector3 WeaponLocalPosition { get; }
    }
}