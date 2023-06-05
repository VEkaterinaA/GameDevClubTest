using Assets.Common.Infrastructure;
using Assets.Common.Scipts.Mutant;
using System.IO;
using UnityEngine;
using Zenject;

public class MutantFactory : IMutantFactory
{
    private Object ZombiePrefab;
    private DiContainer _container;

    private string ZombiePath = "Prefabs/Zombie";
    public MutantFactory(DiContainer container)
    {
        _container = container;
    }

    public void Load()
    {
        ZombiePrefab = Resources.Load(ZombiePath);
    }
    public void Create(MutantType mutantType)
    {
     _container.InstantiatePrefab(ZombiePrefab,new Vector2(20,20),Quaternion.identity,null);
    }
}
