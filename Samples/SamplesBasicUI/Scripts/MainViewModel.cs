using RedMoon.ReactiveKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedMoon.ReactiveKit.Samples
{
    public class MainViewModel : ViewModel<MainViewModel>
    {
        public ReactiveProperty<string> DebugText { get; protected set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<string> ReactiveText { get; protected set; } = new ReactiveProperty<string>("");
        public ReactiveProperty<float> MaxValue { get; protected set; } = new ReactiveProperty<float>();
        public ReactiveProperty<float> MinValue { get; protected set; } = new ReactiveProperty<float>();
        public IReadOnlyReactiveProperty<Vector2> MinMaxValue { get; protected set; }

        public ReactiveCommand<ClickEvent> OnButtonClick;
        public ReactiveCommand<ClickEvent> OnDoubleClick;
        public ReactiveCommand<ChangeEvent<string>> OnTextFieldChange;
        public ReactiveCommand<float> OnSliderFieldChange;
        public ReactiveCommand<Vector2> OnMinMaxSliderFieldChange;
        public ReactiveCommand<ChangeEvent<bool>> OnToggleChange;

        public override void OnInitialization()
        {
            MinMaxValue = MinValue.CombineLatest(MaxValue, (x, y) => new Vector2(x, y)).ToReactiveProperty();

            //Note: it is best practice to try for one line lambdas or to de-anonymize functions
            //Unless of course, you want to make it difficult to inject code.
            OnButtonClick = new ReactiveCommand<ClickEvent>();
            OnButtonClick.Subscribe((c) => UpdateDebugText("Button Clicked"));

            OnDoubleClick = new ReactiveCommand<ClickEvent>();
            OnDoubleClick.Subscribe((c) => UpdateDebugText("Double Clicked"));

            OnTextFieldChange = new ReactiveCommand<ChangeEvent<string>>();
            OnTextFieldChange.Subscribe((c) => UpdateDebugText(c, "Field Changed:"));

            OnSliderFieldChange = new ReactiveCommand<float>();
            OnSliderFieldChange.Subscribe((c) => UpdateDebugText($"Slider Changed: {c}"));

            OnMinMaxSliderFieldChange = new ReactiveCommand<Vector2>();
            OnMinMaxSliderFieldChange.Subscribe((c) => UpdateDebugText($"MinMaxChanged: ({c.x},{c.y})"));

            OnToggleChange = new ReactiveCommand<ChangeEvent<bool>>();
            OnToggleChange.Subscribe((c) => UpdateDebugText($"Toggle {c.newValue}"));
        }


        private void UpdateDebugText<T>(ChangeEvent<T> obj, string Text)
        {
            DebugText.Value += $"{Text} {obj.previousValue} -> {obj.newValue}\n";
        }

        private void UpdateDebugText(string Text)
        {
            DebugText.Value += $"{Text}\n";
        }
    }
}