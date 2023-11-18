
using Blagajna;

var articles = new Dictionary<string, (int, float, DateTime)>()
{
    {"Banana", (20, 1.1f, new DateTime(2023, 12, 1))},
    {"Duhan", (20, 4f, new DateTime(2024, 1, 1))},
    {"Kolač", (11, 2f, new DateTime(2023, 11, 29))},
    {"Mlijeko", (10, 1.5f, new DateTime(2023, 11, 30))},
    {"Jogurt", (15, 1.9f, new DateTime(2024, 1, 10))},
    {"Poriluk", (12, 0.8f, new DateTime(2021, 12, 15))},
    {"Nutella", (6, 20f, new DateTime(2024, 9, 7))},
    {"Fanta", (22, 0.7f, new DateTime(2024, 4, 26))}
};
var workers = new Dictionary<string, DateTime>()
{
    { "Mate Matić", new DateTime(1990, 11, 3) },
    { "Jure Jurić", new DateTime(1999, 6, 12) },
    { "Frane Franić", new DateTime(2002, 12, 22) },
    { "Nika Nikolić", new DateTime(2001, 10, 30) },
    { "Sveto Svetić", new DateTime(1954, 9, 4) }
};

var recipes = new Dictionary<int, (DateTime, Dictionary<string, int>)>()
{
    {111, (new DateTime(2023, 10, 16, 10, 9, 11), new Dictionary<string, int>()
    {
        {"Banana", 2},
        {"Jogurt", 3}
    })},
    {112, (new DateTime(2023, 10, 17, 11, 1, 9), new Dictionary<string, int>()
    {
        {"Banana", 3},
        {"Jogurt", 2},
        {"Fanta", 1}
    })},
    {113, (new DateTime(2023, 10, 13, 8, 59, 5), new Dictionary<string, int>()
    {
        {"Duhan", 1},
        {"Kolač", 5},
        {"Banana", 6},
        {"Mlijeko", 1}
    })}
};
static string ReadLineWithMask()//preuzeto s githuba radi estetike
{
    string pass = "";
    ConsoleKeyInfo key;

    do
    {
        key = Console.ReadKey(true);

        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
        {
            pass += key.KeyChar;
            Console.Write("*");
        }
        else
        {
            if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
            {
                pass = pass.Substring(0, (pass.Length - 1));
                Console.Write("\b \b");
            }
        }
    }
    while (key.Key != ConsoleKey.Enter);

    return pass;
}
void Password()
{
    Console.Clear();
    Console.WriteLine("Unesi lozinku: ");
    string pass = "admin";
    string enteredPass = ReadLineWithMask();
    if (enteredPass != pass)
    {
        Console.Clear();
        Console.WriteLine("Netočna lozinka");
        ReturnToMain();
    }
    else
    {
        Console.Clear();
        StatsMenu s = new StatsMenu();
        s.Menu(recipes, workers, articles);
    }
}
static void ReturnToMain(){
    Console.WriteLine("Povratak na glavni izbornik (enter)");
    Console.ReadKey();
    Console.Clear();
}
static int CheckAnswer(string a)
{
    int n;
    while (int.TryParse(a ,out n) == false)
    {
        Console.Clear();
        Console.WriteLine("Unesite broj.");
        a = Console.ReadLine();
    }
    return n;
}
static bool ConfirmChange(string yesNo)
{
    if (yesNo.ToUpper() == "DA")
    {
        return true;
    }
    if (yesNo.ToUpper() == "NE")
    {
        return false;
    }
    Console.Clear();
    Console.WriteLine("Unesite da ili ne");
    string yesNo2 = Console.ReadLine();
    ConfirmChange(yesNo2);

    return true;
}

ThreadStart:
Console.WriteLine("Dobrodošli u MALI DUĆAN l.t.d \nodaberite sljedeće opcije:\n" + 
                  " 1 - Artikli\n 2 - Radnici \n 3 - Racuni \n 4 - Statistika \n 0 - Izlaz" );
Choice:

int mainChoice = CheckAnswer(Console.ReadLine());
switch (mainChoice)
{
    case 1:
        ArticlesMenu a = new ArticlesMenu();
        a.Menu(articles, recipes);
        goto ThreadStart;
    case 2:
        WorkersMenu w = new WorkersMenu();
        w.Menu(workers);
        goto ThreadStart;
    case 3:
        RecipesMenu r = new RecipesMenu();
        r.Menu(recipes, workers, articles);
        goto ThreadStart;
    case 4:
        Password();
        goto ThreadStart;
    case 0:
        Console.Clear();
        Console.WriteLine("Želite li izaći iz aplikacije (da/ne)?");
        string exit = Console.ReadLine();
        if (ConfirmChange(exit))
        {
            Environment.Exit(0);
        }
        else
        {
            Console.Clear();
            goto ThreadStart;
        }
        break;
    default:
        Console.Clear();
        Console.WriteLine("Unesite ponuđene brojeve.");
        goto Choice;
}     
