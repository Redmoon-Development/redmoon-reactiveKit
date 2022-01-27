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

        public ReactiveCommand<ClickEvent> OnButtonClick;

        public override void OnInitialization()
        {
            OnButtonClick = new ReactiveCommand<ClickEvent>();
            OnButtonClick.Subscribe((c) => UpdateDebugText(c, "Button Clicked"));
        }

        private void UpdateDebugText(ClickEvent obj, string Text)
        {
            Debug.Log(obj);
            DebugText.Value += $"{Text}\n";
        }
    }
}