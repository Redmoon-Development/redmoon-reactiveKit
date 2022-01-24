using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedMoon.ReactiveKit
{
    public abstract class View<VM> : MonoBehaviour where VM : ViewModel<VM>
    {
        [SerializeField]
        protected UIDocument document;

        [SerializeField]
        private VM viewModel;

        CompositeDisposable disposable;

        protected VisualElement Root => document.rootVisualElement;

        private void OnEnable()
        {
            disposable = new CompositeDisposable();

            viewModel.ObserveInit.Subscribe(x =>
            {
                OnActivation(x, disposable);
            }

            ).AddTo(disposable);
        }

        public virtual void OnActivation(VM viewModel, CompositeDisposable disposable)
        {

        }

        private void OnDisable()
        {
            disposable.Dispose();
        }
    }
}
