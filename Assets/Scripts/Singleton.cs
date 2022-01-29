using UnityEngine;

public abstract class Singleton<TBehaviour> : MonoBehaviour
    where TBehaviour : MonoBehaviour
{
    static TBehaviour s_instance;

    public static TBehaviour Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<TBehaviour>();

                if (s_instance == null)
                {
                    GameObject instanceObject =
                        new GameObject($"{nameof(TBehaviour)} Singleton", typeof(TBehaviour));

                    DontDestroyOnLoad(instanceObject);
                }
            }

            return s_instance;
        }
    }

    protected virtual void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}