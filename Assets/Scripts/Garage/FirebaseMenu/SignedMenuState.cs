namespace Garage.FirebaseMenu
{
    public class SignedMenuState : State
    {
        public SignedMenuState(FirebaseContextPage firebaseContextPage) : base(firebaseContextPage)
        {
        }

        public override void EnterState()
        {
            FirebaseContextPage.MainGameObject.SetActive(true);
            FirebaseContextPage.SignOutButton.gameObject.SetActive(true);
            UpdateWelcomeMessage();
            FirebaseContextPage.LoadPlayerData();
        }

        private void UpdateWelcomeMessage()
        {
            FirebaseContextPage.MainTitleText.SetText(
                $"You are signed as {FirebaseContextPage.Auth.CurrentUser.Email}");
        }

        public override void LeaveState()
        {
            FirebaseContextPage.MainGameObject.SetActive(false);
            FirebaseContextPage.SignOutButton.gameObject.SetActive(false);
            FirebaseContextPage.SaveButton.interactable = false;
            FirebaseContextPage.DeleteButton.interactable = false;
        }
    }
}