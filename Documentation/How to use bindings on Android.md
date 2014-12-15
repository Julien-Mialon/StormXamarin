# Binding in Android views

The goal of this is to allow you to use Mvvm with binding just like in Xaml but in Android views.

**_Binding is not supported on Adapter properties for now, but it's planned to be_**

### Setting up your project
First of all, you need to import both nuget packages ([Storm.Mvvm](https://www.nuget.org/packages/Storm.Mvvm/) and [Storm.Mvvm.Android](https://www.nuget.org/packages/Storm.Mvvm.Android/)) in your Android project.
This will create an ActivityDescription.json file on the root of your project, you need to change the build action of this file to BindingProcess. (Use right-click => properties on the file and changed the build action using the dropdown list.) In case the option is not available in the dropdown list, unload and reload your project.

### Initializing IoC and services
In order to initialize our IoC system and all of our services, we need to create the MyApplication.cs file (you could named it as you want) to hook on application start process. Copy/Paste the code below in it (more explanation in the code)
```c#
[Application]
//This must inherit ApplicationBase
public class MyApplication : ApplicationBase
{
    //Default constructor, mandatory otherwise your app won't start
	public MyApplication(IntPtr handle, JniHandleOwnership transfer)
		: base(handle,transfer)
	{
		
	}
    //Override the OnCreate to add our initialization logic
	public override void OnCreate()
	{
		base.OnCreate();
        // Create a default container which expose base services like Navigation, Logger, ...
		AndroidContainer.CreateInstance<AndroidContainer>(this, null, null);
	}
}
```

### Creating your ViewModel
Now, we create the ViewModel which will then be used by our View. This ViewModel could be placed in a PCL (Portable Class Library) project. We will not do this here for simplicity.
We will create a MainViewModel class which will handle the "Home" page of our application. In order to demonstrate how binding work, let's say we want this page to have : 
*An input text where the user can type his name
*A button to validate his input
*A Label to display "Hello" followed by the name he enters
To handle this process in our ViewModel, we need two string property (one for the input text and the other for the label text) and a command which will be bind to the click event of the button.
So our ViewModel will looks like this : 
```c#
public class MainViewModel : ViewModelBase
{
	private string _inputText;
	private string _labelText;

	// Property for the input text field 
	public string InputText
	{
		get { return _inputText; }
		set { SetProperty(ref _inputText, value); }
	}

	// Property for the text to display
	public string LabelText
	{
		get { return _labelText; }
		set { SetProperty(ref _labelText, value); }
	}

	// The command bound to the button click 
	public ICommand ButtonCommand { get; private set; }

	public MainViewModel(IContainer container) : base(container)
	{
		// Associate the command with function 
		ButtonCommand = new DelegateCommand(ButtonAction);
	}

	private void ButtonAction()
	{
		// Update text of label
		LabelText = string.Format("Hello {0} !", InputText);
	}
}
```

### Creating the Android view
Until now, everything should look the same for you as if you were doing Mvvm on Windows Phone or anything else like this. Now things will change a little bit.
With Xamarin, your views are supposed to be in Resources/layout/ and they are *.axml files. But here, since we want to create views which support bindings, we need to change this a little bit so, create a Views subfolders in your project and then add an xml file named Main.xml.
This file will contains the following code in order to handle the scenario we defined earlier.
```xml
<?xml version="1.0" encoding="utf-8"?>
<LinearLayout xmlns:android="http://schemas.android.com/apk/res/android"
			  android:orientation="vertical"
			  android:layout_width="fill_parent"
			  android:layout_height="fill_parent">
	
	<EditText android:layout_width="fill_parent"
			  android:layout_height="60dp"
			  text="{Binding InputText, Mode=TwoWay, UpdateEvent=TextChanged}"
			  />
	
	<Button android:layout_width="fill_parent"
			android:layout_height="wrap_content"
			android:text="OK"
			click="{Binding ButtonCommand}" />
	
	<TextView android:layout_width="fill_parent"
			  android:layout_height="wrap_content"
			  text="{Binding LabelText}"
			  />
</LinearLayout>
```
Here, most people familiar with Android views should see there are some differents. First, when you use binding in Android view, you don't need to specify the xml namespace before the attribute (you know, the android: prefix everywhere) otherwise, the attribute name stay the same. (To be more general, it must match a property or event name in the object but it's case insensitive).

So let's analyse a little bit the use of binding. For instance in the label TextView, we have `text="{Binding LabelText}"` this means the Text property of the TextView will take the value of the property LabelText in our context (here our ViewModel) and will be updated any time PropertyChanged on LabelText will be raised.

The binding on the button click `click="{Binding ButtonCommand}"` looks pretty much the same except that Click is an event and not a property.

But the Input text field on top of the file is a little bit harder, `text="{Binding InputText, Mode=TwoWay, UpdateEvent=TextChanged}"` this means that the Text property will be bound to the InputText property of the ViewModel. But with a Mode=TwoWay it allow update in the "normal" way (from ViewModel to View) but also in the other way (from View to ViewModel). In order to help him to know when to update in this other way, we need to specify the UpdateEvent with the name of the event which will be triggered when the property changed in the EditText. (here : TextChanged)

_For more information on how to use different types of bindings, check this page : [Binding details on Android](https://github.com/Julien-Mialon/StormXamarin/wiki/Binding-details-on-Android)_

### Creating your activity
Now that we have our View and our ViewModel, let's create our MainActivity. The code is available below : 
```c#
[Activity(Label = "TestApp.Android", MainLauncher = true, Icon = "@drawable/icon")]
public partial class MainActivity : ActivityBase
{
	protected override void OnCreate(Bundle bundle)
	{
		base.OnCreate(bundle);

		// Set our view from the "main" layout resource
		SetContentView(Resource.Layout.Main);
		// Set the ViewModel you want to associate with the view
		SetViewModel(new MainViewModel(AndroidContainer.GetInstance()));
	}
}
```
Here, you can obviously use things like ViewModelLocators to get your ViewModel.

### Nearly finished
Just one last step before getting your Mvvm pattern to work, in order to be preprocessed, the binding preprocessing system need to know which activity are associated with which views and where are they. This will be defined in the ActivityDescription.json file we talk in the beginning of this.
```javascript
{
    "list": [
        {
            "activity": {
                //The name of the output file where some code will be generated. This file is auto-included during compilation process
                "outputFile": "MainActivity.ui.cs",
                //The namespace where the Activity is
                "namespaceName": "Sample.Activities",
                //The name of our Activity
                "className": "MainActivity"
            },
            "view": {
                //The path to the view file relative to this json file
                "inputFile": "Views/Main.xml",
                //The name of the output file, this will impact the name of the layout resource in your package
                "outputFile": "Main.axml"
            }
        }
	]
}
```
And now, if everything went good, you can compile and test your app ;-)

### Summary
To summarize, when you want to use Binding, you need to :  
* Import nuget packages
* Change the BuildAction of the ActivityDescription.json file to BindingProcess
* Create your ViewModel
* Create your View
* Create your Activity
* Add the association between your View and your Activity in the ActivityDescription.json file
* Build and enjoy :-)

