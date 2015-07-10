# Unity - NotNullAttribute

The NotNullAttribute is a custom attribute that is used to support workflows in Unity that rely heavily on object references. By applying the `[NotNull]` attribute to public Object fields on MonoBehaviours we can get alerted when a field is not properly wired up in the Editor. This allows us to assume the field is not null.

The package comes with a drawer that renders NotNull fields in the Inspector with errors when null, and with an "*" next to their label to help identify required fields. It also includes a tool to help find NotNull violations, which can be run at launch time or as part of a build.

##Example Workflow
Our workflow for UI requires each root UI screen, such a Main Menu, to be its own prefab. This root prefab controls all of its children, including assigning on click events and text, etc. We assign the children directly to the root prefab object as public members, of say a UIMainMenu.cs script.

Here is an example of the script without a NotNull attribute:

```
public class UI_MainMenu
{
  public Button ContinueButton;
  
  void Awake ()
  {
    if (ContinueButton == null) {
      Debug.LogError ("ContinueButton not assigned on 
    }
  
     ContinueButton.onClick.AddListener ( ClickContinueButton );
  }
  ...
}
```
Every object that we need to link on this Main Menu script will need to have a null error check and debug log. You can see that it would get tedious to type out this boiler plate. After a while, it would be tempting to leave off the null check and error log altogether.

Adding the NotNull gives us several advantages over the previous code:

* Less boiler plate
  * By flagging the field as NotNull, we can remove the null check and alert, since we can be confident it will be wired up at edit time. 
* Better error handling
  * We will catch the null reference at edit / build time, rather than having to instantiate the object to discover the bug. 
  * It will also produce better error output, since we won't have to write an error log for every member.

Here is what the MainMenu script looks like with NotNull:

```
public class UI_MainMenu
{
  [NotNull]
  public Button ContinueButton;
  
  void Awake ()
  {
     ContinueButton.onClick.AddListener ( ClickContinueButton );
  }
  ...
}
```

##Installation
The easiest thing to do is to install the package from the .unitypackage file that's included in the repository.
* In the root folder, click the file called "NotNullAttributePackage.unitypackage".
* Then click on View Raw to download the package.
* In Unity, go to Assets / Import Package / Custom Package
* Select the downloaded package
* Install all components

You can put the folder wherever you want, but the files under the Editor folder must remain in a folder called "Editor" in order to be compiled correctly.

##FAQ

* Why use this workflow instead of using `GameObject.Find ("ObjectName")` or `FindComponent<>` or some other way to link up objects.
  * There are many reasons we prefer this workflow. First it's not brittle. If you link to the object through GameObject.Find (), changing the name of the object in the hierarchy or in the scene will silently break this reference. It is also good for memory management. Assets that are referenced by an object will be loaded when the object is loaded, so it's easy to see what is being brought into memory when it's linked directly on a prefab or object. Finally, and maybe most importantly, if somehow the reference does break, you will find out about it before the build, not at runtime. 
