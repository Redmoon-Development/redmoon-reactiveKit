using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public static class BindingExtensions
{
    #region Clickable
    /// <summary>
    /// Subscribes Action to Clickable until Disposable is Disposed
    /// </summary>
    /// <param name="clickable"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IDisposable BindClickable(this Clickable clickable, Action<EventBase> action)
    {
        var d1 = Observable
            .FromEvent<EventBase>(__action => clickable.clickedWithEventInfo += __action, __action => clickable.clickedWithEventInfo -= __action)
            .SubscribeWithState(action, (__eventBase, __action) => __action.Invoke(__eventBase));
        return d1;
    }
    /// <summary>
    /// Subscribes Action to Clickable until Disposable is Disposed
    /// </summary>
    /// <param name="clickable"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IDisposable BindClickable(this Clickable clickable, Action action)
    {
        var d1 = Observable
            .FromEvent(__action => clickable.clicked += __action, __action => clickable.clicked -= __action)
            .SubscribeWithState(action, (_, __action) => __action.Invoke());
        return d1;
    }
    /// <summary>
    /// Subscribes ReactiveCommand to Clickable until Disposable is Disposed
    /// </summary>
    /// <param name="clickable"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public static IDisposable BindClickable(this Clickable clickable, ReactiveCommand command)
    {
        return BindClickable(clickable, () => command.Execute());
    }
    /// <summary>
     /// Subscribes ReactiveCommand to Clickable until Disposable is Disposed
     /// </summary>
     /// <param name="clickable"></param>
     /// <param name="command"></param>
     /// <returns></returns>
    public static IDisposable BindClickable(this Clickable clickable, ReactiveCommand<EventBase> command)
    {
        return BindClickable(clickable, (ev) => command.Execute(ev));
    }
    #endregion

    #region Visual Element
    public static IDisposable BindVisualElementEnabled(this VisualElement element, IReadOnlyReactiveProperty<bool> property)
    {
        var d1 = property.SubscribeWithState(element, (__property, __element) =>
        {
            element.SetEnabled(__property);
        });
        return d1;
    }
    #endregion

    #region Button
    public static IDisposable BindButtonClick(this Button button, ReactiveCommand command)
    {
        var d1 = button.BindVisualElementEnabled(command.CanExecute);
        var d2 = button.clickable.BindClickable(command);
        return StableCompositeDisposable.Create(d1, d2);
    }
    #endregion
}
