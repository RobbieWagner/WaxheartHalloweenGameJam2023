namespace GameJam.Towers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GenericTowerBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected SphereCollider rangeSphere;

        [SerializeField]
        protected GameObject projectile;
        
        [SerializeField]
        protected Transform bulletSpawnPos;

        [SerializeField]
        protected float attackCooldown = 2f;
        
        [SerializeField]
        protected float rotateSpeed = 2f;

        [SerializeField]
        protected float bulletVelocity = 100f;

        protected List<Transform> enemiesInRange;

        private float attackTime = 0f;

        private Vector3 neutralRotation;

        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            enemiesInRange = new List<Transform>();
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

                RotateTowards(targetPos);
            }
            else if(FacingObject(neutralRotation) == false)
            {
                RotateTowards(neutralRotation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Enemy")
            {
                enemiesInRange.Add(other.gameObject.transform);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Enemy")
            {
                enemiesInRange.Remove(other.gameObject.transform);
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
            float step = rotateSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, target, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}
