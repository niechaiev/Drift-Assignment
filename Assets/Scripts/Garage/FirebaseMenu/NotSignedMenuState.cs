namespace Garage.FirebaseMenu
{
    public class NotSignedMenuState : State
    {
        public NotSignedMenuState(FirebaseContextPage firebaseContextPage) : base(firebaseContextPage)
        {
        }

        public override void EnterState()
        {
            FirebaseContextPage.MainGameObject.SetActive(true);
            FirebaseContextPage.SignInButton.gameObject.SetActive(true);
            FirebaseContextPage.SignUpButton.gameObject.SetActive(true);
            UpdateWelcomeMessage();
        }

        private void UpdateWelcomeMessage()
        {
            FirebaseContextPage.MainTitleText.SetText("You are not signed in.");
            FirebaseContextPage.SaveDescriptionText.SetText("Sign In to access Save Data");
        }

        public override void LeaveState()
        {
            FirebaseContextPage.MainGameObject.SetActive(false);
            FirebaseContextPage.SignInButton.gameObject.SetActive(false);
            FirebaseContextPage.SignUpButton.gameObject.SetActive(false);
        }
    }
}