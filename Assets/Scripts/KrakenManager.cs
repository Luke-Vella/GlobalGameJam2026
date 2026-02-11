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
    public GameObject armouredSlideAttackBottomPrefab;

    public GameObject vulnerableSlideAttackRightPrefab;
    public GameObject vulnerableSlideAttackTopPrefab;
    public GameObject vulnerableSlideAttackLeftPrefab;
    public GameObject vulnerableSlideAttackBottomPrefab;

    [Header("Kraken Settings")]
    public int tentacleHealthPoints = 3;
    public int vulnerableTentacleCount = 2;
    public int armouredTentacleCount = 2;
    public int repeatableActionRatioOnTen = 3; // Chance to repeat the same action
    public float bufferBetweenTentacles = 1f;
    public float bufferPerTentacleBetweenActions = 1f;

    [SerializeField]
    public Tentacle[] tentacleList;

    // Start is called before the first frame update
    void Start()
    {
        tentacleList = new Tentacle[vulnerableTentacleCount + armouredTentacleCount];
        for (int i = 0; i < tentacleList.Length; i++)
        {
            bool isVulnerable = i < vulnerableTentacleCount;
            tentacleList[i] = new Tentacle(i, tentacleHealthPoints, isVulnerable);
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(NextAction(""));
    }

    IEnumerator NextAction(string previousAction)
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
        yield break;
    }

    IEnumerator SlideAttack()
    {
        bool firstTentacle = true;
        List<int> usedSpawnPoints = new List<int>();
        //int remainingTentacleCount = 0;
        for (int c = 0; c < tentacleList.Length; c++)
        {
            if (tentacleList[c].healthPoints > 0)
            {
                //remainingTentacleCount++;
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
        StartCoroutine(NextAction("SlideAttack"));
        yield break;
    }

    void SpawnSlidingTentacle(int spawnPoint, int tentacleNumber)
    {
        GameObject tentacle;
        SlideTentacle slideTentacleSetup;
        if (tentacleList[tentacleNumber].healthPoints <= tentacleHealthPoints)
        {
            if (spawnPoint == 0 || spawnPoint == 1 || spawnPoint == 2 || spawnPoint == 3)
            {
                tentacle = Instantiate(vulnerableSlideAttackRightPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.left;
            }
            else if (spawnPoint == 4 || spawnPoint == 5 || spawnPoint == 6 || spawnPoint == 7)
            {
                tentacle = Instantiate(vulnerableSlideAttackTopPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.down;
            }
            else if (spawnPoint == 8 || spawnPoint == 9 || spawnPoint == 10 || spawnPoint == 11)
            {
                tentacle = Instantiate(vulnerableSlideAttackLeftPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.right;
            }
            else
            {
                tentacle = Instantiate(vulnerableSlideAttackBottomPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.up;
            }

            tentacle?.transform.SetParent(slideAttackSpawnPoints[spawnPoint], true); // keep world scale
            slideTentacleSetup.remainingHealthPoints = tentacleList[tentacleNumber].healthPoints;
            slideTentacleSetup.tentacleNumber = tentacleNumber;
            slideTentacleSetup.krakenManager = this;
        }
        else
        {
            if (spawnPoint == 0 || spawnPoint == 1 || spawnPoint == 2 || spawnPoint == 3)
            {
                tentacle = Instantiate(armouredSlideAttackRightPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.left;
            }

            else if (spawnPoint == 4 || spawnPoint == 5 || spawnPoint == 6 || spawnPoint == 7)
            {
                tentacle = Instantiate(armouredSlideAttackTopPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.down;
            }

            else if (spawnPoint == 8 || spawnPoint == 9 || spawnPoint == 10 || spawnPoint == 11)
            {
                tentacle = Instantiate(armouredSlideAttackLeftPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.right;
            }

            else
            {
                tentacle = Instantiate(armouredSlideAttackBottomPrefab, slideAttackSpawnPoints[spawnPoint].position, Quaternion.identity);
                slideTentacleSetup = tentacle?.GetComponent<SlideTentacle>();
                slideTentacleSetup.slideDirection = Vector2.up;
            }

            tentacle.transform.SetParent(slideAttackSpawnPoints[spawnPoint], true); // keep world scale
            slideTentacleSetup.remainingHealthPoints = tentacleList[tentacleNumber].healthPoints;
            slideTentacleSetup.tentacleNumber = tentacleNumber;
            slideTentacleSetup.krakenManager = this;
        }
    }

    IEnumerator SlamAttack()
    {
        // Implement Slam Attack Logic
        StartCoroutine(NextAction("SlamAttack"));
        yield break;
    }
}
