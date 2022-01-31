using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedMoon.ReactiveKit
{
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
        public static IDisposable BindClick(this VisualElement element, IReactiveCommand<ClickEvent> command)
        {
            var d1 = element.BindEnabled(command.CanExecute);
            var d2 = element.BindCallback(command);
            return StableCompositeDisposable.Create(d1, d2);
        }
        public static IDisposable BindClick<TArgs>(this VisualElement element, IReactiveCommand<(ClickEvent, TArgs)> command, TArgs dataForCallback)
        {
            var d1 = element.BindEnabled(command.CanExecute);
            var d2 = element.BindCallback(command, dataForCallback);
            return StableCompositeDisposable.Create(d1, d2);
        }
        #endregion

        #region Callback Event Handler
        public static IDisposable BindCallback<TEventType>(this CallbackEventHandler element, EventCallback<TEventType> callback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback(callback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        public static IDisposable BindCallback<TEventType, TUserArgsType>(this CallbackEventHandler element, EventCallback<TEventType, TUserArgsType> callback, TUserArgsType dataForCallback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            element.RegisterCallback(callback, dataForCallback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        public static IDisposable BindCallback<TEventType>(this CallbackEventHandler element, IReactiveCommand<TEventType> command, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            var callback = new EventCallback<TEventType>((ev) => command.Execute(ev));
            element.RegisterCallback(callback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        public static IDisposable BindCallback<TEventType, TUserArgsType>(this CallbackEventHandler element, IReactiveCommand<(TEventType, TUserArgsType)> command, TUserArgsType dataForCallback, TrickleDown trickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
        {
            var callback = new EventCallback<TEventType, TUserArgsType>((ev, args) => command.Execute((ev, args)));
            element.RegisterCallback(callback, dataForCallback, trickleDown);
            return Disposable.Create(() => { element.UnregisterCallback(callback, trickleDown); });
        }
        #endregion

        #region Notifications
        public static IDisposable BindValueChanged<T>(this INotifyValueChanged<T> element, IReactiveCommand<ChangeEvent<T>> command)
        {
            var callback = new EventCallback<ChangeEvent<T>>((ev) => command.Execute(ev));
            element.RegisterValueChangedCallback(callback);
            return Disposable.Create(() => { element.UnregisterValueChangedCallback(callback); });
        }
        public static IDisposable BindValueChanged<T>(this INotifyValueChanged<T> element, IReactiveProperty<T> property)
        {
            var callback = new EventCallback<ChangeEvent<T>>((ev) => property.Value = ev.newValue);
            element.RegisterValueChangedCallback(callback);
            return Disposable.Create(() => { element.UnregisterValueChangedCallback(callback); });
        }
        /// <summary>
        /// Binds Element changes to Update Property Values. 
        /// Then Initializes Values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element">Element with Value Changed Callbacks</param>
        /// <param name="property">Reactive Property that value should change on a Value Changed Callback</param>
        /// <param name="stateIsProperty">Whether Default Value should be Default Property Value</param>
        /// <returns></returns>
        public static IDisposable BindValueChangedWithState<T>(this INotifyValueChanged<T> element, IReactiveProperty<T> property, bool stateIsProperty = true)
        {
            var d1 = BindValueChanged(element, property);
            if (stateIsProperty)
            {
                element.value = property.Value;
            }
            else
            {
                property.Value = element.value;
            }
            return d1;
        }
        public static IDisposable BindToValueChanged<T>(this INotifyValueChanged<T> element, IReactiveProperty<T> property)
        {
            return property.SubscribeWithState(element, (__property, __element) =>
            {
                __element.SetValueWithoutNotify(__property);
            });
        }
        public static IDisposable BindToValueChangedWithState<T>(this INotifyValueChanged<T> element, IReactiveProperty<T> property, bool stateIsProperty = true)
        {
            var d1 = BindToValueChanged(element, property);
            if (stateIsProperty)
            {
                element.value = property.Value;
            }
            else
            {
                property.Value = element.value;
            }
            return d1;
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
