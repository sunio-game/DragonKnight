using UnityEngine;
namespace RPG.DialogueSystem
{
    public interface IPredicateEvaluators
    {
        bool? Evaluator(DialogueConditionType predicate, string parameters);
    }
}