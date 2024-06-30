using AudioHelper.Audio;
using UnityEditor;
using UnityEngine;

namespace AudioHelper.Editor
{
    public class MenuItemAdditions
    {
        [MenuItem("GameObject/Audio/Audio Track", false)]
        static void CreateAudioTrack(MenuCommand menuCommand)
        {
            GameObject go = new GameObject(nameof(AudioTrack), typeof(AudioTrack));
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        [MenuItem("GameObject/Audio/Audio Track List", false)]
        static void CreateAudioTrackList(MenuCommand menuCommand)
        {
            GameObject go = new GameObject(nameof(AudioTrackList), typeof(AudioTrackList));
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}
