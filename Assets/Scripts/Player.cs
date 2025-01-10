using System;
using UnityEngine;

// Skript reprezentuje hráče a jeho interakce s prostředím a ovládání.
public class Player : Character
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverWindow; // UI okno zobrazované při konci hry.

    [SerializeField] private CharacterController characterController; // Komponenta pro správu pohybu hráče.
    [SerializeField] private TrajectoryVisualizer trajectoryVisualizer; // Vizualizátor trajektorie střely.

    [Header("Player Settings")]
    [SerializeField] private float walkSpeed; // Rychlost pohybu hráče.
    [SerializeField] private float rotationSpeed; // Rychlost otáčení hráče.
    [SerializeField] private float gravity = 0.5f; // Gravitace ovlivňující hráče.

    // Inicializace, skryje okno pro konec hry.
    private void Awake()
    {
        gameOverWindow.SetActive(false);
    }

    // Hlavní logika volaná v každém snímku.
    private void Update()
    {
        // Zpracuje vstupy od hráče.
        UpdateInput();

        // Aplikuje gravitaci na hráče.
        UpdateGravity();

        // Aktualizuje vizualizaci trajektorie střely.
        UpdateTrajectory();

        // Spustí útok, pokud hráč stiskne mezerník a má náboje.
        if (Input.GetKeyDown(KeyCode.Space) && magazine.GetBulletsCount() > 0)
        {
            Attack();
        }
    }

    // Aktualizace vstupů pro pohyb a rotaci hráče.
    private void UpdateInput()
    {
        // Získá vstupy pro rotaci a pohyb.
        var rotationAmount = Input.GetAxis("Horizontal");
        var moveAmount = Input.GetAxis("Vertical");

        // Rotuje hráčem na základě vstupu.
        characterController.transform.Rotate(new Vector3(0, 1, 0) * (rotationAmount * rotationSpeed * Time.deltaTime));
        
        // Pohybuje hráčem ve směru jeho pohledu.
        characterController.Move(transform.forward * (walkSpeed * moveAmount * Time.deltaTime));
        
    }

    // Aplikuje gravitaci, pokud hráč není na zemi.
    private void UpdateGravity()
    {
        if (!characterController.isGrounded)
        {
            // Pohyb směrem dolů podle gravitační síly.
            characterController.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        }
    }

    // Aktualizace trajektorie střely podle dostupnosti nábojů.
    private void UpdateTrajectory()
    {
        if (magazine.GetBulletsCount() > 0)
        {
            // Pokud má hráč náboje, aktivuje vizualizátor a aktualizuje jeho sílu.
            trajectoryVisualizer.gameObject.SetActive(true);
            trajectoryVisualizer.UpdateLaunchForce(bulletForce);
        }
        else
        {
            // Pokud nemá náboje, deaktivuje vizualizátor.
            trajectoryVisualizer.gameObject.SetActive(false);
        }
    }

    // Metoda volaná při zničení hráče (např. při jeho smrti).
    private void OnDestroy()
    {
        // Zobrazí okno pro konec hry.
        gameOverWindow.SetActive(true);
    }

    // Metoda volaná při vstupu do kolizní oblasti.
    private void OnTriggerEnter(Collider other)
    {
        // Pokud kolize není s nábojem, ukončí metodu.
        if (!other.gameObject.CompareTag("Bullet"))
            return;

        // Získá komponentu Bullet z objektu a zpracuje zásah.
        var bullet = other.GetComponent<Bullet>();
        HitByBullet(bullet);
    }
}