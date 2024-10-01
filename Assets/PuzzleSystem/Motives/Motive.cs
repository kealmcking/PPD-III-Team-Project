using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Case", menuName = "PuzzleSystem/Case")]
public class Motive : ScriptableObject
{
    [SerializeField,Tooltip("This can be a unique name for the killers 'campaign' ")] string caseName;
    [SerializeField] List<Lore> lore = new List<Lore>();
    [SerializeField] List<Puzzle> puzzles = new List<Puzzle>();
    public List<Puzzle> Puzzles => puzzles;
    public List<Lore> Lore => lore;
    public string CaseName => caseName;
}
