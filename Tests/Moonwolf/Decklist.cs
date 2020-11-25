using NUnit.Framework;
using System;
using Handelabra.Sentinels.Engine.Model;
using Handelabra.Sentinels.Engine.Controller;
using System.Linq;
using System.Collections;
using Handelabra.Sentinels.UnitTest;


namespace SotmWorkshop.Moonwolf
{
    [TestFixture()]
    public class Decklist : Base
    {
        
        [Test()]
        public void Loads()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");

            Assert.AreEqual(6, this.GameController.TurnTakerControllers.Count());

            Assert.IsNotNull(moonwolf);
            Assert.IsInstanceOf(typeof(MoonwolfCharacterCardController), moonwolf.CharacterCardController);

            Assert.IsNotNull(env);
        }

        [Test()]
        public void Keywords()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            AssertCardsHaveKeyword("ongoing", "LunasAvatar", "FrenziedStrikes", "TearsOfTheMoon", "CycleOfLife", "NewMoonRising", "ChannelTheMoon", "MoonPriestess");

            AssertCardsHaveKeyword("limited", "LunasAvatar", "FrenziedStrikes", "TearsOfTheMoon", "CycleOfLife", "NewMoonRising", "ChannelTheMoon");

            AssertCardsHaveKeyword("feral", "LunasAvatar", "FrenziedStrikes", "TearsOfTheMoon", "CycleOfLife", "NewMoonRising", "ChannelTheMoon", "MoonPriestess");

            AssertCardsHaveKeyword("one-shot", "BlindRage", "ForcedChange", "TasteForBlood", "HowlAtTheMoon", "Rampage", "MoonlitInterlude", "DrawOutTheBeast", "BloodSacrifice");
        }





    }
}
