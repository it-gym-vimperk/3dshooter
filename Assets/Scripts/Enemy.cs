using UnityEngine;
using UnityEngine.AI;

// Skript reprezentuje nepřítele ve hře, který dokáže útočit na hráče nebo hledat náboje.
// Dědí funkcionalitu ze třídy Character.
public class Enemy : Character
{
    public int Id;
    
    [Header("Settings")]
    [SerializeField] private NavMeshAgent agent; // Agent pro pohyb nepřítele po NavMesh.
    [SerializeField] private Rigidbody rb; // Rigidbody nepřítele pro fyzikální interakce.
    [SerializeField] private float attackInterval; // Časový interval mezi jednotlivými útoky.
    [SerializeField] private float attackDistance; // Maximální vzdálenost, na kterou může nepřítel útočit.
 
    public static int EnemiesCount; // Počet aktuálně aktivních nepřátel.
   
    private static int LastEnemyId = -1;
    private float attackTimer; // Časovač pro řízení útoků.

    // Metoda volaná při inicializaci objektu.
    private void Awake()
    {
        LastEnemyId++;
        Id = LastEnemyId;
            
        // Zvýší počet nepřátel při vytvoření instance.
        EnemiesCount++;
    }

    // Metoda volaná při zničení objektu.
    private void OnDestroy()
    {
        // Sníží počet nepřátel při zničení instance.
        EnemiesCount--;
    }

    // Metoda volaná každý snímek hry.
    private void Update()
    {
        // Najde instanci hráče ve scéně.
        var player = FindObjectOfType<Player>();
        
        // Pokud existuje hráč i NavMesh agent.
        if (player != null && agent != null)
        {
            // Pokud nepřítel nemá náboje.
            if (magazine.GetBulletsCount() == 0)
            {
                // Povolit pohyb agenta.
                agent.isStopped = false;

                // Najde nejbližší náboj.
                var bulletToCollect = GetNearestBullet();

                // Pokud existuje náboj, nastaví agenta na jeho pozici.
                if (bulletToCollect != null)
                {
                    agent.SetDestination(bulletToCollect.transform.position);
                }
            }
            else
            {
                // Pokud je hráč v dosahu útoku.
                if ((transform.position - player.transform.position).magnitude < attackDistance)
                {
                    // Zastaví agenta.
                    agent.isStopped = true;

                    // Natočí nepřítele směrem k hráči.
                    transform.LookAt(player.transform.position);
                    var rotation = transform.localEulerAngles;
                    rotation.x = 0;
                    transform.localEulerAngles = rotation;
                    
                    // Pokud je čas na útok.
                    if (attackTimer <= 0)
                    {
                        // Resetuje časovač.
                        attackTimer = attackInterval;

                        // Spustí útok.
                        Attack();
                    }
                    else
                    {
                        // Sníží časovač o dobu uplynulou od posledního snímku.
                        attackTimer -= Time.deltaTime;
                    }
                }
                else
                {
                    // Pokud je hráč mimo dosah útoku, sleduje jeho pozici.
                    agent.SetDestination(player.transform.position);

                    // Povolit pohyb agenta.
                    agent.isStopped = false;
                }
            }
        }
    }

    // Metoda volaná při kolizi s jiným objektem.
    private void OnCollisionEnter(Collision other)
    {
        // Pokud kolize není s nábojem, ukončí metodu.
        if (!other.gameObject.CompareTag("Bullet"))
            return;

    
        // Získá komponentu Bullet z objektu.
        var bullet = other.gameObject.GetComponent<Bullet>();
        Debug.Log($"Enemy {Id} hit by bullet {bullet.Id}", gameObject);
        // Spustí logiku pro zásah nepřítele nábojem.
        HitByBullet(bullet);
    }
}