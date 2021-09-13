# Not Supported
This project is not currently supported by us. If you like it, I highly recommend buying [Odin Inspector](https://odininspector.com/). It has a [Required] attribute and validation suite that functions similarly (but better). I was hesitant to adopt Odin at first because I wanted to use Unity's native serializer, but you can use the Inspector without the serializer.

If you'd like to continue to use this attribute, feel free to do so. Just be aware we will no longer be making contributions so you'll have to support it yourself.

# Unity - NotNullAttribute

The NotNullAttribute is a custom attribute that is used to support workflows in Unity that rely heavily on GameObject and MonoBehavior references. By applying the `[NotNull]` attribute to public Object fields on MonoBehaviours we can get alerted when a field is not properly wired up in the Editor. This allows our code to assume the field is not null.

![Screenshot of NotNullAttribute](http://i.imgur.com/dTNh2pl.png)

The package comes with a drawer that renders NotNull fields in the Inspector with errors when null, and with an "*" next to their label to help identify required fields. It also includes a tool to help find NotNull violations, which can be run at launch time or as part of a build.

## Example Workflow
Our workflow for UI requires each root UI screen, such as Main Menu, to be its own prefab. This root prefab controls all of its children objects, including assigning OnClick events and text, etc. We assign the children directly to the root prefab object as public members, of say a UIMainMenu.cs script.

Here is an example of the script without a NotNull attribute:

```csharp
public class UI_MainMenu : MonoBehaviour
{
	public Button ContinueButton;
	
	void Awake ()
	{
		if (ContinueButton == null) {
			Debug.LogError ("ContinueButton not assigned on " + gameObject.name);
		}
		
		ContinueButton.onClick.AddListener ( ClickContinueButton );
	}

	void ClickContinueButton ()
	{
		Debug.Log ("Continue Button Clicked");
	}
}
```
To avoid undetected Null Pointer Errors, every object that we need to link on this Main Menu script will need to have a null error check and debug log like the one above. You can see that it would get tedious to type out this boiler plate. After a while, it would be tempting to leave off the null check and error log altogether.

Adding the NotNull gives us several advantages over the previous code:

* Less boiler plate
  * By flagging the field as NotNull, we can remove the null check and alert, since we can be confident it will be wired up at edit time. 
* Better error handling
  * Catches null references at edit / build time, rather than having to instantiate the object to discover the bug.
  * It will also produce better error output, since we won't have to write an error log for every member.
* Avoids referencing scene objects as string literals in code using GameObject.Find

Here is what the MainMenu script looks like with NotNull:

```csharp
public class UI_MainMenu : MonoBehaviour
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

## Installation
The easiest thing to do is to install the package from the .unitypackage file that's included in the repository.
* In the root folder of this Github project, click the file called "NotNullAttributePackage.unitypackage".
* Then click on View Raw to download the package.
* In Unity, go to Assets / Import Package / Custom Package
* Select the downloaded package
* Install all components

You can put the folder wherever you want, but the files under the Editor folder must remain in a folder called "Editor" in order to be compiled correctly.

## How to Use

### Scripting
After installation you can enforce error reporting on null public fields on MonoBehaviors by using the attribute `[NotNull]` or `[NotNullAttribute]`.

```csharp
  [NotNull]
  public GameObject RequiredObject;

  [NotNullAttribute] // This is the same as NotNull
  public GameObject Another RequiredObject;
```

Also provided is a flag called `IgnorePrefab` for ignoring the null check on prefabs. This is valuable when a prefab is used in multiple scenes and must link to an object that is stored in each of those scenes. The null check will run for only the object you use in your scene, not its corresponding prefab in the asset database.
```csharp
  [NotNull (IgnorePrefab = true)]
  public GameObject OnlyNeededInScene;
```

### Editor
You can manually run a test of all null references using the _Not Null Finder_ menu tool.

![RedBlueTools Menu](http://i.imgur.com/czgVruI.png)

Also note that a check for null references will be made at launch as well.

## Follow the Project
Follow our progress and issues on the [GitHub issues page](https://github.com/redbluegames/unity-notnullattribute/issues).

## FAQ

* Why use this workflow instead of using `GameObject.Find ("ObjectName")` or `FindComponent<>` or some other way to link up cached objects as [Unity suggests](http://docs.unity3d.com/ScriptReference/GameObject.Find.html)?
  * It's less brittle. If you link to the object through GameObject.Find (), changing the name of the object in the hierarchy or in the scene will silently break this reference. 
  * It is also good for memory management. Assets that are referenced by an object will be loaded when the object is loaded, so it's easy to see what is being brought into memory when it's linked directly on a prefab or object.
  * Finally, and maybe most importantly, if somehow the reference does break, you will find out about it before the build, not at runtime.
