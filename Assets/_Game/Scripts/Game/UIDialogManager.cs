using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class UIDialogManager : SingletonBehaviour<UIDialogManager>
    {
        private Queue<Tuple<UIDialog, DialogLine>> _dialogQueue;

        protected override void Awake()
        {
            base.Awake();

            _dialogQueue = new Queue<Tuple<UIDialog, DialogLine>>();
        }

        private void Update()
        {
            if (_dialogQueue.Count <= 0)
            {
                return;
            }

            Tuple<UIDialog, DialogLine> pair = _dialogQueue.Peek();
            if (!pair.Item1.HasPlayed)
            {
                pair.Item1.gameObject.SetActive(true);
                pair.Item1.Play(pair.Item2);
            }
            if (pair.Item1.IsComplete)
            {
                _dialogQueue.Dequeue();
                Destroy(pair.Item1.gameObject);
            }
        }

        public static void Queue(UIDialog dialog, DialogLine lineAsset)
        {
            if (!_instance)
            {
                return;
            }

            UIDialog dialogInstance = Instantiate(dialog, _instance.transform);
            dialogInstance.gameObject.SetActive(false);

            _instance._dialogQueue.Enqueue(new Tuple<UIDialog, DialogLine>(dialogInstance, lineAsset));
        }
    }
}