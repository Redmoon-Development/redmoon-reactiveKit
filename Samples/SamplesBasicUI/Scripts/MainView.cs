using RedMoon.ReactiveKit;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedMoon.ReactiveKit.Samples
{

    public class MainView : View<MainViewModel>
    {
        public override void OnActivation(MainViewModel viewModel, CompositeDisposable disposable)
        {
            Button button = Root.Q<Button>("ReactiveButton");
            Label label = Root.Q<Label>("Output");

            button.BindClick(viewModel.OnButtonClick).AddTo(disposable);
            
            label.BindTwoWayValueChanged(viewModel.DebugText).AddTo(disposable);
        }
    }
}
