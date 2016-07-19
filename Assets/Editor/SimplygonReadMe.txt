Simplygon Unity Plugin 2.4.0
This is a short version of the user guide. For more extensive information please go to www.simplygon.com/unity. 


UNITY 5
If you have been using a previous version of the Simplygon plug-in in Unity 5, the shading networks of the 'Standard' and 'Standard (Specular setup)' will not be up to date. In order to update these shading networks to use the new default channels and nodes, click on the 'Edit Shading Network...' button in the 'Advanced Settings' tab and select the 'Standard' shader in the SN Setup window followed by clicking the 'Set Default' and 'Save' buttons. Repeat this procedure for the 'Standard (Specular setup)' shader. The procedure may be repeated for any shader for which you want to use the latest default channels and nodes. Please note that saving a shader's shading network will overwrite the previous setup.

If you want all shaders to use their respective default channels and nodes, close Unity and delete the files SimplygonShadingNetworkSetup.json and SimplygonShadingNetworkSetup.json.meta located in the project's Assets/Editor folder. Once Unity is restarted and the project reopened the default shading networks will be in use and the SimplygonShadingNetworkSetup.json file will be recreated if any shader's shading network is edited and saved.


HOW TO INSTALL IT
1. Start Unity.
2. Go to asset store and install the Simplygon Plugin.
3. In Unity, click Window -> Simplygon.
4. Log into Simplygon using your Simplygon account details. If you do not have a Simplygon account go to www.simplygon.com to register.


CREATING YOUR FIRST LOD
Start by configuring the LOD setting in the Create LOD tab in the Simplygon GUI. There are a lot of settings here that can be used to fine-tune the result you get from Simplygon. 

Now you need to select the asset you want to optimize. This can be done ether by selecting an asset inside of your scene or in the project explorer. Notice that the Target Assets section will show the name of the asset(s) you have selected.

Now, click the Simplygon button!

Your request is now being sent to and processed by the Simplygon servers. You can follow the progress of your LOD creation in the Manage Jobs tab of the Simplygon menu. 

You will find your LODs in your project explorer Assets/LODs/*AssetName_Date*.


OTHER FEATURES

SAVING SETTINGS FILES
You can save and load settings files for your processing’s. This feature is meant to speed up your workflow. When you have found a good setting for a LOD that you would like to apply to many assets. Save the setting so you won’t forget it in the future.

