namespace Garage.FirebaseMenu
{
    public abstract class State
    {
        protected FirebaseContextPage FirebaseContextPage;

        protected State(FirebaseContextPage firebaseContextPage)
        {
            FirebaseContextPage = firebaseContextPage;
        }

        public abstract void EnterState();
        public abstract void LeaveState();
        
    }
}