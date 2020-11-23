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
        public FrenziedStrikesCardController(Card card, TurnTakerController turnTakerController)
            : base(card, turnTakerController)
        {
            base.GameController.AddCardControllerToList(CardControllerListType.IncreasePhaseActionCount, this);

        }

        private StatusEffect GrantACharacterPowerUsage()
        {
            AllowSetNumberOfPowerUseStatusEffect statusEffect = new AllowSetNumberOfPowerUseStatusEffect(2);
            statusEffect.UsePowerCriteria.IsSpecificCard = base.CharacterCard;
            statusEffect.UsePowerCriteria.CardSource = base.CharacterCard;
            statusEffect.UntilThisTurnIsOver(base.GameController.Game);
            statusEffect.CardDestroyedExpiryCriteria.Card = base.CharacterCard;
            statusEffect.NumberOfUses = 1;

            return statusEffect;
        }

        public override void AddTriggers()
        {

        }


        public override IEnumerator Play()
        {
            var statusEffect = GrantACharacterPowerUsage();
            IEnumerator coroutine = AddStatusEffect(statusEffect);
            if (base.UseUnityCoroutines)
            {
                yield return base.GameController.StartCoroutine(coroutine);
            }
            else
            {
                base.GameController.ExhaustCoroutine(coroutine);
            }
            yield break;
            /*
			if (base.GameController.ActiveTurnTaker == base.TurnTaker)
			{
				AllowSetNumberOfPowerUseStatusEffect allowSetNumberOfPowerUseStatusEffect = new AllowSetNumberOfPowerUseStatusEffect(2);
				allowSetNumberOfPowerUseStatusEffect.UsePowerCriteria.IsSpecificCard = base.CharacterCard;
				allowSetNumberOfPowerUseStatusEffect.UsePowerCriteria.CardSource = base.CharacterCard;
				allowSetNumberOfPowerUseStatusEffect.UntilThisTurnIsOver(base.GameController.Game);
				allowSetNumberOfPowerUseStatusEffect.CardDestroyedExpiryCriteria.Card = base.CharacterCard;
				allowSetNumberOfPowerUseStatusEffect.NumberOfUses = 1;
				coroutine = AddStatusEffect(allowSetNumberOfPowerUseStatusEffect);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine);
				}
				yield break;
			}

			int timesUsed = base.Journal.UsePowerEntriesThisTurn()
								.Where(e => e.CardWithPower == base.CharacterCard)
								.Count();

			if (timesUsed < 2)
			{
				List<YesNoCardDecision> storedResults = new List<YesNoCardDecision>();
				SelectionType type = SelectionType.UsePowerTwice;
				if (timesUsed == 1)
				{
					type = SelectionType.UsePowerAgain;
				}
				IEnumerator coroutine2 = base.GameController.MakeYesNoCardDecision(base.HeroTurnTakerController, type, base.CharacterCard, null, storedResults, null, GetCardSource());
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine2);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine2);
				}
				if (!DidPlayerAnswerYes(storedResults))
				{
					yield break;
				}
				for (int i = 0; i < 2 - timesUsed; i++)
				{
					coroutine2 = UsePowerOnOtherCard(base.CharacterCard);
					if (base.UseUnityCoroutines)
					{
						yield return base.GameController.StartCoroutine(coroutine2);
					}
					else
					{
						base.GameController.ExhaustCoroutine(coroutine2);
					}
				}
			}
			else
			{
				IEnumerator coroutine3 = base.GameController.SendMessageAction(base.TurnTaker.Name + " has already used " + base.CharacterCard.Definition.Body.First() + " twice this turn.", Priority.High, GetCardSource(), null, true);
				if (base.UseUnityCoroutines)
				{
					yield return base.GameController.StartCoroutine(coroutine3);
				}
				else
				{
					base.GameController.ExhaustCoroutine(coroutine3);
				}
			}
			*/
        }
    }
}
