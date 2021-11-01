using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleCivilization.Client.UI
{
    public abstract class WindowViewBase : MonoBehaviour
    {
        private readonly List<IWindowViewComponent> additionalComponents = new List<IWindowViewComponent>();
        private UniTask[] viewTasks = Array.Empty<UniTask>();

        public UIElementState State { get; protected set; }

        protected void Awake()
        {
            GetComponents(additionalComponents);
            viewTasks = new UniTask[additionalComponents.Count];
            DoAwake();
        }

        protected virtual void DoAwake() { }

        public virtual UniTask AppearAsync(bool animated = true)
        {
            Appearing(animated);
            return UniTask.WhenAll(viewTasks).ContinueWith(DidAppeared);
        }
        
        public virtual UniTask DisappearAsync(bool animated = true)
        {
            Disappearing(animated);
            return UniTask.WhenAll(viewTasks).ContinueWith(DidDisappeared);
        }

        private void Appearing(bool animated)
        {
            gameObject.SetActive(true);
            State = UIElementState.Appearing;

            for (var i = 0; i < viewTasks.Length; i++)
                viewTasks[i] = additionalComponents[i].AppearAsync(animated);
        }

        private void Disappearing(bool animated)
        {
            State = UIElementState.Disappearing;

            for (var i = 0; i < viewTasks.Length; i++)
                viewTasks[i] = additionalComponents[i].DisappearAsync(animated);
        }

        private void DidAppeared()
        {
            State = UIElementState.Appeared;

            for (var i = 0; i < viewTasks.Length; i++)
                additionalComponents[i].Appeared();
        }

        private void DidDisappeared()
        {
            gameObject.SetActive(false);
            State = UIElementState.Disappeared;

            for (var i = 0; i < viewTasks.Length; i++)
                additionalComponents[i].Disappeared();
        }
    }
}