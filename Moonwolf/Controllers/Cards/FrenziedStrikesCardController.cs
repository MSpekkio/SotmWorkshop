using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Handelabra.Sentinels.Engine.Controller;
using Handelabra.Sentinels.Engine.Model;

namespace SotmWorkshop.Moonwolf
{
    public class FrenziedStrikesCardController : MoonwolfCardController
    {
        public FrenziedStrikesCardController(Card card, TurnTakerController turnTakerController) : base(card, turnTakerController)
        {
        }

        private IEnumerator GrantACharacterPowerUsage()
        {
            AllowSetNumberOfPowerUseStatusEffect statusEffect = new AllowSetNumberOfPowerUseStatusEffect(2);
            statusEffect.UsePowerCriteria.IsSpecificCard = CharacterCard;
            statusEffect.UsePowerCriteria.CardSource = CharacterCard;
            statusEffect.UntilThisTurnIsOver(GameController.Game);
            statusEffect.UntilCardLeavesPlay(Card);
            statusEffect.UntilTargetLeavesPlay(CharacterCard);
            statusEffect.CardSource = Card;
            statusEffect.NumberOfUses = 1;

            IEnumerator coroutine = AddStatusEffect(statusEffect);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
        }

        public override void AddTriggers()
        {
            AddPhaseChangeTrigger(tt => tt == TurnTaker, p => p == Phase.UsePower,
                        pca => GameController.CanUsePowers(HeroTurnTakerController, CharacterCardController.GetCardSource()),
                        pca => GrantACharacterPowerUsage(),
                        new[] { TriggerType.IncreasePowerNumberOfUses }, TriggerTiming.After);

            //only trigger the potential damage effect during Moonwolf's turn, and on her innate power
            AddTrigger<UsePowerAction>(upa => Game.ActiveTurnTaker == TurnTaker && Game.ActiveTurnPhase.IsUsePower && upa.HeroUsingPower == HeroTurnTakerController && upa.Power.CardController == CharacterCardController && upa.Power.Index == 0, UsePowerResponse, TriggerType.DealDamage, TriggerTiming.After);
        }

        private IEnumerator UsePowerResponse(UsePowerAction action)
        {
            var list = Game.Journal.UsePowerEntriesThisTurn().Where(e => e.PowerUser == HeroTurnTaker && e.CardWithPower == CharacterCard && e.PowerIndex == 0).ToList();
            
            //only damage on the second usage in the turn.
            if (list.Count == 2)
            {
                var coroutine = DealDamage(CharacterCard, CharacterCard, 1, DamageType.Melee, cardSource: GetCardSource());
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
        }

        public override IEnumerator Play()
        {
            if (Game.ActiveTurnTaker == TurnTaker && Game.ActiveTurnPhase.IsUsePower)
            {
                IEnumerator coroutine = GrantACharacterPowerUsage();
                if (base.UseUnityCoroutines)
                {
                    yield return base.GameController.StartCoroutine(coroutine);
                }
                else
                {
                    base.GameController.ExhaustCoroutine(coroutine);
                }
            }
        }
    }
}