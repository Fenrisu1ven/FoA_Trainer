# Changelog

All notable public changes to FoA Trainer are documented in this file.

## [2.7.1] - V19.1

- Added a dedicated native **Time of day** block to **XP / TIME**.
- Added a live `HH:mm` clock, 00:00-23:59 slider, exact time input, and Dawn/Morning/Noon/Evening/Midnight buttons.
- Read the absolute game clock from `GameRealTime.WeatherTime` and apply changes through the official `SetWeatherTime` method.
- Preserved the game's own next-day behavior, time-change events, day/night events, quest notifications, and location-spawner updates.
- Added a native Freeze Time mode and 0x-10x progression multiplier through `SetWeatherDayDuration`, without changing Unity `timeScale`.
- Resolved Dawn from the game's `ARDateTime.NightEndTime` value instead of inventing a sunrise constant.
- Stored time mode and multiplier in the existing V19 BepInEx Config while preventing old profiles from overwriting them.
- Kept time controls independent from automatic/forced weather modes.
- Re-evaluated the selected native weather curve immediately, so forced weather updates even while the day/night clock is frozen or slowed down.
- Updated bootstrap identity, documentation, and release artifact to `FoATrainer_V19_1.dll` / `2.7.1`.

## [2.7.0] - V19

- Added native weather control to the **XP / TIME** tab using the game's `Awaken.TG.Graphics.WeatherController`.
- Read the real in-game precipitation preset names at runtime instead of maintaining an invented weather list.
- Added automatic/default mode and a forced-preset mode that safely holds the selected preset across location, load, and day changes.
- Kept the game's native intensity curves, daily scheduling, variation, rain/snow values, and transitions active.
- Added live current-preset, precipitation, rain, snow, and heavy-rain status to the trainer UI.
- Stored the forced-weather state and selected preset in the new V19 BepInEx Config; existing trainer profiles and V18.4 configuration remain unchanged.
- Deliberately omitted a transition-speed control because the game exposes no supported native API for it.
- Moved movement speed, jump height, and no-fall-damage controls from **XP / TIME** to a dedicated **Movement and physics** section in **PLAYER**, preserving values, ranges, hotkeys, and profile compatibility.
- Updated plugin identity, bootstrap diagnostics, documentation, and release artifact to `FoATrainer_V19.dll` / `2.7.0`.
- Retained the V18.4 Mono.CSharp startup fix and all 13 existing Harmony patches.

## [2.6.4] - V18.4

- Added synchronized sliders alongside exact numeric inputs for all gameplay multipliers, rates, flight values, and ESP numeric settings.
- Fixed decimal editing so both `1.5` and `1,5`, intermediate input states, Backspace/Delete, and negative values where allowed work correctly.
- Reduced ESP minimum text size from 9 to 5, icon size from 12 to 6, and HP bar width from 24 to 8; added adjustable 1-8 px HP bar height.
- Replaced `EnemyBaseClass`-only ESP detection with centralized runtime classification based on `NpcElement.AntagonismToHero`.
- Added separate Friendly, Neutral, and Merchant ESP categories, toggles, markers, colors, counters, and backward-compatible profile loading.
- Detect merchants through the game's dedicated `Shop` model and give Merchant priority over combat-capable NPC type information.
- Fixed Mono.CSharp startup hangs by invoking the emitted runtime type directly from the evaluator module instead of compiling a second evaluator submission.
- Returned to a stability branch based directly on the confirmed-working V18.3 runtime.
- Added lightweight adjustable ESP icon badges and icons-only mode.
- Split HP text and HP bars into independent options.
- Added compact 3 px HP bars with adjustable width.
- Added `LOOT / EMPTY` state for searchable containers and corpses.
- Added an option to hide empty containers and corpses.
- Corrected corpse versus living NPC/enemy handling so dead actors are not rendered through the living-NPC branch.
- Retained performance-safe targeted `World.All<T>()` scanning for `PickItemAction`, `SearchAction`, and `NpcElement`.
- Retained `ModelsSet<T>.GetManagedEnumerator()` support, reflection caching, Repaint-only drawing, and distance rejection before expensive loot-state checks.
- Confirmed runtime startup with all 13 Harmony patches installed and 0 missing.

## [2.6.3] - V18.3

