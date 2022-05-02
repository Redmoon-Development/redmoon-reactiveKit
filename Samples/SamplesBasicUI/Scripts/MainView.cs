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
            Slider slider = Root.Q<Slider>("ReactiveSlider");
            MinMaxSlider minMaxSlider = Root.Q<MinMaxSlider>("ReactiveMinMaxSlider");
            Toggle toggle = Root.Q<Toggle>("ReactiveToggle");

            Label fieldLabel = field.labelElement;

            //Bind such that whenever OnButtonClick can Execute, you can click button.
            //Bind such that whenever Button is clicked, it executes OnButtonClick.
            button
                .BindClick(viewModel.OnButtonClick)
                .AddTo(disposable);

            //Turn Event into Observable
            //Buffer based on Throttle Window
            //Check that there is more than 2 clicks
            //Select Last ClickEvent for Processing
            var clickStream = button.AsObservable<ClickEvent>();
            clickStream
                .Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250))) 
                .Where(xs => xs.Count >= 2)
                .Select(x => x.Last())
                .SubscribeToExecuteCommand(viewModel.OnDoubleClick)
                .AddTo(disposable);

            //Bind such that whenever DebugText changes, it updates label value.
            label.BindToValueChanged(viewModel.DebugText).AddTo(disposable);

            //Bind such that whenever field value changes, it executes OnTextFieldChange
            //Bind such that whenever field value changes, it updates reactive text. Initializes to default field value.
            field
                .BindValueChanged(viewModel.OnTextFieldChange)
                .AddTo(disposable);
            field
                .BindValueChanged(viewModel.ReactiveText)
                .AddTo(disposable);
            //Bind such that whenever ReactiveText changes, it updates fieldlabel value.
            fieldLabel
                .BindToValueChanged(viewModel.ReactiveText)
                .AddTo(disposable);

            //Observe the Slider Changing outside of Events
            //Buffer based on Throttle Window
            //Select Last
            var sliderStream = slider.ObserveEveryValueChanged(x => x.value);
            var fullStream = sliderStream
                .Buffer(sliderStream.Throttle(TimeSpan.FromMilliseconds(250)))
                .Select(x => x.Last());
            //Have Stream Update Max Value
            fullStream
                .SubscribeToUpdateProperty(viewModel.MaxValue)
                .AddTo(disposable);
            //Have Stream Execute Change Event
            fullStream
                .SubscribeToExecuteCommand(viewModel.OnSliderFieldChange)
                .AddTo(disposable);

            //Sometimes Binding to Certain Properties needs to be done manually.
            //In this case lowLimit and highLimit are properties that do not inform
            viewModel
                .MinMaxValue
                .Subscribe(x => UpdateMinMaxSlider(minMaxSlider, x))
                .AddTo(disposable);

            //Bind to Observe Values Changing
            //Then Execute Commands
            minMaxSlider
                .ObserveEveryValueChanged(x => x.value)
                .SubscribeToExecuteCommand(viewModel.OnMinMaxSliderFieldChange)
                .AddTo(disposable);

            toggle
                .BindValueChanged(viewModel.OnToggleChange)
                .AddTo(disposable);
        }

        private void UpdateMinMaxSlider(MinMaxSlider slider, Vector2 limit)
        {
            slider.lowLimit = limit.x;
            slider.highLimit = limit.y;
        }
    }
}
