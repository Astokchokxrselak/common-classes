using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Common;
using Common3D;
using Common.Pools;

namespace Common
{
    public class Bullet3D : PoolObject
    {
        public IDamageable creator;
        public Vector3 direction;
        [HideInInspector]
        public Vector3 velocity;

        [System.Flags]
        public enum BulletState
        {
            Ricochetted = 1,
            Player = 2
        }
        public BulletState bulletState;
        BulletState firstBulletState;
        private void Awake()
        {
            firstBulletState = bulletState;
        }

        private void FixedUpdate()
        {
            NormalUpdate();
            RicochetUpdate();
            BulletUpdate();
            PlayerUpdate();
        }
        void NormalUpdate()
        {
            velocity = direction;
            if (bulletState == 0)
            {
                NormalAI();
            }
        }
        public virtual void NormalAI()
        {

        }

        void RicochetUpdate()
        {
            if ((bulletState & BulletState.Ricochetted) != 0)
            {
                RicochetAI();
            }
        }
        public virtual void RicochetAI()
        {

        }

        void PlayerUpdate()
        {
            if ((bulletState & BulletState.Player) != 0)
            {
                PlayerAI();
            }
        }
        public virtual void PlayerAI()
        {

        }

        bool hitEnemy;
        static readonly Collider[] Overlaps = new Collider[5];
        const float OverlapRadius = Mathf.PI / 2f;
        Timer attackPause = 0f;
        const float PostRicochetPause = 0.5f;
        bool Paused => attackPause.Max != 0;
        void UpdateBulletPause()
        {
            if (Paused)
            {
                if (attackPause.IncrementHit(true, true))
                {
                    attackPause = 0f;
                }
            }
        }
        void AttackIfNotPaused()
        {
            if (Paused || hitEnemy)
            {
                return;
            }

            /*
            if ((bulletState & BulletState.Player) == 0)
            {
                AttackPlayer();
            }
            else
            {
                AttackEnemy();
            }
            */
        }
        /*
        void AttackPlayer()
        {
            var overlapCount = Physics.OverlapSphereNonAlloc(transform.position, OverlapRadius, Overlaps);
            bool hitPlayer = false;
            for (int i = 0; i < overlapCount; i++)
            {
                if (!BossRushController.Player.IsInvulnerable() && Overlaps[i].gameObject.CompareTag("Player"))
                {
                    hitPlayer = true;
                }
                else if (Overlaps[i].gameObject.CompareTag("Weapon"))
                {
                    if (BossRushController.Weapon.Type == PlayerWeapon.Bat)
                    {
                        attackPause = PostRicochetPause;
                        var nearby = Physics.OverlapSphere(transform.position, 999f);
                        float nearestDistance = 0;
                        Collider nearest = null;
                        foreach (var candidate in nearby)
                        {
                            if (!candidate.CompareTag("Enemy"))
                            {
                                continue;
                            }
                            var distance = (candidate.transform.position - transform.position).sqrMagnitude;
                            if (distance > nearestDistance)
                            {
                                nearest = candidate;
                            }
                        }
                        if (nearest)
                            direction = (nearest.transform.position - transform.position).normalized * direction.magnitude;
                        else
                            direction = -direction;
                        bulletState = BulletState.Ricochetted | BulletState.Player;
                        hitPlayer = false;
                        BossRushController.Weapon.OnDealDamage(1f, null);
                    }
                }
            }
            if (hitPlayer)
            {
                BossRushController.Player.TakeDamage(1f, this.creator);
                Hit();
            }
        }
        void AttackEnemy()
        {
            var overlapCount = Physics.OverlapSphereNonAlloc(transform.position, OverlapRadius, Overlaps);
            IDamageable hitEnemy = null;
            for (int i = 0; i < overlapCount; i++)
            {
                print(Overlaps[i]);
                if (Overlaps[i].gameObject.CompareTag("Enemy"))
                {
                    hitEnemy = Overlaps[i].GetComponent<IDamageable>();
                    break;
                }
            }
            if (hitEnemy != null)
            {
                hitEnemy.TakeDamage(1f, BossRushController.Player);
                Hit();
            }
        }
                */
        void BulletUpdate()
        {
            transform.position += velocity * Time.fixedDeltaTime;
            UpdateBulletPause();
            AttackIfNotPaused();
        }

        void Hit()
        {
            hitEnemy = true;
            Destroy();
        }
        void Die()
        {
            Destroy();
        }
        private void OnEnable()
        {
            bulletState = firstBulletState;
            hitEnemy = false;
        }
        private void OnBecameInvisible()
        {
            if (transform && gameObject && gameObject.activeInHierarchy)
                Die();
        }
    }
}