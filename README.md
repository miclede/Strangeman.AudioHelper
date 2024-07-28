# Strangeman.AudioHelper
[![ChangelogBadge]](CHANGELOG.md) [![GitHub package.json version]][ReleasesLink] [![InstallationBadge]](#installation)


This package provides a simple data-first Audio Source & Sound management solution for Unity.

The package will be updated as I implement new features and fix issues.

Feel free to browse the online documentation (under development 2024.7.27) and create your own projects using this as a resource. While it is not required to state that a product / project is using this solution, I would like to shout-out those that do.

## Planned Features
- Split Screen Support: Essentially multi-listener functionality
- Simple Playables Setup: DI Sound Data for Unity Playables API
- Mixer UI Helper: Easy setup solution for Options menu to track and update Audio via Mixer & Sliders
- Audio Occlusion: Component & Collider based occlusion for drag and drop scene building.
- Mecanim Expansion: Further building out the Animation Helper for more use cases.
- More examples: Will create more examples as more features are finished.

## Dependency
https://github.com/miclede/Strangeman.Utils

## Installation
Option 1: Add to Unity from Package Manager:

Step 1.
```
https://github.com/miclede/Strangeman.Utils.git
```
Step 2.
```
https://github.com/miclede/Strangeman.AudioHelper.git
```

Option 2: from manifest.json:
```
"com.strangeman.utils": "https://github.com/miclede/Strangeman.Utils.git",
"com.strangeman.audiohelper": "https://github.com/miclede/Strangeman.AudioHelper.git"
```

Option 3: .unitpackage from releases:
```
https://github.com/miclede/Strangeman.AudioHelper/releases
```

Step 3: After Import (optional).
- Navigate to Tools/Strangeman/Setup Audio Helper
- Choose your project Directory
- Press Create Assets
- Press Get Example Scene


## Example
If you are having issues viewing the example scene through the package directory: use the setup wizard (Tools/Strangeman/Setup Audio Helper) "Get Example Scene."

<!------>
[ChangelogBadge]: https://img.shields.io/badge/Changelog-light
[GitHub package.json version]: https://img.shields.io/github/package-json/v/miclede/Strangeman.AudioHelper
[InstallationBadge]: https://img.shields.io/badge/Installation-red

[ReleasesLink]: https://github.com/miclede/Strangeman.AudioHelper/releases/latest