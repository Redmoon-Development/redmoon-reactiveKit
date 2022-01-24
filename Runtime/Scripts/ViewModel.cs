using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx;
using UnityEngine;
using RedMoon.Injector;

namespace RedMoon.ReactiveKit
{
    public abstract class ViewModel<VM> : MonoBehaviour, IClient where VM : ViewModel<VM>
    {
        public ReactiveProperty<bool> Initialized { get; private set; } = new ReactiveProperty<bool>(false);
        public IObservable<VM> ObserveInit { get; private set; }

        public ViewModel()
        {
            ObserveInit = Initialized.Where(x => x).Select(x => (VM)this);
        }
        public virtual void OnEnable()
        {
            DepInjector.AddClient(this);
        }
        public virtual void OnDisable()
        {
            DepInjector.Remove(this);
        }
        public virtual void OnDestroy()
        {
            DepInjector.Remove(this);
        }
        public virtual void NewProviderAvailable(IProvider newProvider) { }
        public void NewProviderFullyInstalled(IProvider newProvider)
        {
            NewProviderInstalled(newProvider);
            TryInitialize(false);
        }
        public virtual void NewProviderInstalled(IProvider newProvider) { }
        public void ProviderRemoved(IProvider removeProvider) { }
        public virtual bool CanInitialize()
        {
            return true;
        }
        public void TryInitialize(bool requireIntiailization = false)
        {
            Initialized.Value = Initialized.Value && !requireIntiailization;
            if (!Initialized.Value && CanInitialize())
            {
                OnInitialization();
                Initialize();
            }
        }
        public void Initialize()
        {
            Initialized.Value = true;
        }
        public virtual void OnInitialization() { }
    }
}