using System;
using NUnit.Framework;
using System.Reflection;
using Handelabra.Sentinels.Engine.Model;


namespace SotmWorkshop
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void DoSetup()
        {
            // Tell the engine about our mod assembly so it can load up our code.
            // It doesn't matter which type as long as it comes from the mod's assembly.
            var a = Assembly.GetAssembly(typeof(Moonwolf.MoonwolfCharacterCardController)); // replace with your own type
            ModHelper.AddAssembly("SotmWorkshop", a); // replace with your own namespace
        }
    }
}
