using Assets.Common.Infrastructure;
using Assets.Common.Scipts;
using Assets.Common.Scipts.Hero;
using Assets.Common.Scipts.HeroInventory;
using Assets.Common.Scipts.Mutant.HelperClasses;
using Assets.Common.Scipts.Weapon;
using Cinemachine;
using UnityEngine;
using Zenject;

public class LocationInstaller : MonoInstaller, IInitializable
{
    public InventoryController inventory;

    public Transform StartPoint;
    public GameObject HeroPrefab;
    public Joystick joystick;//add joystick to the hero
    public CinemachineVirtualCamera VirtualCamera;//fill VirtualCamera.Follow
    public override void InstallBindings()
    {

        BindInstallerInterfaces();

        BindData();

        BindinventoryItemDataBase();

        BindHeroDisplay();
        
        BindHeroWeapon();

        BindHero();

        BindMutantFactory();

    }

    private void BindHeroWeapon()
    {
        Container
            .Bind<HeroWeapon>()
            .AsSingle();
    }

    private void BindData()
    {
        Container
            .Bind<FileOperations>()
            .AsSingle();
    }

    private void BindinventoryItemDataBase()
    {
        Container
            .Bind<InventoryItemDataBase>()
            .AsSingle();
    }

    private void BindInstallerInterfaces()
    {
        Container
            .BindInterfacesTo<LocationInstaller>()
            .FromInstance(this)
            .AsSingle();
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

        HeroInterfaceSetup(heroController);

        Container
            .Bind<HeroController>()
            .FromInstance(heroController)
            .AsSingle();

    }
    private void BindMutantFactory()
    {
        Container
            .Bind<IMutantFactory>()
            .To<MutantFactory>()
            .AsSingle();

        Container
            .Bind<MutantPositionGeneration>()
            .AsSingle();
;
    }
    private void HeroInterfaceSetup(HeroController heroController)
    {
        heroController.inventory = inventory;
        heroController.joystick = joystick;
        VirtualCamera.Follow = heroController.transform;
    }

    public void Initialize()
    {

        var MutantFactory = Container.Resolve<IMutantFactory>();

        for (int i = 0; i < 3; i++)
        {
            MutantFactory.Load();
            MutantFactory.Create(MutantType.Zombie);
        }
    }
}
