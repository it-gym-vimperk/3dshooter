using UnityEngine;

[System.Serializable]
public class Wave
{
    public int Amount; // Počet nepřátel ve vlně.
    public GameObject Prefab; // Prefab nepřítele, který má být spawnován.
    public float SpawnDuration; // Časový interval mezi spawnováním jednotlivých nepřátel.
}