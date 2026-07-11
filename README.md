# Build Sharing

A BepInEx mod for MycoPunk that lets you copy and paste gear upgrade grid layouts as compact shareable codes.

Originally extracted from [Enhanced Upgrade Menu](https://github.com/Little-Sparroh/EnhancedUpgradeMenu).

## Features

- **Copy Grid**: Export your currently equipped upgrade layout to a compact Base64 code on the clipboard
- **Paste Code**: Apply a shared build code to your gear details grid using upgrades from your inventory
- **Compact Encoding**: Binary-encoded build codes for short, easy-to-share strings

## Getting Started

### Dependencies

* MycoPunk (base game)
* [BepInEx](https://github.com/BepInEx/BepInEx) - Version 5.4.2403 or compatible
* .NET Framework 4.8
* [HarmonyLib](https://github.com/pardeike/Harmony) (included via NuGet)

### Building/Compiling

1. Clone this repository
2. Open the solution file in Visual Studio, Rider, or your preferred C# IDE
3. Build the project in Release mode to generate the .dll file

Alternatively, use dotnet CLI:
```bash
dotnet build --configuration Release
```

### Installing

**Via Thunderstore (Recommended)**:
1. Download and install via Thunderstore Mod Manager
2. The mod will be automatically installed to the correct directory

**Manual Installation**:
1. Place the built `BuildSharing.dll` in your `<MycoPunk Directory>/BepInEx/plugins/` folder

### Usage

1. Open the gear details window for a weapon or character
2. Click **Copy Grid** to copy the current upgrade layout to your clipboard
3. Share the code with others, or paste a code and click **Paste Code** to apply it

**Notes**:
- Paste requires matching upgrade instances in your inventory (or on the gear)
- Layouts are applied to the currently open gear details window
- Codes encode upgrade IDs, positions, and rotations

## Help

* **Mod not loading?** Verify BepInEx is installed correctly and check console logs for errors
* **Buttons missing?** Open a gear details window; buttons appear in the top-left of the screen
* **Paste not applying upgrades?** Ensure you own the required upgrades and the code is valid
* **Sharing not working?** Check clipboard permissions and ensure the build code is complete

## Authors

- Sparroh
- Generally Break (Efficient encoding)
- funlennysub (Original hex grid / sharing work in Enhanced Upgrade Menu)

## License

This project is licensed under the MIT License - see the LICENSE file for details
