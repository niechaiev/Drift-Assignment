using MyExtensions;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private PressReleaseHandler handbreakHandler;
    [SerializeField] private PressReleaseHandler forwardHandler;
    [SerializeField] private PressReleaseHandler reverseHandler;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private bool _isForward;
    private bool _isReverse;
    private bool _isHandbrake;
    private bool _isSteering;
    private float _horizontalAxis;
    
    public bool IsForward => _isForward;

    public bool IsReverse => _isReverse;

    public bool IsHandbrake => _isHandbrake;
    
    public bool IsSteering => _isSteering;

    public float HorizontalAxis => _horizontalAxis;

    private void Awake()
    {
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        canvasGroup.ShowCanvasGroup(false);
#else
        forwardHandler.OnPressed = () => _isForward = true;
        forwardHandler.OnReleased = () => _isForward = false;
        reverseHandler.OnPressed = () => _isReverse = true;
        reverseHandler.OnReleased = () => _isReverse = false;
        handbreakHandler.OnPressed = () => _isHandbrake = true;
        handbreakHandler.OnReleased = () => _isHandbrake = false;
#endif
    }

    private void Update()
    {
#if PLATFORM_STANDALONE_WIN || UNITY_EDITOR
        _isForward = Input.GetKey(KeyCode.W);
        _isReverse = Input.GetKey(KeyCode.S);
        _isHandbrake = Input.GetKey(KeyCode.Space);
        _isSteering = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        _horizontalAxis = Input.GetAxis("Horizontal");
#else
        _isSteering = joystick.Horizontal != 0;
        _horizontalAxis = joystick.Horizontal;
#endif
    }
}