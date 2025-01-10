using UnityEngine;

// Skript zajišťuje, že objekt s tímto skriptem bude sledovat pozici hráče (Player),
// přičemž zachová pevně nastavený posun (offset) mezi pozicemi.
public class FollowPlayer : MonoBehaviour
{
    // Reference na objekt hráče, kterého chceme sledovat.
    [SerializeField] private Player player;
    
    // Proměnná pro uložení výchozího posunu mezi objektem a hráčem.
    private Vector3 offset;

    // Inicializace se provádí při spuštění hry.
    private void Start()
    {
        // Výpočet počátečního posunu mezi pozicí objektu a hráče.
        offset = transform.position - player.transform.position;
    }

    // Aktualizace pozice objektu při každém snímku hry.
    private void Update()
    {
        // Pokud je hráč správně nastaven (není null), posuneme objekt podle hráče.
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}