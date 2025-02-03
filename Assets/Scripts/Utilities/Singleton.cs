using UnityEngine;
// 描述：泛型单例。
// 创建者：Aze
// 创建时间：2025-01-02
public class Singleton<T> : MonoBehaviour where T : Singleton<T> 
{
    private static T instance;

    public static T Instance
    {
        get => instance;
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);    //保证只有一个单例存在
        }
        else
        {
            instance = (T)this;
        }
    }

    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
