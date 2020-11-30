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
        public void ChannelTheMoon()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            AddTokensToPool(pullofthemoon, 10);

            var card = PlayCard("ChannelTheMoon");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard);
            DecisionYesNo = true;
            DealDamage(baron, moonwolf, 2, DamageType.Cold);
            QuickHPCheck(0, 0, 0, 0);
            AssertTokenPoolCount(pullofthemoon, 8);

            QuickHPUpdate();
            DecisionYesNo = false;
            DealDamage(baron, moonwolf, 2, DamageType.Cold);
            QuickHPCheck(0, -2, 0, 0);
            AssertTokenPoolCount(pullofthemoon, 10);

            QuickHPUpdate();
            DecisionYesNo = true;
            DealDamage(baron, bunker, 2, DamageType.Cold);
            QuickHPCheck(0, 0, 0, 0);
            AssertTokenPoolCount(pullofthemoon, 8);

            QuickHPUpdate();
            DecisionYesNo = false;
            DealDamage(baron, bunker, 2, DamageType.Cold);
            QuickHPCheck(0, -2, 0, 0);
            AssertTokenPoolCount(pullofthemoon, 10);
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

        [Test]
        public void DrawOutOfTheBeast_Tokens(
            [Values(1, 3, 5)] int tokens
            )
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "MobileDefensePlatform");
            StartGame();

            var priest = PutOnDeck("MoonPriestess");

            SetNumberOfCardsInHand(moonwolf, 4);

            var mdp = GetMobileDefensePlatform().Card;

            AddTokensToPool(pullofthemoon, tokens);

            GoToEndOfTurn(baron);

            int handCount = GetNumberOfCardsInHand(moonwolf);
            int trashCount = GetNumberOfCardsInTrash(moonwolf);
            int playCount = GetNumberOfCardsInPlay(moonwolf);
            AssertTokenPoolCount(pullofthemoon, tokens);

            DecisionSelectNumber = tokens;
            DecisionSelectCard = priest;
            var card = PlayCard("DrawOutTheBeast");
            AssertInTrash(card);
            AssertInPlayArea(moonwolf, priest);

            AssertNumberOfCardsInHand(moonwolf, handCount);
            AssertNumberOfCardsInPlay(moonwolf, playCount + 1);
            AssertNumberOfCardsInTrash(moonwolf, trashCount + 1 + tokens - 1);
            AssertNumberOfCardsInRevealed(moonwolf, 0);
            AssertTokenPoolCount(pullofthemoon, 0);
        }

        [Test]
        public void ForcedChange_ChooseHand()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "MobileDefensePlatform");
            StartGame();

            var cycle = GetCard("CycleOfLife");
            DecisionSelectCard = cycle;
            DecisionMoveCardDestination = new MoveCardDestination(moonwolf.HeroTurnTaker.Hand);
            QuickShuffleStorage(moonwolf);
            var card = PlayCard("ForcedChange");
            AssertInTrash(card);
            AssertInHand(cycle);
            QuickShuffleCheck(1);
        }

        [Test]
        public void ForcedChange_ChoosePlay()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "MobileDefensePlatform");
            StartGame();

            var cycle = GetCard("CycleOfLife");
            DecisionSelectCard = cycle;
            DecisionMoveCardDestination = new MoveCardDestination(moonwolf.HeroTurnTaker.PlayArea);
            QuickShuffleStorage(moonwolf);
            var card = PlayCard("ForcedChange");
            AssertInTrash(card);
            AssertInPlayArea(moonwolf, cycle);
            QuickShuffleCheck(1);
        }

        [Test]
        public void FrenziedStrikes_SameTurn()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            GoToPlayCardPhase(moonwolf);

            var strikes = PutIntoPlay("FrenziedStrikes");

            GoToUsePowerPhase(moonwolf);
            PrintSeparator("Start");

            DecisionSelectTarget = mdp;
            QuickHPStorage(moonwolf.CharacterCard, mdp);
            SelectAndUsePower(moonwolf, out bool skipped);
            QuickHPCheck(-1, -2);

            QuickHPStorage(moonwolf.CharacterCard, mdp);
            SelectAndUsePower(moonwolf, out skipped);
            QuickHPCheck(-2, -2);

            GoToStartOfTurn(bunker);
            AssertNumberOfStatusEffectsInPlay(0);
        }

        [Test]
        public void FrenziedStrikes_PlayedOutOfTurn()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            var strikes = PutIntoPlay("FrenziedStrikes");
            AssertNumberOfStatusEffectsInPlay(0);

            GoToStartOfTurn(moonwolf);

            AssertNumberOfStatusEffectsInPlay(1);
            GoToPlayCardPhase(moonwolf);
            GoToUsePowerPhase(moonwolf);

            DecisionSelectTarget = mdp;
            QuickHPStorage(moonwolf.CharacterCard, mdp);
            SelectAndUsePower(moonwolf, out bool skipped);
            QuickHPCheck(-1, -2);

            QuickHPStorage(moonwolf.CharacterCard, mdp);
            SelectAndUsePower(moonwolf, out skipped);
            QuickHPCheck(-2, -2);

            GoToStartOfTurn(bunker);
            AssertNumberOfStatusEffectsInPlay(0);
        }

        [Test]
        public void HowlAtTheMoon_RemoveTokens()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);
            SetHitPoints(mdp, 8);

            AssertNumberOfStatusEffectsInPlay(0);
            AddTokensToPool(pullofthemoon, 10);

            GoToPlayCardPhase(moonwolf);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp);

            DecisionYesNo = true;
            var card = PutIntoPlay("HowlAtTheMoon");
            AssertInTrash(card);

            QuickHPCheck(0, 1, 1, 1, 0);

            AssertTokenPoolCount(pullofthemoon, 10 - 3);
        }

        [Test]
        public void HowlAtTheMoon_NoTokens()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);
            SetHitPoints(mdp, 8);

            AssertNumberOfStatusEffectsInPlay(0);
            AddTokensToPool(pullofthemoon, 10);

            GoToPlayCardPhase(moonwolf);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp);

            DecisionYesNo = false;
            var card = PutIntoPlay("HowlAtTheMoon");
            AssertInTrash(card);

            QuickHPCheck(0, 0, 0, 0, 0);

            AssertTokenPoolCount(pullofthemoon, 10);
        }

        [Test]
        public void HowlAtTheMoon_InsufficientTokens()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);
            SetHitPoints(mdp, 8);

            AssertNumberOfStatusEffectsInPlay(0);
            AddTokensToPool(pullofthemoon, 2);

            GoToPlayCardPhase(moonwolf);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp);

            DecisionYesNo = true;
            var card = PutIntoPlay("HowlAtTheMoon");
            AssertInTrash(card);

            QuickHPCheck(0, 0, 0, 0, 0);

            AssertTokenPoolCount(pullofthemoon, 0);
        }

        [Test]
        public void HowlAtTheMoon_IncreaseDamage()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;
            AssertNumberOfStatusEffectsInPlay(0);
            AddTokensToPool(pullofthemoon, 10);

            GoToPlayCardPhase(moonwolf);

            DecisionYesNo = true;
            var card = PutIntoPlay("HowlAtTheMoon");
            AssertInTrash(card);

            AssertNumberOfStatusEffectsInPlay(1);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp);
            DealDamage(bunker, mdp, 1, DamageType.Cold);
            QuickHPCheck(0, 0, 0, 0, -2);

            QuickHPUpdate();
            DealDamage(mdp, bunker, 1, DamageType.Cold);
            QuickHPCheck(0, 0, -1, 0, 0);

            GoToStartOfTurn(moonwolf);
            AssertNumberOfStatusEffectsInPlay(0);

            QuickHPUpdate();
            DealDamage(bunker, mdp, 1, DamageType.Cold);
            QuickHPCheck(0, 0, 0, 0, -1);

            QuickHPUpdate();
            DealDamage(mdp, bunker, 1, DamageType.Cold);
            QuickHPCheck(0, 0, -1, 0, 0);
        }

        [Test]
        public void LunasAvatar_DamageIncrease()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            var card = PlayCard("LunasAvatar");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, legacy.CharacterCard, bunker.CharacterCard, mdp);

            DecisionSelectTarget = mdp;
            UsePower(moonwolf.CharacterCard, 0);

            QuickHPCheck(0, -1, 0, 0, -4);
        }


        [Test]
        public void LunasAvatar_StartOfTurnInsufficientTokens()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            var card = PlayCard("LunasAvatar");
            AssertInPlayArea(moonwolf, card);

            GoToStartOfTurn(moonwolf);
            AssertInTrash(card);
        }

        [Test]
        public void LunasAvatar_StartOfTurnChooseNo()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;
            AddTokensToPool(pullofthemoon, 5);

            var card = PlayCard("LunasAvatar");
            AssertInPlayArea(moonwolf, card);

            DecisionYesNo = false;

            GoToStartOfTurn(moonwolf);
            AssertTokenPoolCount(pullofthemoon, 5);
            AssertInTrash(card);
        }

        [Test]
        public void LunasAvatar_StartOfTurnChooseYes()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;
            AddTokensToPool(pullofthemoon, 5);

            var card = PlayCard("LunasAvatar");
            AssertInPlayArea(moonwolf, card);

            DecisionYesNo = true;

            GoToStartOfTurn(moonwolf);
            AssertTokenPoolCount(pullofthemoon, 2);
            AssertInPlayArea(moonwolf, card);
        }

        [Test]
        public void MoonlitInterlude()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);

            var c1 = PutInHand("BringWhatYouNeed");

            DecisionSelectCards = new[] { scholar.CharacterCard, c1 };
            DecisionSelectTurnTaker = scholar.TurnTaker;

            QuickHPStorage(baron, moonwolf, legacy, bunker, scholar);
            var card = PlayCard("MoonlitInterlude");
            AssertInTrash(moonwolf, card);

            QuickHPCheck(0, 2, 0, 0, 2);
            AssertInPlayArea(scholar, c1);
        }

        [Test]
        public void MoonPriestess_Self()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);


            //DecisionSelectTurnTaker = scholar.TurnTaker;


            var card = PlayCard("MoonPriestess");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron, moonwolf, legacy, bunker, scholar);
            QuickHandStorage(moonwolf, legacy, bunker, scholar);
            DecisionSelectCards = new[] { moonwolf.CharacterCard };
            UsePower(card);

            QuickHPCheck(0, 2, 0, 0, 0);
            QuickHandCheck(1, 0, 0, 0);
        }

        [Test]
        public void MoonPriestess_Other()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);

            var card = PlayCard("MoonPriestess");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron, moonwolf, legacy, bunker, scholar);
            QuickHandStorage(moonwolf, legacy, bunker, scholar);
            DecisionSelectCards = new[] { bunker.CharacterCard };
            UsePower(card);

            QuickHPCheck(0, 0, 0, 2, 0);
            QuickHandCheck(0, 0, 1, 0);
        }

        [Test]
        public void NewMoonRising_Resurrection()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);

            AddTokensToPool(pullofthemoon, 11);

            var card = PlayCard("NewMoonRising");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron, moonwolf, legacy, bunker, scholar);

            DealDamage(baron, moonwolf, 99, DamageType.Cold);
            AssertInTrash(moonwolf, card);
            AssertTokenPoolCount(pullofthemoon, 0);
            AssertNotIncapacitatedOrOutOfGame(moonwolf);
            QuickHPCheck(0, -9, 0, 0, 0);
        }

        [Test]
        public void NewMoonRising_NoTokens()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(baron, 30);
            SetHitPoints(moonwolf, 20);
            SetHitPoints(bunker, 20);
            SetHitPoints(scholar, 20);

            var card = PlayCard("NewMoonRising");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron, moonwolf, legacy, bunker, scholar);

            DealDamage(baron, moonwolf, 99, DamageType.Cold);
            
            AssertTokenPoolCount(pullofthemoon, 0);
            AssertIncapacitated(moonwolf);
        }


        [Test]
        public void PullOfTheMoon_Tokens()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            AssertTokenPoolCount(pullofthemoon, 0);

            DealDamage(baron, moonwolf, 2, DamageType.Infernal);

            AssertTokenPoolCount(pullofthemoon, 2);

            DealDamage(moonwolf, moonwolf, 2, DamageType.Infernal);

            AssertTokenPoolCount(pullofthemoon, 4);
        }

        [Test]
        public void PullOfTheMoon_Indestrucible()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var card = GetCard("PullOfTheMoon");

            AssertInPlayArea(moonwolf, card);

            DestroyCard(card, baron.CharacterCard);

            AssertInPlayArea(moonwolf, card);
        }

        [Test]
        public void Rampage()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(moonwolf, 20);

            var o1 = PutIntoPlay("LivingForceField");
            var o2 = PutIntoPlay("PoliceBackup");

            DecisionSelectCards = new[] { o1, o2 };

            QuickHPStorage(baron, moonwolf, legacy, bunker, scholar);
            var card = PutIntoPlay("Rampage");
            AssertInTrash(moonwolf, card);
            AssertInTrash(o1);
            AssertInTrash(o2);

            QuickHPCheck(0, 1, 0, 0, 0);
        }

        [Test]
        public void TasteForBlood()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            SetHitPoints(moonwolf, 20);

            DecisionSelectTarget = mdp;
            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, legacy.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp);
            var card = PutIntoPlay("TasteForBlood");
            AssertInTrash(moonwolf, card);
            
            QuickHPCheck(0, 2, 0, 0, 0, -4);
        }

        [Test]
        public void TasteForBlood_NoDamageNoHeal()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Legacy", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            var mdp = GetMobileDefensePlatform().Card;

            SetHitPoints(moonwolf, 20);

            DecisionSelectTarget = baron.CharacterCard;
            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, legacy.CharacterCard, bunker.CharacterCard, scholar.CharacterCard, mdp);
            var card = PutIntoPlay("TasteForBlood");
            AssertInTrash(moonwolf, card);

            QuickHPCheck(0, 0, 0, 0, 0, 0);
        }


        [Test]
        public void TearsOfTheMoon_StartOfTurn()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(moonwolf, 20);

            GoToEndOfTurn(baron);

            var card = PutIntoPlay("TearsOfTheMoon");
            AssertInPlayArea(moonwolf, card);

            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard);

            GoToStartOfTurn(moonwolf);

            QuickHPCheck(0, 1, 0, 0);
        }

        [Test]
        public void TearsOfTheMoon_DealDamageResponse()
        {
            SetupGameController("BaronBlade", "SotmWorkshop.Moonwolf", "Bunker", "TheScholar", "Megalopolis");
            StartGame();

            SetHitPoints(moonwolf, 20);
            AddTokensToPool(pullofthemoon, 10);

            GoToEndOfTurn(baron);

            var card = PutIntoPlay("TearsOfTheMoon");
            AssertInPlayArea(moonwolf, card);

            DecisionYesNo = true;
            DecisionAmbiguousCard = GetCard("PullOfTheMoon");
            QuickHPStorage(baron.CharacterCard, moonwolf.CharacterCard, bunker.CharacterCard, scholar.CharacterCard);
            DealDamage(baron, moonwolf, 1, DamageType.Infernal, true);
            QuickHPCheck(0, 0, 0, 0);
            AssertTokenPoolCount(pullofthemoon, 9);

            QuickHPUpdate();
            DealDamage(baron, moonwolf, 0, DamageType.Infernal, true);
            QuickHPCheck(0, 0, 0, 0);
            AssertTokenPoolCount(pullofthemoon, 9);

            QuickHPUpdate();
            DealDamage(baron, moonwolf, 3, DamageType.Infernal, true);
            QuickHPCheck(0, -2, 0, 0);
            AssertTokenPoolCount(pullofthemoon, 10);
        }
    }
}