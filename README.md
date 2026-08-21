# Tainted Grail: The Fall of Avalon - Trainer

### by Rijiy

Telegram: [@Captain_S1ow](https://t.me/Captain_S1ow)

![FoA Trainer — BepInEx Mod, ESP, Item Spawner, Flight, EN/RU](assets/foa-trainer-banner.png)

English | [Русский](README_RU.md)

[![Latest Release](https://img.shields.io/github/v/release/Fenrisu1ven/FoA_Trainer?display_name=tag&sort=semver&label=Latest%20Release)](https://github.com/Fenrisu1ven/FoA_Trainer/releases/latest)
[![Download FoATrainer_V19_1.dll](https://img.shields.io/badge/Download-FoATrainer__V19__1.dll-2ea44f?logo=github)](https://github.com/Fenrisu1ven/FoA_Trainer/releases/download/v2.7.1/FoATrainer_V19_1.dll)
![Game: Tainted Grail](https://img.shields.io/badge/Game-Tainted%20Grail-9b7331)
[![BepInEx 5](https://img.shields.io/badge/BepInEx-5-5b6ee1)](https://github.com/BepInEx/BepInEx/releases)
![Platform: Windows](https://img.shields.io/badge/Platform-Windows-0078d4)
![Language: C#](https://img.shields.io/badge/Language-C%23-512bd4)
![UI: English / Russian](https://img.shields.io/badge/UI-English%20%2F%20Russian-d6a957)
![ESP: Configurable](https://img.shields.io/badge/ESP-Configurable-cf4f41)

**Current stable build: V19.1 / 2.7.1**

FoA Trainer is an in-game BepInEx trainer for **Tainted Grail: The Fall of Avalon**. It combines gameplay modifiers, character and inventory editing, profiles, Flight / NoClip, native time-of-day and weather control, an advanced searchable Item Spawner, and a configurable ESP overlay in one bilingual EN/RU interface.

[Features](#features) • [Configurable ESP](#configurable-esp) • [Installation](#installation) • [Controls](#controls) • [Item Spawner](#item-spawner) • [Profiles](#profiles) • [Screenshots](#screenshots) • [Building](#building-from-source) • [Diagnostics](#diagnostics) • [Changelog](#changelog) • [Disclaimer](#disclaimer)

> [!NOTE]
> V19.1 extends the confirmed-working V19 runtime with native time-of-day controls. It uses the game's `GameRealTime` API for clock changes and day progression without changing Unity `timeScale`. Compatibility with every game release is not guaranteed.

## Features

### Player / Combat

- God Mode / Ignore Hits
- Infinite Health, Mana, Stamina, and other character resources
- Stealth Mode and Easy Lockpicking
- One Hit Kills
- Damage, defense, mana-consumption, and stamina-consumption multipliers
- Movement speed and jump-height controls
- No Fall Damage
- Flight / NoClip mode

### Inventory

- Money and Wyrd/resource editors
- Zero item and equipment weight
- Ignore crafting requirements
- Potion, consumable, material, and selected-item quantity editors
- Selected item level editor

### Character / World

- XP and proficiency/mastery controls and multipliers
- Level, attribute-point, and skill-point editors
- Editors for primary attributes and proficiency/mastery values
- Live `HH:mm` game clock, exact input, slider, and quick-time buttons
- Native freeze and 0x-10x day/night progression controls
- Native weather preset selection with automatic/default and forced modes
- Live native precipitation, rain, snow, and heavy-rain status

### Native Weather

- Reads the real preset list from the game's `WeatherController`; no invented weather types
- Applies presets through the game's official `SetPreset(int)` method
- Immediately re-evaluates the selected native curve, including when the day/night clock is frozen or slowed down
- Forced mode safely re-applies the selected preset after location, load, or day changes
- Automatic/default mode stops forcing and returns scheduling to the game
- Keeps native intensity curves, variations, and transitions intact
- Stores weather mode and selection separately in BepInEx Config, leaving existing trainer profiles unchanged
- Does not expose an artificial transition-speed control because the game provides no supported API for one

### Native Time of Day

- Reads the current absolute clock from `GameRealTime.WeatherTime`
- Sets exact time through the official `SetWeatherTime(hour, minute)` path, preserving game time-change, day/night, quest, and spawner notifications
- Uses the game's own next-day rule: selecting an earlier time advances through midnight instead of moving the date backwards
- Includes a 00:00-23:59 slider, exact `HH:mm` input, and Dawn/Morning/Noon/Evening/Midnight buttons
- Resolves Dawn from the game's `ARDateTime.NightEndTime` value
- Freezes or scales only natural day/night progression through `SetWeatherDayDuration`; Unity `timeScale`, AI, and normal gameplay remain active
- Keeps time and weather overrides independent
- Uses immediate native clock changes; no artificial smooth transition is added because `GameRealTime` exposes no supported transition API

### Profiles / Interface

- Multiple named profiles with save, update, load, and delete actions
- Automatic reapplication of the last saved profile
- Profile persistence for enabled features, multipliers, editor values, flight settings, ESP settings, and interface language
- English and Russian UI, with English selected by default
- Resizable 1280×960 default window and fullscreen trainer mode
- Per-tab active-feature counters, global active count, and one-click **Disable All**
- Status and diagnostics page

## Configurable ESP

ESP remains a major V19 feature and continues to render when the trainer menu is hidden.

- `F6` master ESP toggle
- Separate item, searchable-container/corpse, enemy, and NPC groups
- Independent distance limits for items, containers, enemies, and NPCs
- Item-category filters for weapons, armor/shields, consumables, materials, important/key items, and other items
- Optional names and distance labels
- Lightweight letter/symbol icon badges
- Adjustable icon size and icons-only mode
- Independent HP text and HP-bar toggles
- Compact 3 px HP bars with adjustable width
- Optional display of dead NPCs and enemies
- `LOOT / EMPTY` state for searchable containers and corpses
- Option to hide empty containers and corpses
- Adjustable text size, dark label background, scan interval, and maximum on-screen object count
- ESP cache, raw-scan, visible-object, and camera diagnostic counters

### Performance and stability design

V19 keeps the performance-safe scanner from the stable V18.4 line:

- Targeted `World.All<T>()` scans replace the old full-world `Location` scan.
- `PickItemAction` is used for dropped/pickable items.
- `SearchAction` is used for searchable containers and corpses.
- `NpcElement` provides living/dead and enemy/NPC state.
- Game `ModelsSet<T>` collections are enumerated through `GetManagedEnumerator()` when required.
- Reflection access remains cached instead of repeatedly resolving the same fields and properties.
- Overlay drawing runs only during Unity `Repaint` events.
- Distance filtering is performed before the more expensive `SearchAction.IsEmpty()` check.
- The NPC state pass runs before corpse processing; dead NPCs/enemies are not rendered through the living-NPC branch.

## Installation

### Requirements

- Windows
- The Mono version/branch of Tainted Grail: The Fall of Avalon
- BepInEx 5

### Steps

1. Install [BepInEx 5](https://github.com/BepInEx/BepInEx/releases) for the Mono version of the game.
2. Start the game once so BepInEx creates its folders.
3. Download `FoATrainer_V19_1.dll` from the [latest GitHub Release](https://github.com/Fenrisu1ven/FoA_Trainer/releases/latest).
4. Copy the DLL to:

   ```text
   <Game Folder>/BepInEx/plugins/
   ```

5. Remove older `FoATrainer_*.dll` versions from the plugins folder.
6. Start the game.
7. Press `Insert` to open or hide the trainer.

`F8` is the fallback menu key. `F11` switches the trainer between fullscreen and window mode.

## Controls

| Key | Action |
| --- | --- |
| `Insert` | Show / hide trainer |
| `F8` | Alternative trainer toggle |
| `F6` | ESP master toggle |
| `F7` | Flight / NoClip |
| `F11` | Trainer fullscreen / window |
| `WASD` | Flight movement |
| `Space` | Fly up |
| `Ctrl` | Fly down |
| `Shift` | Flight boost |

Individual features also have their own hotkeys, displayed directly in the trainer interface.

## Item Spawner

The Item Spawner reads the game's available item templates and organizes them into layered filters. For example:

```text
Weapons → Bows → Short / Medium / Heavy
Weapons → Swords
```

Search is applied inside the selected category and subtype. The selected-item card can show the name, category/type, level, quantity, base/buy price, weight, calculated weapon damage and other stats, and character requirements where those fields are exposed by the current game build. Requirements are compared with the current character and displayed with green/red indicators.

## Profiles

Profiles are stored in:

```text
BepInEx/config/FoATrainer_Profiles/
```

A profile stores toggles, multipliers, editor values, flight settings, ESP settings, and interface language. Saving with an existing profile name updates it. The last saved profile is selected and reapplied automatically after the character is initialized on the next game start.

Weather and time-of-day settings are intentionally stored in the existing BepInEx configuration (`BepInEx/config/rijiy.foa.trainer.v19.cfg`) rather than in profiles. Existing profiles remain compatible and do not overwrite native weather/time control state.

## Screenshots

### Native Weather — V19

![FoA Trainer V19 native weather controls](screenshots/weather-ru-v19.png)

The V19 panel reads the game's real weather presets and native precipitation intensity in real time. The screenshot shows the forced `Short rain with variation` preset while the game's own heavy-rain state is active.

### Settings & Diagnostics — English

![FoA Trainer settings and diagnostics in English](screenshots/settings-en.png)

### Settings & Diagnostics — Russian

![FoA Trainer settings and diagnostics in Russian](screenshots/settings-ru.png)

The screenshots show the bilingual trainer interface. V19 adds the native weather block to the **XP / TIME** tab while movement speed, jump height, and fall-damage protection are grouped in **PLAYER**.

## Building from source

```bash
git clone https://github.com/Fenrisu1ven/FoA_Trainer.git
cd FoA_Trainer
python tools/build.py
```

Output:

```text
dist/FoATrainer_V19_1.dll
```

This project intentionally uses a bootstrap + runtime-compilation architecture instead of a conventional modern .NET SDK project. The generated BepInEx bootstrap embeds `FoATrainerRuntime.cs`; at game startup it compiles the runtime against the already loaded game/framework assemblies using the Mono.CSharp evaluator available in the game environment.

`tools/build.py` creates a new module GUID during each build, so functionally equivalent builds can have different hashes. The current V19.1 build is emitted alongside preserved V19 and V18.4 binaries in `dist/`.

### Source layout

- `src/FoATrainerRuntime.cs` — complete runtime, UI, ESP, and game logic
- `src/Bootstrap.reference.cs` — readable C# equivalent of the generated bootstrap
- `tools/build.py` — standalone bootstrap/DLL generator for V19.1
- `dist/FoATrainer_V19_1.dll` — current V19.1 binary
- `dist/FoATrainer_V19.dll` — preserved previous V19 binary
- `dist/FoATrainer_V18_4.dll` — preserved previous stable binary
- `assets/foa-trainer-banner.png` — FoA Trainer repository banner
- `screenshots/weather-ru-v19.png` — V19 native weather controls in game
- `screenshots/settings-en.png` and `screenshots/settings-ru.png` — bilingual settings/diagnostics screenshots

The repository does not include game assemblies, BepInEx binaries, Harmony binaries, or other proprietary game files.

## Diagnostics

Logs used for startup and compilation diagnostics:

- `BepInEx/FoATrainer_boot.log`
- `BepInEx/FoATrainer_compile.log`
- `BepInEx/LogOutput.log`

V19.1 retains the expected diagnostic result `Runtime started. Patches: 13, missing: 0`. Weather and time controls use native game controllers and require no additional Harmony patches.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history and [GitHub Releases](https://github.com/Fenrisu1ven/FoA_Trainer/releases) for downloadable builds.

## Author

**Rijiy**

Telegram: [@Captain_S1ow](https://t.me/Captain_S1ow)

## Disclaimer

This project is an unofficial fan-made modification and is not affiliated with, endorsed by, or supported by Awaken Realms or the developers/publishers of Tainted Grail: The Fall of Avalon.

Use at your own risk. Back up important save files before using gameplay modifications.

This is a single-player trainer/mod and is not intended for competitive or multiplayer use.
