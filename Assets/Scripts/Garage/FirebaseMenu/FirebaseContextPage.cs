using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace Garage.FirebaseMenu
{
    public class FirebaseContextPage : Page
    {
        [Header("Main State")]
        [SerializeField] private GameObject mainGameObject;
        [SerializeField] private TMP_Text mainTitleText;
        [SerializeField] private Button signInButton;
        [SerializeField] private Button signOutButton;
        [SerializeField] private Button signUpButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private TMP_Text saveDescriptionText;
        
        [Header("Sign State")]
        [SerializeField] private GameObject signGameObject;
        [SerializeField] private TMP_Text signText;
        [SerializeField] private Button signButton;
        [SerializeField] private TMP_InputField emailField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private Button goBackButton;
        public GameObject MainGameObject => mainGameObject;
        public GameObject SignGameObject => signGameObject;
        public TMP_Text MainTitleText => mainTitleText;
        public Button SignInButton => signInButton;
        public Button SignOutButton => signOutButton;
        public Button SignUpButton => signUpButton;
        public Button LoadButton => loadButton;
        public Button SaveButton => saveButton;
        public Button DeleteButton => deleteButton;
        public Button GoBackButton => goBackButton;
        public Button SignButton => signButton;
        public TMP_InputField EmailField => emailField;
        public TMP_InputField PasswordField => passwordField;
        
        public TMP_Text SaveDescriptionText => saveDescriptionText;

        private State _currentState;
        private NotSignedMenuState _notSignedMenuState;
        private SignedMenuState _signedMenuState;
        private SignUpState _signUpState;
        private SignInState _signInState;
        private FirebaseDatabase _database;
        private DataSnapshot _dataSnapshot;
        private DatabaseReference _reference;
        private FirebaseAuth _auth;
        private PlayerData _playerData;
        
        public State CurrentState => _currentState;
        public FirebaseDatabase Database => _database;
        public DataSnapshot DataSnapshot => _dataSnapshot;
        public DatabaseReference Reference => _reference;
        public NotSignedMenuState NotSignedMenuState => _notSignedMenuState;
        public SignedMenuState SignedMenuState => _signedMenuState;
        public SignUpState SignUpState => _signUpState;
        public SignInState SignInState => _signInState;

        public TMP_Text SignText => signText;

        public FirebaseAuth Auth => _auth;


        private void Awake()
        {
            _notSignedMenuState = new NotSignedMenuState(this);
            _signedMenuState = new SignedMenuState(this);
            _signUpState = new SignUpState(this);
            _signInState = new SignInState(this);
        }

        public void SwitchState(State newGameState)
        {
            if (_currentState != null)
                _currentState.LeaveState();
            _currentState = newGameState;
            _currentState.EnterState();
        }

        public void SignUp() => StartCoroutine(_signUpState.SignUpAsync());

        public void SignIn() => StartCoroutine(_signInState.SignInAsync());

        void Start() => StartCoroutine(InitializeFirebase());

        private IEnumerator InitializeFirebase()
        {
            var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
            yield return new WaitUntil(() => dependencyTask.IsCompleted);
            
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            if (dependencyTask.Exception != null)
                Debug.LogError(dependencyTask.Exception);

            CacheFirebaseInstances();
            
            AddListeners();
            if (_auth.CurrentUser != null)
                SwitchState(_signedMenuState);
            else
                SwitchState(_notSignedMenuState);
        }
        
        private void CacheFirebaseInstances()
        {
            _database = FirebaseDatabase.DefaultInstance;
            _auth = FirebaseAuth.DefaultInstance;
            _reference = _database.RootReference;
        }

        private void AddListeners()
        {
            loadButton.onClick.AddListener(() =>
            {
                Player.Instance.Data = _playerData;
                header.BackButton.onClick?.Invoke();
            });
            saveButton.onClick.AddListener(() =>
            {
                SavePlayerData(Player.Instance.Data);
            });
            deleteButton.onClick.AddListener(DeletePlayerData);
            goBackButton.onClick.AddListener(() => SwitchState(_notSignedMenuState));
            signUpButton.onClick.AddListener(() => SwitchState(_signUpState));
            signInButton.onClick.AddListener(() => SwitchState(_signInState));
            signOutButton.onClick.AddListener(SignOut);
        }

        private void SignOut()
        {
            _auth.SignOut();
            SwitchState(_notSignedMenuState);
        }

        private void UpdateSaveDescription(Task<bool> task)
        {
            UpdateSaveDescription(task.Result);
        }

        private void UpdateSaveDescription(bool result)
        {
            if (result)
            {
                saveDescriptionText.SetText(
                    $"Nickname: {_playerData.nickname}\n{_playerData.gold} \u2666 {_playerData.cash} $\n" +
                    $"Cars owned: {_playerData.ownedCars.Count}");
                SetButtonsInteractable(true);
            }
            else
            {
                saveDescriptionText.SetText("No Save Data found");
                SetButtonsInteractable(false);
            }
        }
        
        private void SetButtonsInteractable(bool isSaved)
        {
            loadButton.interactable = isSaved;
            saveButton.interactable = !isSaved;
            deleteButton.interactable = isSaved;
        }

        private void SavePlayerData(PlayerData player)
        {
            _reference.Child("users").Child(_auth.CurrentUser.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(player))
                .ContinueWithOnMainThread(
                    task =>
                    {
                        if (task.IsCompletedSuccessfully) LoadPlayerData();
                    });
        }

        private void DeletePlayerData()
        {
            _reference.Child("users").Child(_auth.CurrentUser.UserId).RemoveValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                    UpdateSaveDescription(false);
            });
        }
        
        public void LoadPlayerData()
        {
            LoadPlayerDataAsync().ContinueWithOnMainThread(UpdateSaveDescription);
        }

        public async Task<bool> LoadPlayerDataAsync()
        {
            if (!await SaveExistsAsync()) return false;
            _playerData = JsonUtility.FromJson<PlayerData>(_dataSnapshot.GetRawJsonValue());
            return true;
        }

        private async Task<bool> SaveExistsAsync()
        {
            _dataSnapshot = await _reference.Child("users").Child(_auth.CurrentUser.UserId).GetValueAsync();
            return _dataSnapshot.Exists;
        }
    } 
}