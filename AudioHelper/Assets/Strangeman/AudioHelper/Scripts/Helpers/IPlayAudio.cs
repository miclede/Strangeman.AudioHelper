using System.Collections;

public interface IPlayAudio
{
    void Play();
    IEnumerator PlayCoroutine();
    void Stop();
    bool PlayOnStart { get; set; }
}