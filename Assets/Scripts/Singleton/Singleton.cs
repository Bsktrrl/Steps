using UnityEngine;

// These managers need scene references and register themselves during Awake.
public interface ISceneSingleton { }

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static readonly bool sceneOwned = typeof(ISceneSingleton).IsAssignableFrom(typeof(T));
    private static T instance;
    public static T Instance
    {
        get
        {
            if (sceneOwned)
                return instance && instance is Behaviour behaviour && behaviour.isActiveAndEnabled ? instance : null;

            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    SetupInstance();
                }
            }

            return instance;
        }
    }


    //--------------------


    private void Awake()
    {
        RemoveDuplicates();
    }

    protected bool RegisterSceneInstance()
    {
        if (instance && instance != this && instance.gameObject.scene == gameObject.scene)
            return false;

        instance = this as T;
        return true;
    }

    protected void UnregisterSceneInstance()
    {
        if (ReferenceEquals(instance, this))
            instance = null;
    }


    //--------------------


    static void SetupInstance()
    {
        GameObject obj = new GameObject();
        obj.name = typeof(T).Name;
        instance = obj.AddComponent<T>();
        //DontDestroyOnLoad(obj);
    }

    void RemoveDuplicates()
    {
        if (instance == null)
        {
            instance = this as T;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
