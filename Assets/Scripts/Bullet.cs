using System;
using UnityEngine;

// Skript reprezentuje náboj, který může být vystřelen, způsobit poškození a být sbírán.
public class Bullet : MonoBehaviour
{
    public int Id;
    public bool CanHurt; // Určuje, zda náboj může způsobit poškození.
    public string SourceTag; // Identifikuje původce náboje (např. hráč nebo nepřítel).
    public float HitForce => hitForce; // Síla, kterou náboj způsobí při zásahu.
    public float Damage => damage; // Poškození způsobené nábojem.

    [Header("Settings")]
    [SerializeField] private float damage = 1; // Poškození způsobené nábojem.
    [SerializeField] private float hitForce = 6; // Síla nárazu náboje.
    [SerializeField] private float minFlyingSpeed = 1; // Minimální rychlost pro možnost sbírání náboje.

    [Header("Visuals")]
    [SerializeField] private Material flyingMaterial; // Materiál, když náboj létá.
    [SerializeField] private Material collectedMaterial; // Materiál, když je náboj sbírán.
    [SerializeField] private MeshRenderer renderer; // Renderer pro zobrazení materiálu.

    private Rigidbody rigidbody; // RigidBody komponenta pro pohyb a interakci s fyzikou.

    private static int LastBulletId = -1;

    
    private void Awake()
    {
        LastBulletId++;
        Id = LastBulletId;
    }

    // Metoda pro vystřelení náboje s danou silou a označením původce.
    public void Shoot(float force, string sourceTag)
    {
        CanHurt = true; // Povolení poškozování.
        SourceTag = sourceTag; // Nastavení původce náboje.
        transform.SetParent(null); // Odpojí náboj od jakéhokoli rodičovského objektu.
        transform.localScale = Vector3.one;
        rigidbody = gameObject.AddComponent<Rigidbody>(); // Přidá Rigidbody pro fyzikální simulaci.
        rigidbody.drag = 0.5f; // Nastaví odpor vzduchu pro zpomalení náboje.
        rigidbody.AddForce(transform.forward * force, ForceMode.Impulse); // Vystřelí náboj vpřed s danou silou.
        
    }

    // Zkontroluje, zda náboj může být sbírán (na základě rychlosti nebo stavu CanHurt).
    public bool CanBeCollected()
    {
        return rigidbody != null && (rigidbody.velocity.magnitude < minFlyingSpeed || !CanHurt);
    }

    // Metoda volaná každým snímkem pro kontrolu stavu náboje a změnu materiálu.
    private void Update()
    {
        // Pokud má náboj Rigidbody, kontroluje, zda může být sbírán a mění materiál.
        if (rigidbody != null)
        {
            renderer.sharedMaterial = CanBeCollected()
                    ? collectedMaterial // Pokud může být sbírán, nastaví materiál na sbíraný.
                    : flyingMaterial; // Jinak nechá materiál létající.
        }
        else
        {
            // Pokud nemá Rigidbody (tzn. už je sebran), nastaví materiál na sbíraný.
            renderer.sharedMaterial = collectedMaterial;
        }
    }
}