# BuildIn — agent guide

Unity **2022.3.62f2**, 2D project (template `com.unity.template.2d`). Single-scene puzzle game ("The Opposite of Breakout!").

## Entry point

`Assets/Scenes/Game.unity` — the only scene in `EditorBuildSettings`.

`GameManager` (`Assets/Scripts/Managers/GameManager.cs`) is the root MonoBehaviour. It composes all other managers as plain C# objects in `Awake()` (no DI container, no serialization):

- `PlayingGridManager` — grid state + brick views
- `BrickQueueManager` — next-bricks FIFO queue
- `ProtoBrickManager` — top-row preview bricks
- `OverlayManager` — column selection overlays

## Architecture notes

- **Namespace layout**: `Managers`, `Configurations`, `Records`, `Extensions`, `Factories`. View MonoBehaviours are in the global namespace.
- **State/View split**: `BrickState` and `OverlayState` are C# `record` types. Views (`BrickView` abstract, `PlayingBrickView`, `ProtoBrickView`, `OverlayView`) read state but do not write it.
- **Factories**: `BrickFactory` / `OverlayFactory` implement interfaces (`IBrickFactory`, `IOverlayFactory`) for loose coupling.
- **PICO-8 palette**: Hardcoded in `GameManager._pico8Palette`. Active brick colors are Red/Yellow/Blue via `BrickColorPalette`.
- **Grid**: Configurable columns (0–13), fixed max 13 rows (`MaxGridSizeY`). `GridConfig.GetGridOffset()` centers bricks.

## Obsolete / dead code — do not modify

| File | Status |
|------|--------|
| `Boundary.cs` | `[Obsolete]` — ball handles its own OOB |
| `BrickHold.cs` | `[Obsolete]` — not part of game loop |
| `CanvasController.cs` | `[Obsolete]` — unknown purpose |

## Input / debug keys

| Key | Action |
|-----|--------|
| Enter | Start game (from start screen) |
| Arrow keys | Move overlay selection |
| Space | Place focused brick |
| R | Restart (while playing) |
| T | Shift grid rows down |
| Escape | Quit |

## What's missing

- **No tests** of any kind (edit mode or play mode)
- **No CI** (no `.github/workflows`)
- **No build scripts** outside Unity Editor — build via `File > Build Settings` or `-buildTarget` CLI
- **Several TODO comments** in `ProtoBrickManager.CheckTopBricks`, `ProtoBrickManager.MergeTopBrick`
