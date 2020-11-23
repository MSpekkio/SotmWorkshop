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
    public class CharacterCard : BaseTest
    {
     /*   
        [Test()]
        public void TestModWorks()
        {
            SetupGameController("Workshopping.TheBaddies", "Workshopping.MigrantCoder", "Workshopping.DevStream");

            Assert.AreEqual(3, this.GameController.TurnTakerControllers.Count());

            Assert.IsNotNull(baddies);
            Assert.IsInstanceOf(typeof(TheBaddiesTurnTakerController), baddies);
            Assert.IsInstanceOf(typeof(TheBaddiesCharacterCardController), baddies.CharacterCardController);

            Assert.IsNotNull(migrant);
            Assert.IsInstanceOf(typeof(MigrantCoderTurnTakerController), migrant);
            Assert.IsInstanceOf(typeof(MigrantCoderCharacterCardController), migrant.CharacterCardController);

            Assert.IsNotNull(env);

            Assert.AreEqual(40, baddies.CharacterCard.HitPoints);
            Assert.AreEqual(39, migrant.CharacterCard.HitPoints);
            QuickHPStorage(baddies, migrant);

            // Always deals 5 psychic!
            PlayTopCard(baddies);

            QuickHPCheck(-5, -6); // Nemesis!

            PlayTopCard(env);

            // Deals 1 damage
            QuickHPCheck(-1, -1);

            // Heals 1 at the start of the environment turn
            GoToStartOfTurn(env);
            QuickHPCheck(1, 1);
        }
     */

    }
}
