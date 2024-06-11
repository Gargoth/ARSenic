# ARSenic

## Prerequisites:
- [Unity 2022.3.24f1 LTS](https://unity.com/unity-hub) 
- [Vuforia SDK Unity Package 10.21.3](https://developer.vuforia.com/downloads/sdk) 

## Setup:

1. Download the Vuforia Unity Package online
2. Import the Vuforia Unity Package through `Assets > Import Package > Custom Package`
3. Navigate to `Assets > Editor > migrations` and copy the Vuforia `tar.gz` file to Packages
4. Delete `./Assets/Editor`

## Compilation

- Make sure the *Android Build Support* module is installed in the Unity Editor. This can be done through the Unity Hub.
- Make sure to set the target platform to `Android` in the Build Settings.
- To compile, go to the build settings and click `build`.

## Project Directory Structure:

The following shows the directory structure of the Assets folder, which contains the scripts and assets introduced by the developers.

```
 Assets
├──  Resources
├──  Scenes
└──  Scripts
   ├──  Common
   ├──  Energy
   ├──  Friction
   ├──  Gravity
   ├──  Inheritable
   └──  'Main Screens'
```

### Resources

The `Resources` directory contains the media assets used in the project. 
Included are the `prefab` objects (prebuilt gameobjects that can easily be instantiated), materials, physics materials, and imported external assets from the Unity Asset Store.

For proper organization of imported assets, create a subdirectory in the `Assets/Resources/Standard Assets/` directory with the name of the imported asset's source.

### Scenes

The `Scenes` directory contains used scenes in the project, which represent the different screens that the user will interact with.

### Scripts

The `Scripts` directory contains the user-defined scripts that power the functionality of the GameObjects and scene handlers.
It is further split into multiple subdirectories, each holding an assembly definition for the project.
Assembly definitions are used to speed up compilation times and ensure proper containerization.
When adding a new assembly definition, make sure to properly reference the existing assembly definitions used.
The assembly definitions corresponding to the subdirectories are as follows:

#### Inheritable

The `Inheritable` assembly definition contains scripts that house inheritable classes.
Currently, only the `Singleton` class is implemented in this assembly definition, but the intent is for other implemented design patterns to be included in this assembly definition.
The following file is included in this directory:

```
 Inheritable
├──  Inheritable.asmdef
└── 󰌛 Singleton.cs
```

#### Common

The `Common` assembly definition contains scripts that are used in multiple scenes, and mostly handle module-independent functionality.
The following files are included in this directory:

```
 Common
├── 󰌛 ARManager.cs
├──  Common.asmdef
├── 󰌛 EndCanvas.cs
├── 󰌛 GameManagerScript.cs
├── 󰌛 Highlighter.cs
├── 󰌛 PersistentDataContainer.cs
├── 󰌛 PopupCloseButton.cs
├── 󰌛 PopUpNextButton.cs
└── 󰌛 StopwatchScript.cs
```

#### Main Screens

The `Main Screens` assembly definition contains scripts that handle scenes that are not included in the modules.
The following files are included in this directory:

```
 Main Screens
├── 󰌛 HomeManagerScript.cs
├── 󰌛 InventoryManager.cs
├──  MainScreen.asmdef
└── 󰌛 SettingsSceneManagerScript.cs
```

#### Energy

The `Energy` assembly definition contains scripts that were used for the Energy module.
The following files are included in this directory:

```
 Energy
├──  EnergyModule.asmdef
├──  EnergyModule.asmdef.meta
├── 󰌛 EnergyModuleManager.cs
├──  EnergyModuleManager.cs.meta
├── 󰌛 EnergySource.cs
├──  EnergySource.cs.meta
├── 󰌛 EnergyTile.cs
└──  EnergyTile.cs.meta
```

#### Friction

The `Friction` assembly definition contains scripts that were used for the Friction module.
The following files are included in this directory:

```
 Friction
├── 󰌛 DelayedTrigger.cs
├── 󰌛 FailedLevel.cs
├──  FrictionModule.asmdef
├── 󰌛 FrictionModuleManager.cs
├── 󰌛 FrictionPlayerController.cs
├── 󰌛 FrictionStageManager.cs
└── 󰌛 FrictionTargetScript.cs
```

#### Gravity

The `Gravity` assembly definition contains scripts that were used for the Gravity module.
The following files are included in this directory:

```
 Gravity
├── 󰌛 AirResistance.cs
├── 󰌛 AirResistanceDefaults.cs
├──  GravityModule.asmdef
├── 󰌛 GravityModuleManager.cs
├── 󰌛 GravModObject.cs
└── 󰌛 SliderScript.cs
```

Note that the `AirResistance*.cs` files are **not** made by the developers of ARsenic, and are instead sourced from the following [itch.io link](https://thearchitect4855.itch.io/unity-air-resistance).
