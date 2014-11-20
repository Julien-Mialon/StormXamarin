# StormXamarin
## MVVM library for Android with Xamarin

#### How to use binding in Android view files ?

+ compile the console tool Storm.Binding.Android.exe which precompile your Android view file and parse all binding instructions.
+ in your app, create a json file where you will describe your activities. (This json file could be anywhere, even outside the project) Make it looks like the one in TestApp.Android/Activities/ActivityDescription.json. Basically, you just need to describe all of your activity and associated view files. 
+ add to the prebuild event (in property->build events) of your app the call to Storm.Binding.Android.exe with the path to your json file as parameter.
+ make your activity extends ActivityBase in Storm.Mvvm.Android and set your view model with the SetViewModel function in the OnCreate method. (Be sure to insert the call to SetViewModel after the SetContentView)
Finally, run your app ;-)
