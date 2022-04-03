using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableInstaller", menuName = "Installers/ScriptableInstaller")]
public class ScriptableInstaller : ScriptableObjectInstaller<ScriptableInstaller>
{
    private PlayerControls controls = new PlayerControls();

    public override void InstallBindings()
    { 
        Container.Bind<PlayerControls>().AsSingle().NonLazy();
    }
}