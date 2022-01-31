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

        public ReactiveCommand<ClickEvent> OnButtonClick;
        public ReactiveCommand<ChangeEvent<string>> OnTextFieldChange;

        public override void OnInitialization()
        {
            OnButtonClick = new ReactiveCommand<ClickEvent>();
            OnButtonClick.Subscribe((c) => UpdateDebugText(c, "Button Clicked"));

            OnTextFieldChange = new ReactiveCommand<ChangeEvent<string>>();
            OnTextFieldChange.Subscribe((c) => UpdateDebugText(c, "Field Changed:"));
        }

        private void UpdateDebugText(ChangeEvent<string> obj, string Text)
        {
            DebugText.Value += $"{Text} {obj.previousValue} -> {obj.newValue}\n";
        }

        private void UpdateDebugText(ClickEvent obj, string Text)
        {
            Debug.Log(obj);
            DebugText.Value += $"{Text}\n";
        }
    }
}