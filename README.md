# caius-studios
VR Game with Unity: adventure game based on University's project, continuation as a personal project in VR.

To-Do
---
Working on `Scenes/Village` at the moment

Build
* [x] Build `Scenes/Village` and sent to Oculus 2 device
* [ ] Reduce lag: this build currently lag too much/froze on Oculus 2 device: impossible to play!

GitHub Files
* [ ] Clean un-used files from previous non-VR game version (e.g. WebGL)
* [ ] `Assets/Oculus`: keep required/useful files only
* [ ] Remove un-used Assets from previous non-VR game version (e.g. `Assets/BlenderObjects/lowPolyCharacter1.fbx`)

Unity
* [ ] Remove gameobject `BasicPlayer` once the new VR controller is properly setup.
* [ ] Adapt scripts for gameobject `OVRPlayerController`
* [ ] fix Mechanics - Action: walk/run, sword handling
* [ ] fix Mechanics - Action: fighting


Unity > Hierarchy > Village 
---
Village
* `OVRPlayerController`: the new player, using Oculus package
  * sripts from package: OVRPlayerController, OVRSceneSampleController, OVRDebugInfo
  * scripts from previous BasicPlayer: PlayerManager, PlayerHealthDamageController, PlayerCombat
* `BasicPlayer`: represent the old / nonVR player
* UI / old canvas: PauseMenu, InventoryMenu, OptionsMenu, GameoverMenu, GameUI, DialoguesMenu
* All other GameObjects are for the environment/enemies

Other Assets/Scenes
---
* StartMenu
* Village
* Dungeon1
* VRBaseScene: was used for preliminary testing
