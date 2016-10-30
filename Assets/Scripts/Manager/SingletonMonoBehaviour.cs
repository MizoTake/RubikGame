using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> {
    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
#if UNITY_EDITOR
                if (instance == null) {
                    Debug.LogError(typeof(T) + "is nothing");
                }
#endif
            }
            return instance;
        }
    }

    protected void Awake() {
        CheckInstance();
    }

    protected bool CheckInstance() {
        if (instance == null) {
            instance = (T)this;
            return true;
        } else if (Instance == this) {
            return true;
        }

        Destroy(this.gameObject);
        return false;
    } 
}