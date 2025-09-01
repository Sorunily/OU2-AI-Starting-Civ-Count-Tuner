# OU2-AI-Starting-Civ-Count-Tuner
Let's you change the amount of civs starting out. Especially helpful for chaos games so the world isn't as empty.

## Requirements
- BepInEx 5 x64 (Mono). Download from the official BepInEx releases. https://github.com/bepinex/bepinex/releases

## Install
1. Extract the BepInEx zip into the game folder (next to the exe). Run the game once.
2. Extract **this mod’s zip** into the plugins location so that:
   - BepInEx/plugins/AIStartingCivCountTuner/AIStartingCivCountTuner.dll exists
3. Run the game. Check `BepInEx/LogOutput.log` for “Loading [Starting Civs Tweaks 1.0.0]”. After the first launch with the plugin installed you can find the created config in BepInEx/config/ou2.AIStartingCivCountTuner.cfg if you want to change the speed value. Vanilla value is 0 I believe. Default for the mod is 33.

## Configure
Edit `BepInEx/config/AIStartingCivCountTuner.cfg`:
