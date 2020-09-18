using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T> : MonoBehaviour  where T : Singleton<T>
    {
    static public T Instance { get; private set; }

    protected void OnStart()
    {

    }

    protected void OnAwake()
    {
     
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
        if (Instance == (T)this)
            OnStart();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