- Fixed the Mono.CSharp generic compile regression introduced in V18.2.
- Replaced problematic generic-set usage with the proven dictionary-based approach.
- Restored the working ESP/runtime baseline used by the V18.4 stability build.

## [2.6.2] - V18.2

- Fixed enumeration of the game's `ModelsSet<T>` collections through `GetManagedEnumerator()`.
- Added additional raw/cache ESP diagnostic counters.
- Superseded this development build after a duplicate generic-type compiler issue was found.

## [2.6.1] - V18.1

- Removed the full-world `Location` scan.
- Switched container/corpse discovery to targeted `SearchAction` scanning.
- Cached positions, HP values, and reflection accessors.
- Limited ESP overlay rendering to Unity `Repaint` events.
- Added gameplay-camera fallback.

## [2.6.0] - V18

- Added the first configurable ESP release line.
- Added the dedicated ESP tab and `F6` master toggle.
- Added item, container, enemy, and NPC groups.
- Added per-group distances, item-category filters, optional names/distance/HP, dead-actor display, and profile-persisted ESP settings.

## [2.5.1] - V17.1

- Fixed the clipped `TAINTED GRAIL - TRAINER` title.
- Centered the author and Telegram header.
- Added the English/Russian interface with English as the default language.
- Added automatic loading and reapplication of the last saved profile.
- Improved Item Spawner search and hierarchical filtering.
- Added item price, weight, weapon damage, stat, and requirement previews where available.
- Added requirement matching against the current character with red/green indication.
- Added the resizable 1280×960 window and fullscreen trainer mode.
- Added active-function counters for each tab and the global active count.

## [2.5.0] - V17

- Centered `by Rijiy | Telegram - @Captain_S1ow` in the top header row.
- Kept V16 startup-profile auto-load behavior.
- Prepared the source repository package.

## [2.4.0] - V16

- Fixed startup reapplication of saved enabled functions.
- Made the startup profile wait for a fully initialized Hero and retry when needed.
- Forced the header text onto a single line.
- Changed the creator label to `by Rijiy | Telegram - @Captain_S1ow`.

## [2.3.0] - V15

- Added English/Russian localization with English as the default language.
- Persisted the interface language in profiles.
- Selected the last saved profile at startup.
- Renamed the `Profiles` tab to `Settings`.
- Added dedicated visual markers to action-only rows.

## [2.2.0] - V14

- Added larger ON/OFF markers.
- Added active-function counts to tab names.
- Added the creator credit to the header.

## [2.1.0] - V13

- Added Item Spawner requirement comparison against current character stats.
- Changed the default window size to 1280×960.

## [2.0.0] - V12

- Added item stat requirements to the Item Spawner.
- Added the orange **Disable All** button.
- Improved fast window-resize capture.

## [1.9.0] - V11

- Added fullscreen trainer window mode and the `F11` shortcut.
- Initialized item damage preview through the game world before calculation.

## [1.8.0] - V10

- Added multi-level Item Spawner grouping and filtering.
- Improved weapon-stat calculation attempts.

## [1.7.0] - V9

- Added the dedicated Item Spawner tab.
- Added a resizable window.
- Redesigned the interface with an opaque dark theme.

## V8.x

- Added **Disable All**.
- Added saved profiles.
- Added Flight / NoClip.
- Added the Item Spawner and item-stat preview.

[2.7.1]: https://github.com/Fenrisu1ven/FoA_Trainer/releases/tag/v2.7.1
[2.7.0]: https://github.com/Fenrisu1ven/FoA_Trainer/releases/tag/v2.7.0
[2.6.4]: https://github.com/Fenrisu1ven/FoA_Trainer/releases/tag/v2.6.4
[2.5.1]: https://github.com/Fenrisu1ven/FoA_Trainer/releases/tag/v2.5.1
[2.5.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v2.4.0...v2.5.0
[2.4.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v2.3.0...v2.4.0
[2.3.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v2.2.0...v2.3.0
[2.2.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v2.1.0...v2.2.0
[2.1.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v2.0.0...v2.1.0
[2.0.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v1.9.0...v2.0.0
[1.9.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v1.8.0...v1.9.0
[1.8.0]: https://github.com/Fenrisu1ven/FoA_Trainer/compare/v1.7.0...v1.8.0
[1.7.0]: https://github.com/Fenrisu1ven/FoA_Trainer/releases/tag/v1.7.0
