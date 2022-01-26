using RedMoon.ReactiveKit;
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
            OnButtonClick.Subscribe(ce =>
            {
                Debug.Log("Clicked");
                DebugText.Value += $"Button Clicked\n";
            });
        }
    }
}