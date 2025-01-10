using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private float maxHp;
    [SerializeField] private int maxBullets;
    [SerializeField] protected float bulletForce;

    [Header("Ui")]
    [SerializeField] private HpBar hpBar;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform gunHead;
    [SerializeField] protected Magazine magazine;
    [SerializeField] private GameObject hitEffect;
 
    private float currentHp;
    private bool isDying;
    
    private void Start()
    {
        for (int i = 0; i < maxBullets; i++)
        {
            magazine.Add(Instantiate(bulletPrefab));
        }

        currentHp = maxHp;
        hpBar.Set(1);
    }

    public void HitByBullet(Bullet bullet)
    {
        if (!isDying && bullet.CanBeCollected())
        {
            magazine.Add(bullet);
        }
        else if (bullet.CanHurt && bullet.SourceTag != gameObject.tag)
        {
            var damageForce = bullet.transform.forward.normalized * bullet.HitForce;
            TakeDamage(bullet.Damage, damageForce);
            bullet.CanHurt = false;
        }
    }

    private void TakeDamage(float damage, Vector3 force)
    {
        currentHp -= damage;
        hpBar.Set(currentHp / maxHp);

        var rigidBody = GetComponent<Rigidbody>();
     
        if (currentHp <= 0)
        {
            isDying = true;
            if (rigidBody != null)
            {
                rigidBody.isKinematic = false;
                rigidBody.AddForce(force, ForceMode.Impulse);
            }

            var agent = GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                Destroy(agent);
            }

            Destroy(gameObject, 2);
            hpBar.gameObject.SetActive(false);
        }
    }


    protected void Attack()
    {
        var bullet = magazine.GetBullet();
        bullet.transform.position = gunHead.transform.position;
        bullet.transform.rotation = gunHead.transform.rotation;
        bullet.Shoot(bulletForce, tag);
    }

    protected Bullet GetNearestBullet()
    {
        return FindObjectsOfType<Bullet>() // Find all Bullet objects in the scene
            .Where(bullet => bullet.CanBeCollected()) // Filter bullets that can be collected
            .OrderBy(bullet =>
                Vector3.Distance(transform.position, bullet.transform.position)) // Order by distance to player
            .FirstOrDefault();
    }
}
    