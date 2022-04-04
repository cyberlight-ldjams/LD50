using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    public GameObject player;

    public override void InstallBindings()
    {
        Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle().NonLazy();
        if (!Container.HasBinding<PlayerMovement>() && player != null)
        {
            Container.Bind<PlayerMovement>().FromComponentsInNewPrefab(player).AsSingle().NonLazy();
        }

        if(!Container.HasBinding<PlayerMovement>()) {
            Debug.LogError("There is no player in the hierarchy and no player prefab has been set.");
        }

        Container.Bind<UI>().FromComponentInHierarchy().AsSingle();

    }
}