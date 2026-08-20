# Tainted Grail: The Fall of Avalon - Trainer

### by Rijiy

Telegram: [@Captain_S1ow](https://t.me/Captain_S1ow)

English | [Русский](README_RU.md)

[![Latest Release](https://img.shields.io/github/v/release/Fenrisu1ven/FoA_Trainer?display_name=tag&sort=semver&label=Latest%20Release)](https://github.com/Fenrisu1ven/FoA_Trainer/releases/latest)
![Game: Tainted Grail](https://img.shields.io/badge/Game-Tainted%20Grail-9b7331)
[![BepInEx 5](https://img.shields.io/badge/BepInEx-5-5b6ee1)](https://github.com/BepInEx/BepInEx/releases)
![Platform: Windows](https://img.shields.io/badge/Platform-Windows-0078d4)
![Language: C#](https://img.shields.io/badge/Language-C%23-512bd4)
![UI: English / Russian](https://img.shields.io/badge/UI-English%20%2F%20Russian-d6a957)

FoA Trainer is an in-game BepInEx trainer for Tainted Grail: The Fall of Avalon. It provides gameplay modifiers, character and inventory editing, flight mode, profiles, an advanced searchable Item Spawner, and a bilingual EN/RU interface.

[Features](#features) • [Installation](#installation) • [Controls](#controls) • [Item Spawner](#item-spawner) • [Profiles](#profiles) • [Screenshots](#screenshots) • [Building](#building-from-source) • [Changelog](#changelog) • [Disclaimer](#disclaimer)

> [!NOTE]
> FoA Trainer is a single-player trainer/mod. Features depend on the game and Mono assembly versions; compatibility with every game release is not guaranteed.

## Features

### Player

- God Mode / Ignore Hits
- Infinite Health, Mana, and Stamina
- Infinite character resources
- Stealth Mode
- Easy Lockpicking
- One Hit Kills
- Damage and defense multipliers
- Mana and stamina consumption multipliers
- Movement speed and jump height controls
- No Fall Damage
- Flight / NoClip mode

### Inventory

- Money editor
- Wyrd / resource editor
- Zero item weight
- Zero equipment weight
- Ignore crafting requirements
- Potion, consumable, and material quantity editors
- Selected item quantity editor
- Selected item level editor

### Item Spawner

The Item Spawner is one of the trainer's main features:

- Searchable in-game item database
- Hierarchical category, type, and subtype filters
- Weapon → weapon type → subtype filtering
- Categories for armor, shields, consumables, materials, jewelry, gems, books, keys, tools, important items, and more
- Item level and quantity selection
- Price and weight preview where available
- Calculated weapon damage and stat preview where available
- Weapon attribute requirements
- Automatic comparison of requirements with the current character's stats
- Green/red requirement indication when a requirement is met or not met

See [Item Spawner](#item-spawner) for details.

### Character / Progression

- Infinite XP and XP multiplier controls
- Proficiency / mastery XP and multiplier controls
- Level editor
- Attribute point editor
- Skill point editor
- Editors for all primary attributes
- Proficiency / mastery editors

### World

- Game speed
- Movement speed
- Freeze daytime
- Daytime speed

### Profiles

- Multiple named profiles
- Save, update by saving the same name, load, and delete
- Enabled cheats stored in profiles
- Multipliers and editor settings stored in profiles
- Flight settings stored in profiles
- Interface language stored in profiles
- Automatic loading of the last saved profile when the game starts

### Interface

- English and Russian localization
- English selected by default
- Resizable trainer window
- 1280×960 default size
- Fullscreen trainer mode
- Active-feature counter for every tab
- One-click **Disable All** button
- Status and diagnostics page

## Installation

### Requirements

- Windows
- The Mono version/branch of Tainted Grail: The Fall of Avalon
- BepInEx 5

### Steps

1. Install [BepInEx 5](https://github.com/BepInEx/BepInEx/releases) for the Mono version of the game.
2. Start the game once so BepInEx creates its folders.
3. Download `FoATrainer_V17_1.dll` from the [latest GitHub Release](https://github.com/Fenrisu1ven/FoA_Trainer/releases/latest).
4. Copy the DLL to:

   ```text
   <Game Folder>/BepInEx/plugins/
   ```

5. Remove older FoATrainer DLL versions from the plugins folder.
6. Start the game.
7. Press `Insert` to open or hide the trainer.

`F8` is the fallback menu key. `F11` switches the trainer between fullscreen and window mode.

## Controls

| Key | Action |
| --- | --- |
| `Insert` | Show / hide trainer |
| `F8` | Alternative trainer toggle |
| `F11` | Trainer fullscreen / window |
| `F7` | Toggle Flight mode |
| `WASD` | Flight movement |
| `Space` | Fly up |
| `Ctrl` | Fly down |
| `Shift` | Flight boost |

Individual features also have their own hotkeys, displayed directly in the trainer interface.

## Item Spawner

The Item Spawner reads the game's available item templates and organizes them into layered filters. For example:

```text
Weapons → Bows → Short / Medium / Heavy
```

You can also select a direct weapon type such as:

```text
Weapons → Swords
```

Search is applied inside the currently selected category and subtype. This makes it possible to narrow a large database first, then search only the relevant results.

The selected-item card can show:

- Name
- Category and type
- Selected level and quantity
- Base/buy price
- Weight, where available
- Calculated weapon damage and other stats, where available
- Character attribute requirements
- Current-character comparison, with green/red requirement indicators

Some preview fields depend on data and calculation methods exposed by the current game build, so a field may be unavailable for a particular item.

## Profiles

Profiles are stored in:

```text
BepInEx/config/FoATrainer_Profiles/
```

A profile stores toggles, multipliers, editor values, flight settings, and interface language. Saving a profile with an existing name updates it. The last saved profile is selected and reapplied automatically after the character is initialized on the next game start.

## Screenshots

### Main Trainer

![Main Trainer](screenshots/trainer-main.jpg)

## Building from source

Clone the repository and run the included builder:

```bash
git clone https://github.com/Fenrisu1ven/FoA_Trainer.git
cd FoA_Trainer
python tools/build.py
```

The generated plugin is written to:

```text
dist/FoATrainer_V17_1.dll
```

This project intentionally uses a bootstrap + runtime compilation architecture instead of a conventional modern .NET SDK project. The BepInEx bootstrap loads the trainer runtime and compiles it against the game's Mono assemblies using the runtime compiler available in the game environment.

The repository does not include game assemblies, BepInEx binaries, or other proprietary game files.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history and [GitHub Releases](https://github.com/Fenrisu1ven/FoA_Trainer/releases) for downloadable builds.

## Author

**Rijiy**

Telegram: [@Captain_S1ow](https://t.me/Captain_S1ow)

## Disclaimer

This project is an unofficial fan-made modification and is not affiliated with, endorsed by, or supported by Awaken Realms or the developers/publishers of Tainted Grail: The Fall of Avalon.

Use at your own risk. Back up important save files before using gameplay modifications.

This is a single-player trainer/mod and is not intended for competitive or multiplayer use.
