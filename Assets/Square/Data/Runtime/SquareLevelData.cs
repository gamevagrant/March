using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class SquareLevelData
{
  [SerializeField]
  int id;
  public int Id { get {return id; } set { id = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { name = value;} }
  
  [SerializeField]
  string background;
  public string Background { get {return background; } set { background = value;} }
  
  [SerializeField]
  int[] squareitemids = new int[0];
  public int[] Squareitemids { get {return squareitemids; } set { squareitemids = value;} }
  
}