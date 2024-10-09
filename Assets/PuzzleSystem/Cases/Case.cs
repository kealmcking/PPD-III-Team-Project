using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Case represents the possible puzzles and lore for a given game. 
/// </summary>
[CreateAssetMenu(fileName = "Case", menuName = "PuzzleSystem/Killer/Case")]
public class Case : ScriptableObject, ICustomizableComponent
{
    [SerializeField] CaseDifficulty difficulty = CaseDifficulty.Easy;
    [SerializeField,Tooltip("This can be a unique name for the killers 'case' ")] string caseName;
    [SerializeField] List<Lore> lore = new List<Lore>();
    [SerializeField] List<Puzzle> puzzles = new List<Puzzle>(); 
    public List<Puzzle> Puzzles => puzzles;
    public List<Lore> Lore => lore;
    public string CaseName => caseName;
    public CaseDifficulty Difficulty => difficulty;    
}
public enum CaseDifficulty
{
    Easy,
    Medium,
    Hard,
    Impossible,
}
