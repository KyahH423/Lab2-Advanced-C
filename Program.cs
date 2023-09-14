/*       
 *--------------------------------------------------------------------
 * 	   File name: Program.cs
 * 	Project name: Lab2_Advanced_C#
 *--------------------------------------------------------------------
 * Author’s name and email:	 Kyah Hanson - hansonkm@etsu.edu				
 *          Course-Section:  CSCI-2910-800
 *           Creation Date:	 9/12/2023		
 * -------------------------------------------------------------------
 */

using Lab2_Advanced_C_;
using System.Diagnostics;
using System.Linq;


/******************************* PULL IN FILE DATA ********************************/
string projectRootFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
string filePath = projectRootFolder + "/videogames.csv";
List<VideoGame> gamesList = new List<VideoGame>();

using (StreamReader reader = new StreamReader(filePath))
{
    reader.ReadLine();

    while (!reader.EndOfStream)
    {
        string newGame = reader.ReadLine();
        string[] gameData = newGame.Split(',');

        VideoGame game = new VideoGame(gameData[0], gameData[1], Convert.ToInt32(gameData[2]), gameData[3], gameData[4], Convert.ToDouble(gameData[5]), Convert.ToDouble(gameData[6]), Convert.ToDouble(gameData[7]), Convert.ToDouble(gameData[8]), Convert.ToDouble(gameData[9]));
        gamesList.Add(game);
    }
    reader.Close();
}
gamesList.Sort();

List<VideoGame> gamesByYear = new List<VideoGame>();
gamesByYear = gamesList.OrderBy(x => x.Year).ToList();
Stack<VideoGame> newest = new Stack<VideoGame>();
Queue<VideoGame> oldest = new Queue<VideoGame>();

string menuOption;
string userInput;
int counter;
bool run = true;
bool addGames = true;
List<string> genreNum = new List<string>();




/******************************** GENRE DICTIONARY *********************************/
Dictionary<string, List<VideoGame>> genreDictionary = new Dictionary<string, List<VideoGame>>();
IEnumerable<string> allGenres = gamesList.Select(game => game.Genre.ToLower()).Distinct();
List<string> genres = allGenres.ToList();
genres.Sort();

List<VideoGame> newList = new List<VideoGame>();
for (int i = 0; i < genres.Count; i++)
{
    var gamesByGenre = gamesList.Where(VideoGame => VideoGame.Genre.ToLower() == genres[i]);
    foreach (VideoGame game in gamesByGenre)
    {
        newList.Add(game);
    }
}

// USed reference from https://www.codeproject.com/Tips/786253/Transform-a-List-To-Dictionary-of-Lists as inspiration
genreDictionary = newList.GroupBy(k => k.Genre, v => v).ToDictionary(g => g.Key, g => g.ToList());
newList.Clear();



/****************************** PUBLISHER DICTIONARY *******************************/
Dictionary<string, List<VideoGame>> publisherDictionary = new Dictionary<string, List<VideoGame>>();

IEnumerable<string> allPublishers = gamesList.Select(game => game.Publisher.ToLower()).Distinct();
List<string> publishers = allPublishers.ToList();
publishers.Sort();

for (int i = 0; i < publishers.Count; i++)
{
    var gamesByPublisher = gamesList.Where(VideoGame => VideoGame.Publisher.ToLower() == publishers[i]);
    foreach (VideoGame game in gamesByPublisher)
    {
        newList.Add(game);
    }
}
publisherDictionary = newList.GroupBy(k => k.Publisher, v => v).ToDictionary(g => g.Key, g => g.ToList());
newList.Clear();


