using Cinemachine;
using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller
{
    public Transform StartPoint;
    public GameObject HeroPrefab;
    public Joystick joystick;//add joystick to the hero
    public CinemachineVirtualCamera VirtualCamera;//fill VirtualCamera.Follow

    public override void InstallBindings()
    {
        BindHeroDisplay();

        BindHero();

    }

    private void BindHeroDisplay()
    {
        Container
            .Bind<Joystick>()
            .FromInstance(joystick)
            .AsSingle();
    }

    private void BindHero()
    {
        HeroController heroController = Container
            .InstantiatePrefabForComponent<HeroController>(HeroPrefab, StartPoint.position, Quaternion.identity, null);

        HeroDisplaySetting(heroController);

        Container
            .Bind<HeroController>()
            .FromInstance(heroController)
            .AsSingle();
    }

    private void HeroDisplaySetting(HeroController heroController)
    {
        heroController.joystick = joystick;
        VirtualCamera.Follow = heroController.transform;
    }
}