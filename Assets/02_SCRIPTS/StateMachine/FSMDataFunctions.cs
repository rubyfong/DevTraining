using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMDataFunctions : MonoBehaviour
{
    [HideInInspector] public float DefaultMaxDistance;

    public void ActivateCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public GameObject SpawnObject(GameObject obj, Vector3 pos, Quaternion rot)
    {
        return Instantiate(obj, pos, rot);
    }

    public void DestroyGameObject(GameObject obj, float delay = 0)
    {
        Destroy(obj, delay);
    }

    public virtual void ResetData()
    {

    }

    public void ResetMaxAudioDistance(AudioSource source)
    {
        StartCoroutine(DelayResetMaxAudioDistance(source));
    }

    IEnumerator DelayResetMaxAudioDistance(AudioSource source)
    {
        yield return new WaitForSecondsRealtime(2);
        source.maxDistance = DefaultMaxDistance;
    }
}
