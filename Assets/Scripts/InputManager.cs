using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

    public bool PrimaryPressed { get; private set; }
    public bool SecondaryPressed { get; private set; }
    public bool ConfirmPressed { get; private set; }
    public bool CancelPressed { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // RESET one-frame inputs
        PrimaryPressed = false;
        SecondaryPressed = false;
        ConfirmPressed = false;
        CancelPressed = false;
    }

    // These get called by the Input System
    public void OnMove(Vector2 value) => Move = value;
    public void OnLook(Vector2 value) => Look = value;

    public void OnPrimary() => PrimaryPressed = true;
    public void OnSecondary() => SecondaryPressed = true;
    public void OnConfirm() => ConfirmPressed = true;
    public void OnCancel() => CancelPressed = true;
}
