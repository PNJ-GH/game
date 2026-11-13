# LUNARA Character Creation — MPFB Setup

This guide prepares Blender for creating LUNARA's original human characters with free tools.

## Use the official MPFB extension

MPFB is a free, open-source human character generator/editor for Blender. It can create a humanoid mesh, adjust body proportions, apply skin and eye materials, and generate rigs.

- Official Blender extension: https://extensions.blender.org/add-ons/mpfb/
- Official getting-started guide: https://static.makehumancommunity.org/mpfb/docs/getting_started.html
- Source repository: https://github.com/makehumancommunity/mpfb2
- License: MPFB source code is GPL-3.0-or-later. Check the licence of each downloaded asset pack separately.

**We are not copying MPFB source into this game repository.** Install it as a Blender extension from the official Blender Extensions platform. This keeps the third-party tool separate from LUNARA's own game assets and code.

## Install in Blender

1. Open Blender 4.2 or newer.
2. Open Edit → Preferences → Get Extensions.
3. Search for **MPFB**.
4. Install the official MPFB extension.
5. Return to the 3D View and press **N** to open the sidebar.
6. Select the **MPFB** tab.
7. In MPFB, open the asset/library settings and load the official MakeHuman system assets pack from its ZIP file.
8. Restart Blender if the asset library does not appear.

## LUNARA Character 001 target

- Original adult female fantasy adventurer.
- Realistic human proportions; no anime styling.
- Long dark-brown hair.
- Silver-white and deep-navy clothing.
- Subtle moon motifs and restrained luminous accents.
- Neutral A-pose or T-pose for rigging.
- Export a working copy as FBX for Unity after mesh, materials, rig and scale are checked.

## Important production notes

- MPFB generates a starting human character; it does not automatically create LUNARA's final costume, hair design, premium face detail or finished game-ready optimization.
- Keep the editable Blender source file (.blend) in the LUNARA art-source folder.
- Keep exported Unity-ready files in the game's Art/Characters folder.
- Before shipping, review the licence for every add-on, asset pack, texture, hair asset and clothing asset used.
- Do not commit Blender cache files, temporary renders or large unused source packs to Git.
