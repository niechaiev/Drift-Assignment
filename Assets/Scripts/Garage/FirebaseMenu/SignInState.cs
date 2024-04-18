using System.Collections;

namespace Garage.FirebaseMenu
{
    public class SignInState : SignState
    {
        private const string Title = "Sign In";
        public SignInState(FirebaseContextPage firebaseContextPage) : base(firebaseContextPage)
        {
        }

        public override void EnterState()
        {
            EnterState(Title, FirebaseContextPage.SignIn);
        }

        public override void LeaveState()
        {
            LeaveState(FirebaseContextPage.SignIn);
        }

        public IEnumerator SignInAsync()
        {
            var registerTask = FirebaseContextPage.Auth
                .SignInWithEmailAndPasswordAsync(
                    FirebaseContextPage.EmailField.text,
                    FirebaseContextPage.PasswordField.text);

            yield return SignAsync(registerTask);
        }
    }
}