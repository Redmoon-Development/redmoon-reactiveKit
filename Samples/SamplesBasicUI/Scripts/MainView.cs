using RedMoon.ReactiveKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

namespace RedMoon.ReactiveKit.Samples
{

    public class MainView : View<MainViewModel>
    {
        public override void OnActivation(MainViewModel viewModel, CompositeDisposable disposable)
        {
            //Grab Button
            Button button = Root.Q<Button>("ReactiveButton");
            Label label = Root.Q<Label>("Output");
            TextField field = Root.Q<TextField>("ReactiveTextField");
            Label fieldLabel = field.labelElement;

            //Bind such that whenever OnButtonClick can Execute, you can click button.
            //Bind such that whenever Button is clicked, it executes OnButtonClick.
            button.BindClick(viewModel.OnButtonClick).AddTo(disposable);

            //Turn Event into Observable
            //Buffer based on Throttle Window
            //Check that there is more than 2 clicks
            //Double Click Detected
            var clickStream = button.AsObservable<ClickEvent>();
            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250))).Where(xs => xs.Count >= 2).Subscribe(x =>
            {
                viewModel.OnDoubleClick.Execute(x.Last());
            });

            //Bind such that whenever DebugText changes, it updates label value.
            label.BindToValueChanged(viewModel.DebugText).AddTo(disposable);

            //Bind such that whenever field value changes, it executes OnTextFieldChange
            //Bind such that whenever field value changes, it updates reactive text. Initializes to default field value.
            field.BindValueChanged(viewModel.OnTextFieldChange).AddTo(disposable);
            field.BindValueChangedWithState(viewModel.ReactiveText, false).AddTo(disposable);
            //Bind such that whenever ReactiveText changes, it updates fieldlabel value.
            fieldLabel.BindToValueChanged(viewModel.ReactiveText).AddTo(disposable);
        }
    }
}
