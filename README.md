# Build Sharing

A BepInEx mod for Mycopunk that lets you copy and paste gear upgrade grid layouts as compact shareable codes.

Originally extracted from [Enhanced Upgrade Menu](https://github.com/Little-Sparroh/EnhancedUpgradeMenu).

## Features

- **Copy Grid**: Export your currently equipped upgrade layout to a compact Base64 code on the clipboard
- **Paste Code**: Apply a shared build code to your gear details grid using upgrades from your inventory
- **Compact Encoding**: Binary-encoded build codes for short, easy-to-share strings

## Getting Started

### Dependencies

- Mycopunk (base game)

- [BepInEx](https://github.com/BepInEx/BepInEx) - Version 5.4.2403 or compatible
- [SparrohUILib](https://thunderstore.io/c/mycopunk/p/Sparroh/SparrohUILib/) - Version 1.1.1 or newer
- HarmonyLib (included with BepInEx)

### Building/Compiling

1. Clone this repository
2. Open the solution file in Visual Studio, Rider, or your preferred C# IDE
3. Build the project in Release mode to generate the .dll file

Alternatively, use the dotnet CLI:

```bash
dotnet build --configuration Release
```

### Installing

**Via Thunderstore (Recommended)**:

1. Download and install via Thunderstore Mod Manager
2. The mod will be automatically installed to the correct directory

**Manual Installation**:

1. Install [SparrohUILib](https://thunderstore.io/c/mycopunk/p/Sparroh/SparrohUILib/) if it is not already installed
2. Place the built `BuildSharing.dll` in your `<Mycopunk Directory>/BepInEx/plugins/` folder

### Usage

1. Open the gear details window for a weapon or character
2. Use **Copy Grid** on the gear action bar to copy the current upgrade layout to your clipboard
3. Share the code with others, or paste a code and use **Paste Code** to apply it

**Notes**:

- Paste requires matching upgrade instances in your inventory (or on the gear)
- Layouts are applied to the currently open gear details window
- Codes encode upgrade IDs, positions, and rotations

## Help

- **Mod not loading?** Verify BepInEx and SparrohUILib are installed correctly and check console logs for errors
- **Buttons missing?** Open a gear details window; **Copy Grid** and **Paste Code** appear on the gear action bar
- **Paste not applying upgrades?** Ensure you own the required upgrades and the code is valid
- **Sharing not working?** Check clipboard permissions and ensure the build code is complete

## Authors

- Sparroh
- Generally Break (Efficient encoding)
- funlennysub (Original hex grid)

## License

This project is licensed under the MIT License - see the LICENSE file for details
