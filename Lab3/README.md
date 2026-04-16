# Lab 3 — Physics, Input & Collision (Unity)

Unity project demonstrating physics, player input, collision handling, trigger zones, coroutines, and vehicle control.

## Running

1. Open the project in Unity Hub
2. Load the scene: `Assets/Scenes/SampleScene`
3. Press Play in the Unity Editor

## Scripts

| Script | Description |
|---|---|
| `PlayerMovement.cs` | Player movement and jumping using new Input System (`Keyboard`, `InputActionMap`) |
| `CameraFollow.cs` | Smooth third-person camera that follows the active target (player or car) |
| `GameManager.cs` | Singleton — tracks score, displays status messages via TextMeshPro UI |
| `Coin.cs` | On collision with player: adds score, destroys itself |
| `Hazard.cs` | On collision with player: reduces speed by 60% for 2 seconds (Coroutine) |
| `BoostZone.cs` | Trigger zone — doubles speed while player or car is inside |
| `GravityZone.cs` | Trigger zone — reduces gravity to 20% (moon effect) while player is inside |
| `Gate.cs` | Press E near gate to open; gate rises gradually (Lerp Coroutine), auto-closes after 5s (WaitForSeconds) |
| `CarController.cs` | Rigidbody-based car — WASD controls, lateral damping, visual wheel spin |
| `VehicleEntry.cs` | Press E near car to enter/exit; switches camera target and enables car driving |

## Controls

| Key | Action |
|---|---|
| W / A / S / D | Move player / Drive car (when inside) |
| Space | Jump |
| E (near gate) | Open gate (closes automatically after 5s) |
| E (near car) | Enter / exit car |

## Systems

### Collisions
- **Coins** (gold spheres) — touch to collect, +10 score, object disappears
- **Hazards** (red pads) — touch to trigger 2s speed penalty (-60%), resets automatically

### Trigger Zones
- **Boost Zone** (green) — 2× speed while inside; also boosts car motor force
- **Gravity Zone** (purple) — low gravity (20%) while inside; affects both player and car

### Gate (Input + Coroutine)
- Approach the orange gate to see the interact prompt
- Press **E** to open — gate smoothly rises over ~1.3s (Lerp each frame)
- Gate automatically closes after **5 seconds** (WaitForSeconds + Lerp)

### Vehicle
- Blue car located on the left side of the scene
- Approach and press **E** to enter — player hides, camera switches to car
- Drive with **WASD**, press **E** again to exit
- Car passes through Boost Zone and Gravity Zone

## Scene

- Large ground plane with 4 static cube obstacles
- 5 collectible coins and 3 hazard pads scattered around
- 1 Boost Zone and 1 Gravity Zone
- Orange gate with side walls blocking passage until opened
- Blue car with visual wheels

## Technologies

- Engine: Unity 6 (6000.3.11f1)
- Language: C#
- Render Pipeline: URP
- Input: Unity New Input System (`UnityEngine.InputSystem`)
