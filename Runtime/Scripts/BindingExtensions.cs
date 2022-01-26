using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedMoon.ReactiveKit
{
    public enum BindingMode : byte
    {
        OneWay,
        TwoWay,
        OneWayToSource
    }


    public static class BindingExtensions
    {
        #region Visual Element
        public static IDisposable BindEnabled(this VisualElement element, IReadOnlyReactiveProperty<bool> property)
        {
            return property.SubscribeWithState(element, (__property, __element) =>
            {
                element.SetEnabled(__property);
            });
        }
        public static IDisposable BindClick(this VisualElement element, ReactiveCommand<ClickEvent> command)
        {
            var d1 = element.BindEnabled(command.CanExecute);
            var d2 = element.BindVisualElementCallback(command);
            return StableCompositeDisposable.Create(d1, d2);
        }
        public static IDisposable BindClick<TArgs>(this VisualElement element, ReactiveCommand<(ClickEvent, TArgs)> command, TArgs dataForCallback)
        {
            var d1 = element.BindEnabled(command.CanExecute);
            var d2 = element.BindVisualElementCallback(command, dataForCallback);
            return StableCompositeDisposable.Create(d1, d2);
        }
        public static IDisposable BindVisualElementCallback<TEventType>(this VisualElement element, EventCallback<TEventType> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback(callback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        public static IDisposable BindVisualElementCallback<TEventType, TUserArgsType>(this VisualElement element, EventCallback<TEventType, TUserArgsType> callback, TUserArgsType dataForCallback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback(callback, dataForCallback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        public static IDisposable BindVisualElementCallback<TEventType>(this VisualElement element, ReactiveCommand<TEventType> command, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            var callback = new EventCallback<TEventType>((ev) => command.Execute(ev));
            element.RegisterCallback(callback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        public static IDisposable BindVisualElementCallback<TEventType, TUserArgsType>(this VisualElement element, ReactiveCommand<(TEventType, TUserArgsType)> command, TUserArgsType dataForCallback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            var callback = new EventCallback<TEventType, TUserArgsType>((ev, args) => command.Execute((ev, args)));
            element.RegisterCallback(callback, dataForCallback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        #endregion

        #region Notifications
        public static IDisposable BindValueChanged<T>(this INotifyValueChanged<T> element, IReactiveProperty<T> property)
        {
            var callback = new EventCallback<ChangeEvent<T>>((ev) => property.Value = ev.newValue);
            element.RegisterValueChangedCallback(callback);
            return Disposable.Create(() => { element.UnregisterValueChangedCallback(callback); });
        }
        public static IDisposable BindToValueChanged<T>(this INotifyValueChanged<T> element, IReactiveProperty<T> property)
        {
            return property.SubscribeWithState(element, (__property, __element) =>
            {
                __element.SetValueWithoutNotify(__property);
            });
        }
        public static IDisposable BindTwoWayValueChanged<T>(this INotifyValueChanged<T> element, IReactiveProperty<T> property)
        {
            var d1 = BindValueChanged(element, property);
            var d2 = BindToValueChanged(element, property);
            return StableCompositeDisposable.Create(d1, d2);
        }
        #endregion
    }
}
