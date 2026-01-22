using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected  static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
               SetupInstance();
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        RemoveDuplicates();
    }

    private static void SetupInstance()
    {
        instance = (T)FindAnyObjectByType(typeof(T));
        if(instance == null)
        {
            GameObject go = new GameObject();
            go.name = typeof(T).Name;
            instance = go.AddComponent<T>();
            DontDestroyOnLoad(go);
        }
    }

    private void RemoveDuplicates()
    {
        if(instance == null)
        {
            instance = this as T;
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
