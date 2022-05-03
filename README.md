# Red Moon Reactive Kit

## Description
A Lightweight UniRx Integration into Unity's UI Toolkit.

## How to Use (Install)
Follow the Install Instructions for https://github.com/sandolkakos/unity-package-manager-utilities

For Git Versioning and Updates, Install: https://github.com/mob-sakai/UpmGitExtension#usage

Then Just copy the link below and add it to your project via Unity Package Manager: [Installing from a Git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
```
https://github.com/crazyjackel/redmoon-reactiveKit.git
```

## How to Use (Work)

Red Moon Reactive Kit provides Binding and Subscription Extensions that work with Unity's UI Toolkit.

### Commands

#### BindCallback

BindCallback Binds Visual Element Callbacks to ReactiveCommands and Other Callbacks
```cs
Button button = Root.Q<Button>("MyButton");
button.BindCallback<ClickEvent>(command);
```
In the Example, The Button is bound to the command. When the Button is Clicked it will call the ReactiveCommand with a ClickEvent.
Use BindClick instead of BindCallback for your click commands.

#### BindValueChanged

BindValueChanged emits a new observable whenever an element's value changes.
```cs
TextField field = Root.Q<TextField>("MyField");
field.BindValueChanged<string>(command);
field.BindValueChanged<string>(property);
```
In the Example, The Textfield is bound such that whenever it text changes, it will call a ReactiveCommand and Update a ReactiveProperty.

#### BindToValueChanged

BindToValueChanged set the Element to change it's value whenever an Observable Emits a New Value. 
It does not notify that the value was changed.
```cs
TextField field = Root.Q<TextField>("MyField");
field.BindToValueChanged<string>(property);
```
In the Example, the ReactiveProperty is bound to the Field, such that whenever the ReactiveProperty Changes, the Textfield will update it's string.


#### AsObservable

AsObservable gets the Visual Element Events as Obsevable Emissions.
```cs
Button button = Root.Q<Button>("MyButton");
Observable<ClickEvent> clickStream = button.AsObservable<ClickEvent>(); 
clickStream.Subscribe((c) => Debug.Log(c));
```
In this Example, The Button now Emits ClickEvents as an Observable Stream, which is subscribed to log out the ClickEvent information.

### Views

A View is an Abstract Class that Wraps around a Document and Connects it to the ViewModel. Override OnActivation and you can use it to set up your UI.

```cs
public class MainView : View<MainViewModel>
{
  public override void OnActivation(MainViewModel viewModel, CompositeDisposable disposable)
  {
    Button button = Root.Q<Button>("ReactiveButton");
    button
        .BindClick(viewModel.OnButtonClick)
        .AddTo(disposable);
  }
}
```

### ViewModels

A ViewModel is an Abstract Class that is designed to act as your connector between the View and the functionality of the class. It is Loaded In first and allows you
to set up a bunch of commands and expose them to many other classes.

```cs
public class MainViewModel : ViewModel<MainViewModel>
{
  public ReactiveCommand<ClickEvent> OnButtonClick;
  public override void OnInitialization()
  {
    OnButtonClick = new ReactiveCommand<ClickEvent>();
    OnButtonClick.Subscribe((c) => Debug.Log("Button Clicked"));
  }
}
```


## Plans

- More Stylization UniRX commands.
- Optimizing calls
- More Examples

## License
MIT License

There is no legally binding modifications to the MIT License, but if you are using my stuff, I would appreciate doing one of the following: buy me a beer if you ever meet me at a bar or invite me to potential money-making operations via my Contact Information provided below.

## Contact Me
Contact me at jackel1020@gmail.com.
If you want your message to actually be read, add "[Github-Message]" to your subject line.
