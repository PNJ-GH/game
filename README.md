# LUNARA — Unity Game One

Clean Unity-first project. Active engine: Unity 6.6.0f1. Target: macOS Apple silicon (Mac mini M4, 16 GB unified memory).

## Create the first scene
1. Open Unity Hub and select Add > Add project from disk.
2. Select this repository folder after cloning it.
3. Wait for scripts to compile.
4. In the Unity menu choose LUNARA > Build First World Scene.
5. Open Assets/LUNARA/Scenes/LUNARA_WhiteNight.unity and press Play.

Controls: WASD move, mouse look, Space jump, Shift sprint, left click Moonblade attack, Esc toggle cursor.

## Game direction
- White Night fantasy survival world; one connected world.
- 64 match participants, 16 squads of 4.
- Offline-first: one human plus AI opponents. Online multiplayer is a later phase.
- Third-person primary camera; first-person option planned.
- Fantasy physical weapons, artifacts, Moon Energy, mounts, air-balloon squad deployment and Moon Circle.
- No modern rifles as the core identity.

## Current engineering status
This repository starts clean in Unity. The editor builder creates an initial gameplay scene with a terrain base, river, Moonheart landmark, simple forest placeholders, player controller and a damageable Moon Sentinel. It is a development foundation, not final art, not the complete 64-participant game, and not a release candidate. It has not been compiled or QA-tested on the target Mac yet.

Keep Unity-generated Library, Temp, Obj, Logs and Build folders out of Git.