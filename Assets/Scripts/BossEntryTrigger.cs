using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntryTrigger : MonoBehaviour
{
    [SerializeField] private GameObject AudioManagerObject;
    [SerializeField] private GameObject KrakenBoss;

    private AudioManager audioManager;
    //private bool hasTriggered = false;

    void Start()
    {
        audioManager = AudioManagerObject.GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (hasTriggered)
        //    return;

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
            StartCoroutine(StartBossFight());
            // Remove blocker so player can proceed
            Destroy(gameObject);
        }
    }

    IEnumerator StartBossFight()
    {
        ChangeMusic();
        //hasTriggered = true;
        KrakenBoss.SetActive(true);
        yield break;
    }

    void ChangeMusic()
    {
        audioManager.PlayBossMusic();
    }
}