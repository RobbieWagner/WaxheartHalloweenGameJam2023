namespace GameJam.Towers
{
    using UnityEngine;

    public class GenericBullet : MonoBehaviour
    {
        public float lifeTime = 5f;
        [SerializeField] private bool ignoreCollisions = false;

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Enemy" && !ignoreCollisions)
            {
                Destroy(gameObject);
            }
        }
    }
}
