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
            //Create a New Composite Disposable Object
            disposable = new CompositeDisposable();
            //When ViewModel becomes Available Activate the View.
            viewModel.ViewModelAvailable.Subscribe(__viewModel => OnActivation(__viewModel, disposable)).AddTo(disposable);
        }
        private void OnDisable()
        {
            disposable.Dispose();
        }
        private void OnDestroy()
        {
            disposable.Dispose();
        }
        public virtual void OnActivation(VM viewModel, CompositeDisposable disposable)
        {

        }
    }
}
