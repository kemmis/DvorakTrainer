<Query Kind="Statements">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

Directory.SetCurrentDirectory (Path.GetDirectoryName (Util.CurrentQueryPath));

// read in all words from text file
string allWordsString = File.ReadAllText("words.txt");
// split words up into individual strings
var allWordsList = allWordsString.Split(new string [] { "\n"}, StringSplitOptions.RemoveEmptyEntries);
// filter out all words < 4 characters long
var wordsLongEnough = allWordsList.Where(w=>w.Trim().Length > 3).Select(w=>w.Trim());

var level1Characters = "aoeuhtns".ToCharArray();
var level1Words = wordsLongEnough.Where(w=> w.ToCharArray().All(l=>level1Characters.Contains(l))).ToList();
var level1WordsJson = JsonConvert.SerializeObject(level1Words);
File.WriteAllText("level1.json",level1WordsJson);

var level2Characters = "aoeuidhtns".ToCharArray();
var level2Words = wordsLongEnough.Where(w=> w.ToCharArray().All(l=>level2Characters.Contains(l))).ToList();
var level2WordsJson = JsonConvert.SerializeObject(level2Words);
File.WriteAllText("level2.json",level2WordsJson);

var level3Characters = "aoeuidhtnscfklmprv".ToCharArray();
var level3Words = wordsLongEnough.Where(w=> w.ToCharArray().All(l=>level3Characters.Contains(l))).ToList();
var level3WordsJson = JsonConvert.SerializeObject(level3Words);
File.WriteAllText("level3.json",level3WordsJson);

var level4Characters = "aoeuidhtnscfklmprvbgjqwxyz".ToCharArray();
var level4Words = wordsLongEnough.Where(w=> w.ToCharArray().All(l=>level4Characters.Contains(l))).ToList();
var level4WordsJson = JsonConvert.SerializeObject(level4Words);
File.WriteAllText("level4.json",level4WordsJson);