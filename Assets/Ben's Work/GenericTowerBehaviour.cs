namespace GameJam.Towers
{
    using System.Collections;
    using System.Collections.Generic;
    using RobbieWagnerGames.StrategyCombat.Units;
    using UnityEngine;

    [System.Serializable]
    public class TowerInfo
    {
        [SerializeField] public string name;
        [SerializeField] public int attackPower;
        [SerializeField] public float attackCooldown;

        public TowerInfo(string name, int power, float cooldown)
        {
            attackPower = power;
            attackCooldown = cooldown;
        }
    }

    public class GenericTowerBehaviour : MonoBehaviour
    {
        [SerializeField] public string towerName = "Celery";
        [SerializeField] public int attackPower = 1;

        [SerializeField] public float attackCooldown = 2f;
        
        [SerializeField]
        protected SphereCollider rangeSphere;

        [SerializeField]
        protected GameObject projectile;
        
        [SerializeField]
        protected Transform bulletSpawnPos;
        
        [SerializeField]
        protected float rotateSpeed = 2f;

        [SerializeField]
        protected float bulletVelocity = 100f;

        protected List<TDEnemy> enemiesInRange;

        private float attackTime = 0f;

        private Vector3 neutralRotation;

        private IEnumerator rotationCoroutine;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            enemiesInRange = new List<TDEnemy>();
            neutralRotation = this.transform.forward;
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            if(enemiesInRange != null && enemiesInRange.Count > 0)
            {
                Vector3 targetPos = new Vector3(enemiesInRange[0].transform.position.x,
                                                0f,
                                                enemiesInRange[0].transform.position.z);
                if(FacingObject(targetPos))
                {
                    attackTime -= Time.deltaTime;
                    if(attackTime <= 0)
                    {
                        Attack();
                    }
                }
                else if(rotationCoroutine == null)
                {
                    rotationCoroutine = LookAt(targetPos);
                    StartCoroutine(rotationCoroutine);
                }
            }
            else if(FacingObject(neutralRotation) == false && rotationCoroutine == null)
            {
                rotationCoroutine = LookAt(neutralRotation);
                StartCoroutine(rotationCoroutine);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            TDEnemy enemyComponent = other.gameObject.GetComponent<TDEnemy>();
            if(enemyComponent != null)
            {
                enemiesInRange.Add(enemyComponent);
                enemyComponent.OnKillEnemy += RemoveEnemyFromSight;
                Debug.Log("enemy");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            TDEnemy enemyComponent = other.gameObject.GetComponent<TDEnemy>();
            if(enemyComponent != null)
            {
                RemoveEnemyFromSight(enemyComponent);
                enemyComponent.OnKillEnemy -= RemoveEnemyFromSight;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Attack()
        {
            attackTime = attackCooldown;
            GameObject bullet = Instantiate(projectile, bulletSpawnPos.position, bulletSpawnPos.rotation);
            Vector3 launchForceVector = Vector3.forward * bulletVelocity;
            bullet.GetComponent<Rigidbody>().AddRelativeForce(launchForceVector, ForceMode.Impulse);
            if(enemiesInRange.Count > 0)
            {
                enemiesInRange[0].Health -= attackPower;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool FacingObject(Vector3 target)
        {
            float facingAngle = 10;
            return Vector3.Angle(this.transform.forward, target - this.transform.position) < facingAngle;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void RotateTowards(Vector3 target)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);
            float step = rotateSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, target, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        private IEnumerator LookAt(Vector3 target)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);

            float time = 0;

            while (time < 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
                time += Time.deltaTime * rotateSpeed;

                yield return null;
            }

            rotationCoroutine = null;
            StopCoroutine(LookAt(target));
        }

        public void RemoveEnemyFromSight(TDEnemy enemy)
        {
            enemiesInRange.Remove(enemy);
        }

        public TowerInfo GetTowerInfo()
        {
            TowerInfo returnValue = new TowerInfo(towerName, attackPower, attackCooldown);

            return returnValue;
        }

        public void SetTowerInfo(TowerInfo info)
        {
            towerName = info.name;
            attackPower = info.attackPower;
            attackCooldown = info.attackCooldown;
        }
    }
}
