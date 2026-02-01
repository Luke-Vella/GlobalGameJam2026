using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntryTrigger : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private GameObject AudioManagerObject;
    [SerializeField] private GameObject KrakenBoss;
    [SerializeField] private AudioClip newMusic;

    private AudioManager audioManager;
    private bool hasTriggered = false;

    void Start()
    {
        audioManager = AudioManagerObject.GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasTriggered)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Safety check in case singleton is missing
        if (FuelCellManager.Instance == null)
        {
            Debug.LogError("FuelCellManager Instance not found!");
            return;
        }

        if (FuelCellManager.Instance.fuelCellsAcquired >=
            FuelCellManager.Instance.fuelCellsRequired)
        {
            ChangeMusic();
            hasTriggered = true;
            KrakenBoss.SetActive(true);
            // Remove blocker so player can proceed
            Destroy(gameObject);
        }
    }

    void ChangeMusic()
    {
        audioManager.PlayBossMusic();
    }
}
