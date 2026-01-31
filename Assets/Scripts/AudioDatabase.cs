using UnityEngine;

public class AudioDatabase : MonoBehaviour
{
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip backClickClip;
    [SerializeField] private AudioClip bubblesClip;
    [SerializeField] private AudioClip firingProjectilesClip;
    [SerializeField] private AudioClip pickingUpFuelCellClip;
    [SerializeField] private AudioClip airPocketClip;
    [SerializeField] private AudioClip splashClip;
    [SerializeField] private AudioClip alarmClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip tentaclesClip;
    [SerializeField] private AudioClip callClip;
    [SerializeField] private AudioClip openHatchClip;
    [SerializeField] private AudioClip sinkingClip;
    [SerializeField] private AudioClip connectingFuelCellsClip;
    [SerializeField] private AudioClip outOfWaterClip;
    [SerializeField] private AudioClip damageHitClip;
    [SerializeField] private AudioClip toggleBetweenMasksClip;
    [SerializeField] private AudioClip sonarOnOffClip;
    [SerializeField] private AudioClip lightOnOffClip;
    [SerializeField] private AudioClip pageTurnClip;
    [SerializeField] private AudioClip speedBurstClip;
    [SerializeField] private AudioClip speedBurstStopClip;
    [SerializeField] private AudioClip swordFishChargeClip;
    [SerializeField] private AudioClip pauseMenuClip;

    public static AudioDatabase Instance;

    public AudioClip ClickClip { get => clickClip; set => clickClip = value; }
    public AudioClip BackClickClip { get => backClickClip; set => backClickClip = value; }
    public AudioClip BubblesClip { get => bubblesClip; set => bubblesClip = value; }
    public AudioClip FiringProjectilesClip { get => firingProjectilesClip; set => firingProjectilesClip = value; }
    public AudioClip PickingUpFuelCellClip { get => pickingUpFuelCellClip; set => pickingUpFuelCellClip = value; }
    public AudioClip AirPocketClip { get => airPocketClip; set => airPocketClip = value; }
    public AudioClip SplashClip { get => splashClip; set => splashClip = value; }
    public AudioClip AlarmClip { get => alarmClip; set => alarmClip = value; }
    public AudioClip GameOverClip { get => gameOverClip; set => gameOverClip = value; }
    public AudioClip TentaclesClip { get => tentaclesClip; set => tentaclesClip = value; }
    public AudioClip CallClip { get => callClip; set => callClip = value; }
    public AudioClip OpenHatchClip { get => openHatchClip; set => openHatchClip = value; }
    public AudioClip SinkingClip { get => sinkingClip; set => sinkingClip = value; }
    public AudioClip ConnectingFuelCellsClip { get => connectingFuelCellsClip; set => connectingFuelCellsClip = value; }
    public AudioClip OutOfWaterClip { get => outOfWaterClip; set => outOfWaterClip = value; }
    public AudioClip DamageHitClip { get => damageHitClip; set => damageHitClip = value; }
    public AudioClip ToggleBetweenMasksClip { get => toggleBetweenMasksClip; set => toggleBetweenMasksClip = value; }
    public AudioClip SonarOnOffClip { get => sonarOnOffClip; set => sonarOnOffClip = value; }
    public AudioClip LightOnOffClip { get => lightOnOffClip; set => lightOnOffClip = value; }
    public AudioClip PageTurnClip { get => pageTurnClip; set => pageTurnClip = value; }
    public AudioClip SpeedBoostClip { get => speedBurstClip; set => speedBurstClip = value; }
    public AudioClip SpeedBurstStopClip { get => speedBurstStopClip; set => speedBurstStopClip = value; }
    public AudioClip PauseMenuClip { get => pauseMenuClip; set => pauseMenuClip = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
