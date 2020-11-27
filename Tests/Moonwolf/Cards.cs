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
    public class Cards : Base
    {

        [Test]
        public void BlindRage_NoHeroTargets()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;
            var bb1 = PlayCard("BladeBattalion");
            var bb2 = PlayCard("BladeBattalion");

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp, bb1, bb2);
            DecisionSelectCards = new[] { mdp, bb1, bb2 };

            var card = PlayCard("BlindRage");
            AssertInTrash(card);
            QuickHPCheck(0, 0, 0, 0, -2, -2, -2);
        }

        [Test]
        public void BlindRage_OneHeroTargets()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;
            var bb1 = PlayCard("BladeBattalion");
            var bb2 = PlayCard("BladeBattalion");

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp, bb1, bb2);
            DecisionSelectCards = new[] { bunker.CharacterCard, bb1, bb2, bb1, bb2 };

            var card = PlayCard("BlindRage");
            AssertInTrash(card);
            QuickHPCheck(0, 0, -2, 0, 0, -4, -4);
        }

        [Test]
        public void BloodSacrifice()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var priest = PutInHand("MoonPriestess");

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard);
            DecisionYesNo = true;
            DecisionSelectCards = new[] { priest };

            var card = PlayCard("BloodSacrifice");
            AssertInTrash(card);
            AssertInPlayArea(moonwolf, priest);
            QuickHPCheck(0, -2, 0, 0);
        }

        [Test]
        [Ignore("TODO")]
        public void ChannelTheMoon()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();
        }

        [Test]
        public void CycleOfLife_StartOfTurn()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(moonwolf, 20);

            GoToEndOfTurn(baron);

            var card = PutIntoPlay("CycleOfLife");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard);

            GoToStartOfTurn(moonwolf);

            QuickHPCheck(0, 1, 0, 0);
        }

        [Test]
        public void CycleOfLife_EnvironmentTargets()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "MobileDefensePlatform");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            GoToEndOfTurn(baron);

            var card = PutIntoPlay("CycleOfLife");
            AssertInPlayArea(moonwolf, card);

            var target = PlayCard("ShieldGenerator");

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, target, mdp);
            DealDamage(moonwolf, target, 1, DamageType.Melee);
            QuickHPCheck(0, 0, 0, 0, -3, 0);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, target, mdp);
            DealDamage(moonwolf, mdp, 1, DamageType.Melee);
            QuickHPCheck(0, 0, 0, 0, 0, -1);
        }


        [Test]
        public void DrawOutOfTheBeast_NoTokens()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "MobileDefensePlatform");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            GoToEndOfTurn(baron);

            int handCount = GetNumberOfCardsInHand(moonwolf);
            int trashCount = GetNumberOfCardsInTrash(moonwolf);
            int playCount = GetNumberOfCardsInPlay(moonwolf);
            AssertTokenPoolCount(pullofthemoon, 0);

            var card = PlayCard("DrawOutTheBeast");
            AssertInTrash(card);

            AssertNumberOfCardsInHand(moonwolf, handCount);
            AssertNumberOfCardsInPlay(moonwolf, playCount);
            AssertNumberOfCardsInTrash(moonwolf, trashCount + 1);
            AssertTokenPoolCount(pullofthemoon, 0);
        }
    }
}
