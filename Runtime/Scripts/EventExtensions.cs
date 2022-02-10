using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Operators;
using UnityEngine;
using UnityEngine.UIElements;

namespace RedMoon.ReactiveKit
{
    public static class EventExtensions
    {
        public static IObservable<TEventType> AsObservable<TEventType>(this VisualElement element) where TEventType : EventBase<TEventType>, new()
        {
            return new EventCallbackObservable<TEventType>(element);
        }
    }

    internal class EventCallbackObservable<TEventType> : OperatorObservableBase<TEventType> where TEventType : EventBase<TEventType>, new()
    {
        readonly VisualElement element;
        public EventCallbackObservable(VisualElement element) : base(true)
        {
            this.element = element;
        }

        protected override IDisposable SubscribeCore(IObserver<TEventType> observer, IDisposable cancel)
        {
            return element.BindCallback<TEventType>(x => observer.OnNext(x));
        }
    }
}