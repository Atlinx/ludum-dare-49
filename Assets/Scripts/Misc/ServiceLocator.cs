using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IService { }

public class ServiceLocator : MonoBehaviour
{
    public static ServiceLocator Instance { get; set; }

    public List<IService> Services { get; set; } = new List<IService>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public T GetService<T>() where T : IService
    {
        return (T) Services.Find(x => x is T);
    }

    public void AddService<T>(T service) where T : IService
    {
        if (!Services.Any(x => x is T))
            Services.Add(service);
    }

    public void RemoveService<T>() where T : IService
    {
        Services.RemoveAll(x => x is T);
    }
}
