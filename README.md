# Strangeman.AudioHelper
[Changelog](CHANGELOG.md) 

This package provides a simple data-first Audio Source & Sound management solution for Unity.

The package will be updated as I implement new features and fix issues.

Feel free to browse the online documentation (under development 2024.7.27) and create your own projects using this as a resource. While it is not required to state that a product / project is using this solution, I would like to shout-out those that do.

## Planned Features
- Split Screen Support: Essentially multi-listener functionality
- Simple Playables Setup: DI Sound Data for Unity Playables API
- Mixer UI Helper: Easy setup solution for Options menu to track and update Audio via Mixer & Sliders
- Audio Occlusion: Component & Collider based occlusion for drag and drop scene building.
- Mecanim Expansion: Further building out the Animation Helper for more use cases.

## Dependency
https://github.com/miclede/Strangeman.Utils

## Installation
Add to Unity from Package Manager:

Step 1.
```
https://github.com/miclede/Strangeman.Utils.git
```
Step 2.
```
https://github.com/miclede/Strangeman.AudioHelper.git
```

Or from manifest.json:
```
"com.strangeman.utils": "https://github.com/miclede/Strangeman.Utils.git",
"com.strangeman.audiohelper": "https://github.com/miclede/Strangeman.AudioHelper.git"
```

## Example
To view the example scene, drag it out of the immutable folder within the packages folder structure, and place it into your main project directory.
This will create a copy of the scene, which you can open.