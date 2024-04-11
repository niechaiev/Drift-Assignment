using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Score
{
    public abstract class Score : MonoBehaviourPunCallbacks
    {
        protected TMP_Text Text;
        protected PlayerListItem PlayerListItem;
        protected Transform PlayerListTransform;
        protected List<PlayerListItem> PlayerListItems = new();
        protected int count;
        protected CarController CarController;
        private Coroutine _coroutine;

        public virtual void Init(TMP_Text text, PlayerListItem playerListItem, Transform playerListTransform)
        {
            this.Text = text;
            this.PlayerListItem = playerListItem;
            this.PlayerListTransform = playerListTransform;
        }

        public int Count => count;


        public void Setup(CarController carController)
        {
            this.CarController = carController;
            _coroutine = StartCoroutine(CountScore());
        }

        protected virtual IEnumerator CountScore()
        {
            throw new System.NotImplementedException();
        }


        public override void OnDisable()
        {
            base.OnDisable();
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
    }

}