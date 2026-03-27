# Lab 2 — FPS/TPS Controller (Unity)

Unity project with a player controller supporting FPS and TPS perspective switching.

## Running

1. Open the project in **Unity Hub**
2. Load the scene: `Assets/Scenes/GameScene`
3. Press **Play** in the Unity Editor

---

## Scripts

| Script | Description |
|--------|-------------|
| [PlayerController.cs](Assets/Scripts/PlayerController.cs) | Player movement, jumping, crouching, stamina system, player state machine |
| [CameraController.cs](Assets/Scripts/CameraController.cs) | FPS/TPS camera switching, smooth TPS follow with shoulder offset |
| [UIManager.cs](Assets/Scripts/UIManager.cs) | HUD — stamina bar, stamina text, current state display |

---

## Controls

| Key | Action |
|-----|--------|
| `W / A / S / D` | Move |
| `Shift` | Run (drains stamina) |
| `C` | Toggle crouch |
| `Space` | Jump |
| `V` | Toggle FPS / TPS |
| `Mouse` | Look / camera rotation |

---

## Systems

### Player States
- `Idle`, `Walk`, `Run`, `Crouch`, `Jump`, `Fall`
- Current state is displayed on the HUD in real time

### Stamina
- Running drains stamina at 20/s
- Stamina regenerates at 10/s when not running
- Running is automatically disabled when stamina drops below the threshold of 20

### Camera
- **FPS mode:** Camera sits at the player's head position; weapon visible
- **TPS mode:** Camera smoothly follows the player with a shoulder offset; weapon hidden

---

## Technologies

- **Engine:** Unity 6 (6000.3.11f1)
- **Language:** C#
