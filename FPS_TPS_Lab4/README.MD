# Lab 4 — Animation, Audio And Particle Effects (Unity)

Unity project with a FPS/TPS controller, animated SWAT character, Animation Event audio, and particle effects.

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
| [PlayerEffects.cs](Assets/Scripts/PlayerEffects.cs) | Footstep, jump, landing and weapon audio, landing dust, muzzle flash |
| [AnimationEventRelay.cs](Assets/Scripts/AnimationEventRelay.cs) | Receives Animation Events from the SWAT model and forwards them to player effects |
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
| `Left Mouse` | Fire weapon muzzle flash |
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

### Audio And Particles
- Footstep audio is triggered from Animation Events on walk/run/crouch clips
- Jump and landing sounds play during jump/landing
- Landing spawns a dust ParticleSystem
- Firing plays a short weapon sound and MuzzleFlash ParticleSystem

### Input
- Gameplay scripts use Unity's new Input System API
- Legacy `Input.GetAxis`, `Input.GetKey`, and `Input.GetButtonDown` calls were removed

---

## Scene

- Walls, floor, and ramp have colored materials applied
- Added a 3D character asset (visible in TPS mode)
- SWAT animations use Animation Events for footsteps, jump sound and landing dust
- Particle effects are present in the scene as `LandingDust` and `MuzzleFlash`

---

## Technologies

- **Engine:** Unity 6 (6000.4.0f1)
- **Language:** C#