/*********************************** USER MENU *************************************/
while (run == true)
{
    Console.Clear();
    Console.WriteLine("Welcome, would you like to view games within our data ");
    Console.WriteLine("1. View games by Genre\n2. View games by Publisher\n3. Access Wish List\n4. End Program");
    menuOption = Console.ReadLine();
    while ((int.TryParse(menuOption, out int id) == false) || Convert.ToInt32(menuOption) != 1 && Convert.ToInt32(menuOption) != 2 && Convert.ToInt32(menuOption) != 3 && Convert.ToInt32(menuOption) != 4)
    {
        Console.WriteLine("Error: Invalid Input");
        Console.WriteLine("Please enter 1,2,3, or 4 based on what you wish to do:");
        menuOption = Console.ReadLine();
    }
    switch (Convert.ToInt32(menuOption))
    {
        case 1:
            {
                Console.Clear();
                userInput = "Y";
                while (Convert.ToChar(userInput) == 'Y' || Convert.ToChar(userInput) == 'y')
                {
                    Console.Clear();
                    UseGenreDictionary();
                    CallByGenre(genreDictionary.ElementAt(Convert.ToInt32(userInput) - 1).Key);
                    Console.WriteLine("Would you like to enter another genre? Enter 'Y' for 'yes' or 'N' to return to the main menu: ");
                    userInput = Console.ReadLine();
                    userInput = YesOrNo(userInput);
                }
                Console.Clear();
                run = true;
                break;
            }
        case 2:
            {
                Console.Clear();
                userInput = "Y";
                while (Convert.ToChar(userInput) == 'Y' || Convert.ToChar(userInput) == 'y')
                {
                    Console.Clear();
                    UsePublisherDictionary();
                    CallByPublisher(publisherDictionary.ElementAt(Convert.ToInt32(userInput) - 1).Key);
                    Console.WriteLine("Would you like to enter another publisher? Enter 'Y' for 'yes' or 'N' to return to the main menu: ");
                    userInput = Console.ReadLine();
                    userInput = YesOrNo(userInput);
                }
                Console.Clear() ;
                run = true;
                break;
            }
        case 3:
            {
                
                Console.Clear();
                Console.WriteLine("Would you like to view all games starting with 1.Oldest or 2.Youngest:");
                userInput = Console.ReadLine();
                while ((int.TryParse(userInput, out int id) == false) || Convert.ToInt32(userInput) != 1 && Convert.ToInt32(userInput) != 2)
                {
                    Console.WriteLine("Error: Invalid Input\nPlease enter 1 for olderst or 2 for newest:");
                    userInput = Console.ReadLine();
                }

                // :|    I originally wanted to do a wish list but after 4 hours of coding I gave up after the 150+ lines were unsalvagable 
                // QUEUE EXAMPLE
                if (Convert.ToInt32(userInput) == 1)
                {
                    for (int i = 0; i < gamesByYear.Count; i++)
                    {
                        oldest.Enqueue(gamesByYear[i]);
                    }
                    foreach (var game in oldest)
                    {
                        Console.WriteLine($"{game.Name} released in {game.Year}");
                    }
                }

                // STACK EXAMPLE
                else
                {
                    for (int i = 0; i < gamesByYear.Count; i++)
                    {
                        newest.Push(gamesByYear[i]);
                    }
                    foreach(var game in newest)
                    {
                        Console.WriteLine($"{game.Name} released in {game.Year}");
                    }
                }
                Console.WriteLine("Press enter to returnt o the main menu after viewing the games");
                Console.ReadLine();

               
                break;
            }
        case 4:
            {
                run = false;
                break;
            }
        default:
            {
                break;
            }
    }
}





/****************************** METHODS *******************************/
void CallByPublisher(string publisher)
{
    Console.WriteLine(publisher);
    counter = 1;
    foreach (var game in publisherDictionary[publisher])
    {
        Console.WriteLine($"{counter}: {game}");
        counter++;
    }
}

void CallByGenre(string genre)
{
    Console.WriteLine(genre);
    counter = 1;
    foreach (var game in genreDictionary[genre])
    {
        Console.WriteLine($"{counter}: {game}");
        counter++;
    }
}

string YesOrNo(string input)
{
    while (string.IsNullOrWhiteSpace(input) == true || char.TryParse(input, out char id) == false || Convert.ToChar(input) != 'y' &&
            Convert.ToChar(input) != 'Y' && Convert.ToChar(input) != 'N' && Convert.ToChar(input) != 'n')
    {
        Console.WriteLine("Error: Invalid Input\nPlease enter 'Y' for yes or 'N' to stop:");
        input = Console.ReadLine();
    }
    return input;
}

void UseGenreDictionary()
{
    Console.Clear ();
    counter = 1;
    Console.WriteLine("Here is a list of all of the genres we have available: ");
    foreach (var key in genreDictionary.Keys)
    {
        Console.WriteLine($"{counter}. {key}");
        counter++;
    }
    Console.WriteLine("Please enter the number associated with the genre that you would like to view games for: ");
    userInput = Console.ReadLine();
    while ((int.TryParse(userInput, out int id) == false) || Convert.ToInt32(userInput) <= 0 || Convert.ToInt32(userInput) > genreDictionary.Count())
    {
        Console.WriteLine("Error: Invalid Input");
        Console.WriteLine("Please enter the number associated with the genre you wish to view:");
        userInput = Console.ReadLine();
    }
}

void UsePublisherDictionary()
{
    Console.Clear();
    counter = 1;
    Console.WriteLine("Here is a list of all of the publishers we have available: ");
    foreach (var key in publisherDictionary.Keys)
    {
        Console.WriteLine($"{counter}. {key}");
        counter++;
    }
    Console.WriteLine("Please enter the number associated with the publisher that you would like to view games for: ");
    userInput = Console.ReadLine();
    while ((int.TryParse(userInput, out int id) == false) || Convert.ToInt32(userInput) <= 0 || Convert.ToInt32(userInput) > publisherDictionary.Count())
    {
        Console.WriteLine("Error: Invalid Input");
        Console.WriteLine("Please enter the number associated with the publisher you wish to view:");
        userInput = Console.ReadLine();
    }
}

