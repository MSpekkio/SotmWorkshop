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
    public class CharacterCard : Base
    {
        [Test()]
        public void Innate()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, legacy.CharacterCard, bunker.CharacterCard, mdp);

            DecisionSelectTarget = mdp;
            UsePower(moonwolf.CharacterCard, 0);

            QuickHPCheck(0, -1, 0, 0, -2);
        }

        [Test()]
        public void Incap1()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            DealDamage(baron.CharacterCard, moonwolf, 99, DamageType.Psychic);

            var mdp = GetMobileDefensePlatform().Card;
            SetHitPoints(mdp, 5);

            QuickHPStorage(baron.CharacterCard, legacy.CharacterCard, bunker.CharacterCard, mdp);

            GoToUseIncapacitatedAbilityPhase(moonwolf);

            DecisionSelectCard = mdp;

            UseIncapacitatedAbility(moonwolf, 0);

            QuickHPCheck(0, 0, 0, 1);
        }

        [Test()]
        public void Incap2()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            DealDamage(baron.CharacterCard, moonwolf, 99, DamageType.Psychic);

            var mdp = GetMobileDefensePlatform().Card;
            var strike = PutInHand("BackFistStrike");
            var top = legacy.TurnTaker.Deck.TopCard;

            QuickHPStorage(baron.CharacterCard, legacy.CharacterCard, bunker.CharacterCard, mdp);
            QuickHandStorage(legacy, bunker, scholar);
            GoToUseIncapacitatedAbilityPhase(moonwolf);

            DecisionSelectCards = new Card[] { legacy.CharacterCard, strike, mdp };

            UseIncapacitatedAbility(moonwolf, 1);

            AssertInHand(top);
            AssertInTrash(strike);
            QuickHandCheck(0, 0, 0);
            QuickHPCheck(0, -2, 0, -4);
        }

        [Test()]
        public void Incap3()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            DealDamage(baron.CharacterCard, moonwolf, 99, DamageType.Psychic);

            var mdp = GetMobileDefensePlatform().Card;

            var card = PutIntoPlay("TrafficPileup");

            DecisionSelectCard = card;

            UseIncapacitatedAbility(moonwolf, 2);

            AssertInTrash(card);
        }
    }
}
