using System.Collections;
using System.Threading.Tasks;
using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Garage.FirebaseMenu
{
    public abstract class SignState : State
    {
        protected SignState(FirebaseContextPage firebaseContextPage) : base(firebaseContextPage)
        {
        }

        protected void EnterState(string title, UnityAction signAction)
        {
            FirebaseContextPage.SignGameObject.gameObject.SetActive(true);
            FirebaseContextPage.SignText.SetText(title);
            FirebaseContextPage.SignButton.GetComponentInChildren<TMP_Text>().text = title;
            FirebaseContextPage.SignButton.onClick.AddListener(signAction);
            FirebaseContextPage.GoBackButton.gameObject.SetActive(true);
        }

        protected void LeaveState(UnityAction signAction)
        {
            FirebaseContextPage.SignGameObject.gameObject.SetActive(false);
            FirebaseContextPage.SignButton.onClick.RemoveListener(signAction);
            FirebaseContextPage.GoBackButton.gameObject.SetActive(false);
        }

        protected IEnumerator SignAsync(Task<AuthResult> registerTask)
        {
            yield return new WaitUntil(() => registerTask.IsCompleted);
            if (registerTask.Exception != null)
            {
                FirebaseContextPage.SignText.SetText(registerTask.Exception.Message);
                yield return null;
            }
            else
            {
                FirebaseContextPage.SwitchState(FirebaseContextPage.SignedMenuState);
            }
        }
    }
}