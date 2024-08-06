## [1.0.0] - 2024-07-27
### First Release
- Pooled Audio Sources
- Simple Helpers for getting audio up quickly in: Animations, Monobehaviours, Builder
- Data first approach to audio configuration
- Global bootstrapper with selectable persistence types
- Menu integrations for key features

## [1.1.0] - 2024-07-28
### Feature Addition: Editor Setup Wizard
- Removed the persistent prefab, the better solution is to just use one manager prefab
- Added an editor window under Tools/Strangeman/Setup Audio Helper
- Editor window includes a directory pointer, a button to create starter assets for customization, and a button to get the example scene from the package

## [1.2.0] - 2024-08-02
### Feature Addition: Stop Audio on Scene Transition
- Added Scene event listener to audio manager, if a scene is loaded it will attempt to stop audio (depending on config settings)
- Added SceneField (new from Utils) list to config asset, that allows added scenes to play audio through load (this only works if the persistence is scene or bootstrapped)

## [1.2.1] - 2024-08-05
### Noop: Updated Package Information
- Updated organization name in package