using System.Collections.Generic;
using UnityEngine;

// Skript reprezentuje zásobník na náboje, který spravuje jejich přidávání a odebírání.
public class Magazine : MonoBehaviour
{
    public int Capacity; // Maximální kapacita zásobníku.

    private List<Bullet> bullets; // Seznam nábojů aktuálně v zásobníku.

    // Inicializace seznamu nábojů při spuštění.
    private void Awake()
    {
        bullets = new List<Bullet>();
    }

    // Vrací počet nábojů aktuálně v zásobníku.
    public int GetBulletsCount()
    {
        return bullets.Count;
    }

    // Přidá náboj do zásobníku, pokud je místo. Vrací true, pokud přidání proběhlo úspěšně.
    public bool Add(Bullet bullet)
    {
        // Zkontroluje, zda je v zásobníku místo.
        if (bullets.Count < Capacity)
        {
            // Zjistí, zda má náboj komponentu Rigidbody.
            var rigidbody = bullet.GetComponent<Rigidbody>();

            // Pokud ano, odstraní ji, aby náboj neinteragoval s fyzikálním systémem.
            if (rigidbody != null)
            {
                Destroy(rigidbody);
            }

            // Nastaví náboj jako dítě zásobníku.
            bullet.transform.SetParent(transform);

            // Přidá náboj do seznamu.
            bullets.Add(bullet);

            // Nastaví pozici náboje v zásobníku na základě jeho pořadí.
            bullet.transform.localPosition = Vector3.up * bullets.Count * 0.5f;

            return true;
        }

        // Pokud je zásobník plný, vrátí false.
        return false;
    }

    // Odebere poslední náboj ze zásobníku a vrátí ho. Pokud je zásobník prázdný, vrátí null.
    public Bullet GetBullet()
    {
        // Zkontroluje, zda je v zásobníku alespoň jeden náboj.
        if (bullets.Count > 0)
        {
            // Získá poslední přidaný náboj.
            var bullet = bullets[^1];

            // Odstraní náboj ze seznamu.
            bullets.Remove(bullet);

            return bullet;
        }

        // Pokud je zásobník prázdný, vrátí null.
        return null;
    }
}