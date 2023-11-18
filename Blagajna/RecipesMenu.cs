namespace Blagajna;

public class RecipesMenu
{
    static void DeleteEmptyArticles(Dictionary<string, (int, float, DateTime)> articles)
    {
        foreach (var item in articles)
        {
            if (item.Value.Item1 == 0)
            {
                articles.Remove(item.Key);
            }
        }
    }
    static bool CheckUnique(string checkArticle, Dictionary<int, (DateTime, Dictionary<string, int>)> recipes)
    {
        foreach (var item in recipes[recipes.Keys.Last()].Item2)
        {
            if (checkArticle == item.Key)
            {
                return true;
            }
        }
        return false;
    }
    static bool ConfirmChange(string yesNo)
    {
        if (yesNo.ToUpper() == "DA")
        {
            return true;
        }
        else if (yesNo.ToUpper() == "NE")
        {
            return false;
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Unesite da ili ne");
            string yesNo2 = Console.ReadLine();
            ConfirmChange(yesNo2);
        }

        return true;
    }
    static void returnToMain(){
        Console.WriteLine("Povratak na glavni izbornik (enter)");
        Console.ReadKey();
        Console.Clear();
    }
    static bool CheckArticle(string ar, Dictionary<string, (int, float, DateTime)> articles)
    {
        foreach (var item in articles)
        {
            if (ar == item.Key)
            {
                return true;
            }
        }
        return false;
    }
    static bool CheckRecipe(int ar, Dictionary<int, (DateTime, Dictionary<string, int>)> recipes)
    {
        foreach (var item in recipes)
        {
            if (ar == item.Key)
            {
                return true;
            }
        }
        return false;
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
    static void writeBorder()
    {
        string border = "--";
        for (int i = 0; i < 5; i++)
        {
            border += border;
        }
        Console.WriteLine(border);
    }
    static void printRecipe(int checkRecipe, Dictionary<string, (int, float, DateTime)> articles, Dictionary<int, (DateTime, Dictionary<string, int>)> recipes)
    {
        writeBorder();
        Console.WriteLine($"ID RAČUNA: {checkRecipe}\nDATUM IZDAVANJA: {recipes[checkRecipe].Item1}" +
                          $"\n\nPROIZVODI:\nNaziv\t\tKoličina\tCijena\t\tUkupno");
        foreach (var item in recipes[checkRecipe].Item2)
        {
            float fullArticlePrice = articles[item.Key].Item2 * item.Value;
            Console.WriteLine($"{item.Key}\t\t{item.Value}\t\t{articles[item.Key].Item2}e\t\t{fullArticlePrice}e");
        }

        Console.WriteLine($"\n{TotalValue(checkRecipe, articles, recipes)}");
        writeBorder();
    }
    static string TotalValue(int a, Dictionary<string, (int, float, DateTime)> articles, Dictionary<int, (DateTime, Dictionary<string, int>)> recipes)
    {
        float total = 0;
        foreach (var itemRecipe in recipes[a].Item2)
        {
            foreach (var itemArticle in articles)
            {
                if (itemArticle.Key == itemRecipe.Key)
                {
                    total += itemArticle.Value.Item2 * itemRecipe.Value;
                }
            }
        }

        string s = "Ukupna cijena računa: " + total.ToString("0.00") + "e";
        return s;
    }

    public void Menu(Dictionary<int, (DateTime, Dictionary<string, int>)> recipes, Dictionary<string, DateTime> workers,
    Dictionary<string, (int, float, DateTime)> articles)
{     
    Console.Clear();
    Console.WriteLine("1 - Unos računa\n2 - Ispis računa\n0 - Povratak");
    RChoice:
    int recipesChoice = CheckAnswer(Console.ReadLine());
    switch (recipesChoice)
    {
        case 1:
            int temp = 0;
            Console.Clear();
            SelectArticle:
            Console.WriteLine("ODABERITE PROIZVOD KOJI ŽELITE UNIJETI: \n" +
                              "Naziv\t\tKoličina\tCijena\t\tVrijeme Trajanja");
            foreach (var item in articles)
            {   
                Console.WriteLine($"{item.Key}\t\t{item.Value.Item1}\t\t{item.Value.Item2}\t\t{item.Value.Item3.ToString("yyyy.MM.dd")}");
            }
            Console.WriteLine("\nZa printanje računa pritisnite enter");
            string newArticle = Console.ReadLine();
            if (CheckArticle(newArticle, articles))
            {
                if (temp == 0)
                {
                    Console.WriteLine("Unesite količinu");
                    int newQuantity = CheckAnswer(Console.ReadLine());
                    if (newQuantity < articles[newArticle].Item1)
                    {
                        recipes.Add(recipes.Keys.Last() + 1 , (DateTime.Now, new Dictionary<string, int>()
                        {
                            {newArticle, newQuantity}
                        }));
                        articles[newArticle] = (articles[newArticle].Item1-newQuantity, articles[newArticle].Item2,
                            articles[newArticle].Item3);
                        Console.Clear();
                        Console.WriteLine($"\nDodan je artikal {newArticle}, količina {newQuantity}\n");
                        temp++;
                    }
                    else if (newQuantity == articles[newArticle].Item1)
                    {
                        recipes.Add(recipes.Keys.Last() + 1 , (DateTime.Now, new Dictionary<string, int>()
                        {
                            {newArticle, newQuantity}
                        }));
                        articles[newArticle] = (0, articles[newArticle].Item2,
                            articles[newArticle].Item3);
                        Console.Clear();
                        Console.WriteLine($"\nDodan je artikal {newArticle}, količina {newQuantity}\n");
                        temp++;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\nUnijeli ste veću količinu nego je raspoloživo!\n");
                    }
                    goto SelectArticle;
                }
                if (CheckUnique(newArticle, recipes))
                {
                    Console.Clear();
                    Console.WriteLine("\nArtikal je već unesen u račun, unesite novi\n");
                    goto SelectArticle;
                }
                else
                {
                    Console.WriteLine("Unesite količinu");
                    int newQuantity = CheckAnswer(Console.ReadLine());
                    if (newQuantity < articles[newArticle].Item1)
                    {
                        recipes[recipes.Keys.Last()].Item2.Add(newArticle, newQuantity);
                        articles[newArticle] = (articles[newArticle].Item1-newQuantity, articles[newArticle].Item2,
                            articles[newArticle].Item3);
                        Console.Clear();
                        Console.WriteLine($"\nDodan je artikal {newArticle}, količina {newQuantity}\n");
                    }
                    else if (newQuantity == articles[newArticle].Item1)
                    {
                        recipes[recipes.Keys.Last()].Item2.Add(newArticle, newQuantity);
                        articles[newArticle] = (0, articles[newArticle].Item2,
                            articles[newArticle].Item3);
                        Console.Clear();
                        Console.WriteLine($"\nDodan je artikal {newArticle}, količina {newQuantity}\n");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\nUnijeli ste veću količinu nego je raspoloživo!\n");
                    }
                    goto SelectArticle;
                }
            }
            if (newArticle == "")
            {
                if (temp != 0)
                {
                    Console.Clear();
                    printRecipe(recipes.Keys.Last(), articles, recipes);
                    Console.WriteLine("Potvrdite ispis (da/ne)");
                    string exit = Console.ReadLine();
                    if (ConfirmChange(exit))
                    {
                        Console.Clear();
                        Console.WriteLine("Račun je uspješno ispisan.\nPREGLED RAČUNA:\n");
                        printRecipe(recipes.Keys.Last(), articles, recipes);
                        DeleteEmptyArticles(articles);
                        returnToMain();
                    }
                    else
                    {
                        Console.Clear();
                        recipes.Remove(recipes.Keys.Last());
                        Console.WriteLine("Račun nije spremljen niti ispisan.");
                        returnToMain();
                    }
                }

                if (temp == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Niste unijeli niti jedan prizvod!");
                    returnToMain();
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\n//Unesi jedan od ponuđenih artikala//\n");
                goto SelectArticle;
            }
            break;
        case 2:
            Console.Clear();
            Console.WriteLine("Povijest računa:");
            foreach (var item in recipes)
            {
                Console.WriteLine($"{item.Key} vrijeme izdavanja: {item.Value.Item1} {TotalValue(item.Key, articles, recipes)}");
            }
            Console.WriteLine("Za više informacija o računu upišite id računa: ");
            int checkRecipe = CheckAnswer(Console.ReadLine());
            if (CheckRecipe(checkRecipe, recipes))
            {
                Console.Clear();
                printRecipe(checkRecipe, articles, recipes);
                returnToMain();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Nepostoji račun s unesenim id-om");
                returnToMain();
            }
            break;
        case 0:
            Console.Clear();
            return;
        default:
            Console.Clear();
            Console.WriteLine("Unesite ponuđene brojeve.");
            goto RChoice;
        }
    }
}