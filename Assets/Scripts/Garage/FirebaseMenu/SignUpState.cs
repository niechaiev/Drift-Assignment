using System.Collections;

namespace Garage.FirebaseMenu
{
    public class SignUpState : SignState
    {
        private const string Title = "Sign Up";
        public SignUpState(FirebaseContextPage firebaseContextPage) : base(firebaseContextPage)
        {
        }

        public override void EnterState()
        {
            EnterState(Title, FirebaseContextPage.SignUp);
        }
        
        public override void LeaveState()
        {
            LeaveState(FirebaseContextPage.SignUp);
        }


        public IEnumerator SignUpAsync()
        {
            var registerTask = FirebaseContextPage.Auth
                .CreateUserWithEmailAndPasswordAsync(
                    FirebaseContextPage.EmailField.text,
                    FirebaseContextPage.PasswordField.text);

            yield return SignAsync(registerTask);
        }
    }
}