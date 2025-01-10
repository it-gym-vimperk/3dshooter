using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Správa vln nepřátel a zobrazení uživatelského rozhraní během hry.
public class WaveManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI enemiesAliveLabel; // Textové pole zobrazující počet aktuálně živých nepřátel.
    [SerializeField] private TextMeshProUGUI waveCounterLabel; // Textové pole zobrazující pořadí aktuální vlny.
    [SerializeField] private TextMeshProUGUI waveIntroductionLabel; // Textové pole pro zobrazení úvodu vlny.
    [SerializeField] private GameObject gameWonWindow; // UI okno, které se zobrazí po vítězství ve hře.

    [Header("Settings")]
    [SerializeField] private List<Wave> waves; // Seznam všech vln, které budou ve hře spuštěny.

    // Hlavní metoda, která spouští a spravuje postup jednotlivými vlnami.

    private int enemiesInWave;
    public IEnumerator Start()
    {
        // Skryj vítězné okno a úvodní text při spuštění.
        gameWonWindow.SetActive(false);
        waveIntroductionLabel.enabled = false;
        waveCounterLabel.enabled = false;

        // Krátká pauza před startem hry.
        yield return new WaitForSeconds(1);

        // Pro každou vlnu v seznamu waves.
        for (int i = 0; i < waves.Count; i++)
        {
            enemiesInWave = waves[i].Amount;
            // Nastav pocitadlo vln
            waveCounterLabel.enabled = true;
            waveCounterLabel.text = i + 1 + "/" + waves.Count;
            // Zobraz úvodní text aktuální vlny.
            waveIntroductionLabel.text = "Wave " + (i + 1);
            waveIntroductionLabel.enabled = true;

            // Pauza pro přečtení.
            yield return new WaitForSeconds(2);

            waveIntroductionLabel.enabled = false;

            // Spawnuj nepřátele ve vlně.
            for (int j = 0; j < waves[i].Amount; j++)
            {
                // Vytvoř instanci nepřítele z prefabu a nastav jeho pozici.
                var enemy = Instantiate(waves[i].Prefab);
                enemy.transform.position = Utils.GetRandomPointOnNavMesh(Vector3.zero, 100);

                // Pauza mezi spawny nepřátel.
                yield return new WaitForSeconds(waves[i].SpawnDuration);
            }

            // Počkej, dokud nejsou všichni nepřátelé zničeni.
            yield return new WaitUntil(() => Enemy.EnemiesCount == 0);
        }

        // Po dokončení všech vln zobraz vítězné okno.
        yield return new WaitForSeconds(2.0f);
        gameWonWindow.SetActive(true);
    }

    // Aktualizace počtu aktuálně živých nepřátel v každém snímku.
    private void Update()
    {
        // Nastav počet aktuálně živých nepřátel do textového pole.
        enemiesAliveLabel.text = Enemy.EnemiesCount.ToString();
    }
}