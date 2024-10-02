using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Motive", menuName = "PuzzleSystem/Killer/Motive")]
public class Motive : ScriptableObject
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
