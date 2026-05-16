# Unity Starter Tools

A professional, generic suite of tools and managers to quickly bootstrap your Unity game prototypes and projects. Built with performance, modularity, and rapid development in mind.

## What's Included?

### 1. Audio System (Pro Edition)
A centralized audio management system that requires **no manual dragging of AudioClips**.
- **Auto-Import**: Simply drop your `.wav` or `.mp3` files into `Assets/Resources/Audio`. The `AudioAutoImporter` will detect them instantly and create the necessary data.
- **Editor Window**: Go to `Starter Tools > Audio Manager` in the Unity top menu to adjust Volume, Pitch, Loop, and assign Audio Mixers for all your sounds in one place.
- **Usage**:
  ```csharp
  AudioManager.Instance.PlaySFX("JumpSound");
  AudioManager.Instance.PlayMusic("BackgroundTheme");
  ```

### 2. Object Pooler
Recycle game objects (bullets, enemies, effects) to maintain high performance and avoid garbage collection spikes.
- **Spawn**:
  ```csharp
  GameObject bullet = ObjectPooler.Instance.Spawn(bulletPrefab, position, rotation);
  ```
- **Despawn**:
  ```csharp
  ObjectPooler.Instance.Despawn(bullet);
  ```

### 3. UI Panel Manager
A robust, stack-based UI manager to handle navigation without spaghetti code. Perfect for menus that require a functioning "Back" button.
- Add the `UIPanel` component to your Canvas elements and assign a `panelId`.
- **Open a Panel**:
  ```csharp
  PanelManager.Instance.OpenPanel("SettingsMenu");
  ```
- **Go Back**:
  ```csharp
  PanelManager.Instance.CloseCurrentPanel();
  ```

### 4. Input Reader (New Input System)
A centralized wrapper for Unity's New Input System. Stop dragging `UnityEvents` in the inspector!
- Ensure your `.inputactions` file has "Generate C# Class" checked and is named `InputSystem_Actions`.
- **Usage**:
  ```csharp
  private void OnEnable() {
      InputReader.OnJump += HandleJump;
      InputReader.OnMove += HandleMove;
  }
  private void OnDisable() {
      InputReader.OnJump -= HandleJump;
      InputReader.OnMove -= HandleMove;
  }
  ```

### 5. Save System
Save and load JSON data effortlessly to the device's persistent data path.
```csharp
PlayerData data = new PlayerData { score = 100 };
SaveSystem.SaveData("SaveSlot1", data);

PlayerData loaded = SaveSystem.LoadData<PlayerData>("SaveSlot1");
```

### 6. EventManager & SceneLoader
- **EventManager**: A strongly typed, global event bus to decouple your scripts.
  ```csharp
  EventManager.AddListener<EnemyKilledEvent>(OnEnemyKilled);
  EventManager.TriggerEvent(new EnemyKilledEvent { enemyId = 1 });
  ```
- **SceneLoader**: Asynchronously load scenes with a loading screen and progress bar.
  ```csharp
  SceneLoader.Instance.LoadScene("Level_01");
  ```

## Installation (Unity Package Manager)
The recommended way to install these tools in any project is via the Unity Package Manager (UPM):
1. Open your Unity Project.
2. Go to **Window > Package Manager**.
3. Click the `+` button in the top left corner and select **"Add package from git URL..."**.
4. Paste the following link:
   `https://github.com/gabrieloide/Unity-Starting-Tools.git`
5. Click **Add**. Unity will automatically download and install the tools.

## Requirements
- Unity 2021.3+ (Recommended)
- Unity Input System Package installed (will be automatically installed as a dependency).
