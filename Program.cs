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

        VideoGame game = new VideoGame(gameData[0], gameData[1], gameData[2], gameData[3], gameData[4], Convert.ToDouble(gameData[5]), Convert.ToDouble(gameData[6]), Convert.ToDouble(gameData[7]), Convert.ToDouble(gameData[8]), Convert.ToDouble(gameData[9]));
        gamesList.Add(game);
    }
    reader.Close();
}
gamesList.Sort();


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
// Used reference from https://www.codeproject.com/Tips/786253/Transform-a-List-To-Dictionary-of-Lists as inspiration
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
    Console.WriteLine("1. View games by Genre\n2. View games by Publisher\n3. Add to Wish List\n4. Access Wish List\n5. End Program");
    menuOption = Console.ReadLine();
    while ((int.TryParse(menuOption, out int id) == false) || Convert.ToInt32(menuOption) != 1 && Convert.ToInt32(menuOption) != 2 && Convert.ToInt32(menuOption) != 3 && Convert.ToInt32(menuOption) != 4)
    {
        Console.WriteLine("Error: Invalid Input");
        Console.WriteLine("Please enter 1,2, or 3 based on what you wish to do:");
        menuOption = Console.ReadLine();
    }
    switch (Convert.ToInt32(menuOption))
    {
        case 1:
            {
                // Displays Genre Information
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
                // Displays Publisher Information
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
                // Adding games to wish list 
                // I'm so sorry, hours of pain

                Console.Clear();
                Console.WriteLine("Please select a genre to view games for");
                UseGenreDictionary();
                string userG = userInput;
                CallByGenre(genreDictionary.ElementAt(Convert.ToInt32(userG) - 1).Key);
                int gc = 0;
                foreach (var g in genreDictionary.ElementAt(Convert.ToInt32(userG) - 1).Value)
                {
                    gc++;
                }
                addGames = true;
                while (addGames == true)
                {
                    VideoGame newGame = new VideoGame();
                    counter = genreNum.Count + 1;
                    while (oldest.Count < 3)
                    {
                        Console.WriteLine("You must add atleast 3 games to your wish list:");
                        Console.WriteLine($"Enter Game {counter}: ");
                        userInput = Console.ReadLine();
                        
                        while ((int.TryParse(userInput, out int id) == false) || Convert.ToInt32(userInput) <= 0 || Convert.ToInt32(userInput) > gc || genreNum.Contains($"{userG}.{userInput}"))
                        {
                            Console.WriteLine("That game does not exist or has already been added to your list, please enter a valid number: ");
                            userInput = Console.ReadLine();
                        }

                        var i = 0;
                        foreach (var g in genreDictionary.ElementAt(Convert.ToInt32(userG) - 1).Value)
                        {
                            if (Convert.ToInt32(userInput) - 1 == i)
                            {
                                newGame = new VideoGame(g);
                            }
                            i++;
                        }
                        oldest.Enqueue(newGame);
                        newest.Push(newGame);
                        genreNum.Add($"{userG}.{userInput}");
                        counter++;        
                    }
                    Console.WriteLine("Would you like to add another game to your Wish List? Enter 'Y' for YES or 'N' to return to the main menu:");
                    userInput = Console.ReadLine();
                    userInput = YesOrNo(userInput);
                    if (Convert.ToChar(userInput) == 'Y' || Convert.ToChar(userInput) == 'y')
                    {
                        Console.WriteLine($"Enter Game {counter}: ");
                        userInput = Console.ReadLine();
                        while ((int.TryParse(userInput, out int id) == false) || Convert.ToInt32(userInput) <= 0 || Convert.ToInt32(userInput) > gc || genreNum.Contains($"{userG}.{userInput}"))
                        {
                            Console.WriteLine("That game does not exist or has already been added to your list, please enter a valid number: ");
                            userInput = Console.ReadLine();
                        }
                        var i = 0;
                        foreach (var g in genreDictionary.ElementAt(Convert.ToInt32(userG) - 1).Value)
                        {
                            if (Convert.ToInt32(userInput) - 1 == i)
                            {
                                newGame = new VideoGame(g);
                            }
                            i++;
                        }
                        oldest.Enqueue(newGame);
                        newest.Push(newGame);
                        genreNum.Add($"{userG}.{userInput}");
                        counter++;
                    }
                    else
                    {
                        addGames = false;
                        continue;
                    }
                }
                break;
            }
        case 4:
            {
                Console.Clear();
                if(oldest.Count == 0)
                {
                    Console.WriteLine("Your wish list is currently empty, please add to wish list to view.\nPress the enter key to return to the main menu");
                    Console.ReadLine();
                    break;
                }
                else
                {
                    Console.WriteLine("Would you like to view your wish list by 1. Oldest or 2. Newest: ");
                    userInput = Console.ReadLine() ;
                    while((int.TryParse(userInput, out int id) == false) || Convert.ToInt32(userInput) != 1 && Convert.ToInt32(userInput) != 2)
                    {
                        Console.WriteLine("Error: Invalid Input");
                        Console.WriteLine("Please enter 1 or 2:");
                        userInput = Console.ReadLine();
                    }


                    if(Convert.ToInt32(userInput) == 1)
                    {
                        Console.WriteLine("Here is your wish list starting at your oldest additions:");
                        foreach(VideoGame g in oldest)
                        {
                            Console.WriteLine(g);
                        }
                        Console.WriteLine("When you are done, please press enter to return to the main menu:");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.WriteLine("Here is your wish list starting at your newest additions:");
                        foreach (VideoGame g in newest)
                        {
                            Console.WriteLine(g);
                        }
                        Console.WriteLine("When you are done, please press enter to return to the main menu:");
                        Console.ReadLine();
                    }
                }
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
