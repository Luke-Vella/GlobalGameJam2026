using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenManager : MonoBehaviour
{
    private Transform player;

    public Transform[] slideAttackSpawnPoints;
    public Transform[] slamAttackSpawnPoints;

    public GameObject armouredSlideAttackRightPrefab;
    public GameObject armouredSlideAttackTopPrefab;
    public GameObject armouredSlideAttackLeftPrefab;
    public GameObject armouredSlideAttackDownPrefab;

    public GameObject vulnerableSlideAttackRightPrefab;
    public GameObject vulnerableSlideAttackTopPrefab;
    public GameObject vulnerableSlideAttackLeftPrefab;
    public GameObject vulnerableSlideAttackDownPrefab;

    [Header("Kraken Settings")]
    public int tentacleHealthPoints = 3;
    public int vulnerableTentacleCount = 2;
    public int armouredTentacleCount = 2;
    public int repeatableActionRatioOnTen = 3; // Chance to repeat the same action
    public float bufferBetweenTentacles = 1f;
    public float bufferPerTentacleBetweenActions = 1f;

    public int[] tentacleHealth;

    // Start is called before the first frame update
    void Start()
    {
        tentacleHealth = new int[vulnerableTentacleCount + armouredTentacleCount];
        for (int i = 0; i < tentacleHealth.Length; i++)
        {
            tentacleHealth[i] = tentacleHealthPoints;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        NextAction("");
    }

    void NextAction(string previousAction)
    {
        int randomValue = Random.Range(1, 11);
        if (previousAction == "SlideAttack")
        {
            if (randomValue <= repeatableActionRatioOnTen)
            {
                StartCoroutine(SlideAttack());
            }
            else
            {
                StartCoroutine(SlamAttack());
            }
        }
        else if (previousAction == "SlamAttack")
        {
            if (randomValue <= repeatableActionRatioOnTen)
            {
                StartCoroutine(SlamAttack());
            }
            else
            {
                StartCoroutine(SlideAttack());
            }
        }
        else
        {
            // First action, choose randomly
            if (randomValue <= 5)
            {
                StartCoroutine(SlideAttack());
            }
            else
            {
                StartCoroutine(SlamAttack());
            }
        }
    }

    IEnumerator SlideAttack()
    {
        bool firstTentacle = true;
        List<int> usedSpawnPoints = new List<int>();
        int remainingTentacleCount = 0;
        for (int c = 0; c < tentacleHealth.Length; c++)
        {
            if (tentacleHealth[c] > 0)
            {
                remainingTentacleCount++;
                int closestSpawnPoint = 0;
                if (firstTentacle)
                {
                    float minDistance = Mathf.Infinity;
                    for (int i = 0; i < slideAttackSpawnPoints.Length; i++)
                    {
                        //find the closest spawn point to the player
                        float distance = Vector2.Distance(slideAttackSpawnPoints[i].position, player.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestSpawnPoint = i;
                        }
                    }

                    SpawnSlidingTentacle(closestSpawnPoint, c);
                    usedSpawnPoints.Add(closestSpawnPoint);
                    firstTentacle = false;
                }
                else
                {
                    int randomSpawnPoint;
                    do
                    {
                        randomSpawnPoint = Random.Range(0, slideAttackSpawnPoints.Length);
                    } while (usedSpawnPoints.Contains(randomSpawnPoint)); // Ensure it's not the same as a used spawn point

                    SpawnSlidingTentacle(randomSpawnPoint, c);
                    usedSpawnPoints.Add(randomSpawnPoint);
                }
            }
            yield return new WaitForSeconds(bufferBetweenTentacles);
        }
        if(firstTentacle)
        {
            // All tentacles are destroyed, handle accordingly
        }

        yield return new WaitForSeconds(bufferPerTentacleBetweenActions);
        NextAction("SlideAttack");
        yield break;
    }

    void SpawnSlidingTentacle(int spawnPoint, int tentacleNumber)
    {
        switch (spawnPoint)
        {
            // Right
            case 0:
            case 1:
            case 2:
            case 3: 
                if (tentacleNumber <= vulnerableTentacleCount)
                {
                    GameObject tentacle = Instantiate(vulnerableSlideAttackRightPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                else
                {
                    GameObject tentacle = Instantiate(armouredSlideAttackRightPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                break;

            // Top
            case 4:
            case 5:
            case 6:
            case 7:
                if (tentacleNumber <= vulnerableTentacleCount)
                {
                    GameObject tentacle = Instantiate(vulnerableSlideAttackTopPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                else
                {
                    GameObject tentacle = Instantiate(armouredSlideAttackTopPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                break;

            // Left
            case 8:
            case 9:
            case 10:
            case 11:
                if (tentacleNumber <= vulnerableTentacleCount)
                {
                    GameObject tentacle = Instantiate(vulnerableSlideAttackLeftPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                else
                {
                    GameObject tentacle = Instantiate(armouredSlideAttackLeftPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                break;

            // Down
            case 12:
            case 13:
            case 14:
            case 15:
                if (tentacleNumber <= vulnerableTentacleCount)
                {
                    GameObject tentacle = Instantiate(vulnerableSlideAttackDownPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                else
                {
                    GameObject tentacle = Instantiate(armouredSlideAttackDownPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                }
                break;
        }
    }

    IEnumerator SlamAttack()
    {
        // Implement Slam Attack Logic
        NextAction("SlamAttack");
        yield break;
    }
}
