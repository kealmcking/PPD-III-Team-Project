using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Case", menuName = "PuzzleSystem/Killer/Case")]
public class Case : ScriptableObject
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
