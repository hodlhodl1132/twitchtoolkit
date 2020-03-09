using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rim_twitch;
using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Settings;
using TwitchToolkit.Store;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Traits
{
    [StaticConstructorOnStartup]
    public static class AllTraits
    {
        static AllTraits()
        {
            List<TraitDef> traitDefs = DefDatabase<TraitDef>.AllDefs.ToList();
            foreach (TraitDef def in traitDefs)
            {
                if (def.degreeDatas != null)
                {
                    foreach (TraitDegreeData degree in def.degreeDatas)
                    {
                        buyableTraits.Add(new BuyableTrait(def, string.Join("", degree.label.Split(' ')).ToLower(), degree.degree));
                    }
                }
                else
                {
                    buyableTraits.Add(new BuyableTrait(def, string.Join("", def.label.Split(' ')).ToLower()));
                }
            }
        }

        public static List<BuyableTrait> buyableTraits = new List<BuyableTrait>();
    }

    public class BuyableTrait
    {
        public TraitDef def;
        public string label;
        public int degree;

        public BuyableTrait(TraitDef def, string label, int degree = 0)
        {
            this.def = def;
            this.label = label;
            this.degree = degree;
        }
    }

    public static class TraitHelpers
    {
        public static int FinalLevelOfSkill(Pawn pawn, SkillDef sk)
        {
            float num;
            if (sk.usuallyDefinedInBackstories)
            {
                num = (float)Rand.RangeInclusive(0, 4);
            }
            else
            {
                num = Rand.ByCurve(LevelRandomCurve);
            }
            foreach (Backstory backstory in from bs in pawn.story.AllBackstories
                                            where bs != null
                                            select bs)
            {
                foreach (KeyValuePair<SkillDef, int> keyValuePair in backstory.skillGainsResolved)
                {
                    if (keyValuePair.Key == sk)
                    {
                        num += (float)keyValuePair.Value * Rand.Range(1f, 1.4f);
                    }
                }
            }
            for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
            {
                int num2 = 0;
                if (pawn.story.traits.allTraits[i].CurrentData.skillGains.TryGetValue(sk, out num2))
                {
                    num += (float)num2;
                }
            }
            float num3 = Rand.Range(1f, AgeSkillMaxFactorCurve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears));
            num *= num3;
            num = LevelFinalAdjustmentCurve.Evaluate(num);
            return Mathf.Clamp(Mathf.RoundToInt(num), 0, 20);
        }

        private static readonly SimpleCurve AgeSkillMaxFactorCurve = new SimpleCurve
        {
            {
                new CurvePoint(0f, 0f),
                true
            },
            {
                new CurvePoint(10f, 0.7f),
                true
            },
            {
                new CurvePoint(35f, 1f),
                true
            },
            {
                new CurvePoint(60f, 1.6f),
                true
            }
        };

        private static readonly SimpleCurve LevelFinalAdjustmentCurve = new SimpleCurve
        {
            {
                new CurvePoint(0f, 0f),
                true
            },
            {
                new CurvePoint(10f, 10f),
                true
            },
            {
                new CurvePoint(20f, 16f),
                true
            },
            {
                new CurvePoint(27f, 20f),
                true
            }
        };

        private static readonly SimpleCurve LevelRandomCurve = new SimpleCurve
        {
            {
                new CurvePoint(0f, 0f),
                true
            },
            {
                new CurvePoint(0.5f, 150f),
                true
            },
            {
                new CurvePoint(4f, 150f),
                true
            },
            {
                new CurvePoint(5f, 25f),
                true
            },
            {
                new CurvePoint(10f, 5f),
                true
            },
            {
                new CurvePoint(15f, 0f),
                true
            }
        };
    }

    public class AddTrait : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} syntax is {this.storeIncident.syntax}");
                return false;
            }

            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (!gameComponent.HasUserBeenNamed(viewer.username))
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} you must be in the colony to use this command.");
                return false;
            }

            pawn = gameComponent.PawnAssignedToUser(viewer.username);

            float customMaxTraits = AddTraitSettings.maxTraits > 0 ? AddTraitSettings.maxTraits : 4;

            if (pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Count >= customMaxTraits)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} your pawn already has max {customMaxTraits} traits.");
                return false;
            }

            string traitKind = command[2].ToLower();

            BuyableTrait search = AllTraits.buyableTraits.Find(s => traitKind == s.label);


            if (search == null)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} trait {traitKind} not found.");
                return false;
            }

            buyableTrait = search;
            traitDef = buyableTrait.def;

            //if (!pawn.story.traits.allTraits.Any((Trait tr) =>
            //    traitDef.ConflictsWith(tr)) &&
            //    (traitDef.conflictingTraits == null ||
            //    !traitDef.conflictingTraits.Any((TraitDef tr) => pawn.story.traits.HasTrait(tr))))
            //{
            //    return true;
            //}

            trait = new Trait(traitDef, buyableTrait.degree);

            foreach (Trait tr in pawn.story.traits.allTraits)
            {
                if (tr.def.ConflictsWith(trait) || traitDef.ConflictsWith(tr))
                {
                    MessageQueue.messageQueue.Enqueue($"@{viewer.username} {traitDef.defName} conflicts with your pawn's trait {tr.LabelCap}.");
                    return false;
                }
            }

            if (pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Find(s => s.def.defName == search.def.defName) != null)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} you already have this trait of this type.");
                return false;
            }

            return true;
        }

        public override void TryExecute()
        {
            pawn.story.traits.GainTrait(trait);

            TraitDegreeData traitDegreeData = traitDef.DataAtDegree(buyableTrait.degree);

            if (traitDegreeData != null)
            {
                if (traitDegreeData.skillGains != null)
                {
                    foreach (KeyValuePair<SkillDef, int> pair in traitDegreeData.skillGains)
                    {
                        SkillRecord skill = pawn.skills.GetSkill(pair.Key);
                        int num = TraitHelpers.FinalLevelOfSkill(pawn, pair.Key);
                        skill.Level = num;
                    }
                }
            }

            Viewer.TakeViewerCoins(storeIncident.cost);
            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} just added the trait " + trait.Label + " to " + pawn.Name + ".");
            string text = $"{Viewer.username} has purchased " + trait.LabelCap + " for " + pawn.Name + ".";
            Current.Game.letterStack.ReceiveLetter("Trait", text, LetterDefOf.PositiveEvent, pawn);
        }

        public override Viewer Viewer { get; set; }

        private bool separateChannel = false;
        private Pawn pawn = null;
        private TraitDef traitDef = null;
        private Trait trait = null;
        private BuyableTrait buyableTrait = null;
    }

    public class RemoveTrait : IncidentHelperVariables
    {
        public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
        {
            this.separateChannel = separateChannel;
            this.Viewer = viewer;
            string[] command = message.Split(' ');
            if (command.Length < 3)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} syntax is {this.storeIncident.syntax}");
                return false;
            }

            GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (!gameComponent.HasUserBeenNamed(viewer.username))
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} you must be in the colony to use this command.");
                return false;
            }

            pawn = gameComponent.PawnAssignedToUser(viewer.username);

            if (pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Count <= 0)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} your pawn doesn't have any traits.");
                return false;
            }

            string traitKind = command[2].ToLower();

            BuyableTrait search = AllTraits.buyableTraits.Find(s => traitKind == s.label);

            if (search == null)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} trait {traitKind} not found.");
                return false;
            }

            buyableTrait = search;

            return true;
        }

        public override void TryExecute()
        {
            Trait traitToRemove = pawn.story.traits.allTraits.Find(s => s.def.defName == buyableTrait.def.defName);
            if (traitToRemove != null)
            {
                pawn.story.traits.allTraits.Remove(traitToRemove);

                TraitDegreeData traitDegreeData = traitToRemove.def.DataAtDegree(buyableTrait.degree);

                if (traitDegreeData != null)
                {
                    if (traitDegreeData.skillGains != null)
                    {
                        foreach (KeyValuePair<SkillDef, int> pair in traitDegreeData.skillGains)
                        {
                            SkillRecord skill = pawn.skills.GetSkill(pair.Key);
                            skill.Level -= pair.Value;
                        }
                    }
                }
            }
            else
            {
                return;
            }
            Viewer.TakeViewerCoins(storeIncident.cost);
            Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
            VariablesHelpers.SendPurchaseMessage($"@{Viewer.username} just removed the trait " + buyableTrait.label.CapitalizeFirst() + " from " + pawn.Name + ".");
            string text = $"{Viewer.username} has purchased trait removal of " + buyableTrait.label.CapitalizeFirst() + " from " + pawn.Name + ".";
            Current.Game.letterStack.ReceiveLetter("Trait", text, LetterDefOf.PositiveEvent, pawn);
        }

        public override Viewer Viewer { get; set; }

        private bool separateChannel = false;
        private Pawn pawn = null;
        private BuyableTrait buyableTrait = null;
    }
}
