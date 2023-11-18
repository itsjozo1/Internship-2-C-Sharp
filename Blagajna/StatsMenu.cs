namespace Blagajna;

public class StatsMenu
{
    static double TotalValue(int a, Dictionary<string, (int, float, DateTime)> articles, Dictionary<int, (DateTime, Dictionary<string, int>)> recipes)
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


        return total;
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
    static void ReturnToMain(){
        Console.WriteLine("Povratak na glavni izbornik (enter)");
        Console.ReadKey();
        Console.Clear();
    }

    public void Menu(Dictionary<int, (DateTime, Dictionary<string, int>)> recipes, Dictionary<string, DateTime> workers,
    Dictionary<string, (int, float, DateTime)> articles)
{
    Console.Clear();
    Console.WriteLine("1 - Ukupan broj artikala u trgovini\n2 - Vrijednost neprodanih artikala\n3 - Vrijednost prodanih artikala" +
                      "\n4 - Stanje po mjesecima\n0 - Povratak");
    SChoice:
    int SChoice = CheckAnswer(Console.ReadLine());
    switch (SChoice)
    {
        case 1:
            Console.Clear();
            int count = 0;
            Console.WriteLine($"SVI ARTIKLI: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\tUkupno");
            foreach (var item in articles)
            {
                Console.WriteLine($"{item.Key}\t\t{item.Value.Item1}\t\t{item.Value.Item2}e\t\t" +
                                  $"{item.Value.Item3.ToString("dd.MM.yyyy")}\t\t" +
                                  $"{item.Value.Item1 * item.Value.Item2}e");
                count++;
            }
            Console.WriteLine($"UKUPAN BROJ ARTIKALA: {count}");
            ReturnToMain();
            break;
        case 2:
            Console.Clear();
            float valueUnsell = 0;
            var uniqueArticles = articles.Keys.ToList();
            foreach (var itemRecipe in recipes)
                {
                    foreach (var itemRecipeArticle in itemRecipe.Value.Item2)
                    {
                        if (uniqueArticles.Contains(itemRecipeArticle.Key))
                        {
                            uniqueArticles.Remove(itemRecipeArticle.Key);
                        }
                    }
                }
            Console.WriteLine($"ARTIKLI KOJI NISU PRODANI: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\tUkupno");
            foreach (var item in uniqueArticles)
            {
                valueUnsell += (articles[item].Item2*articles[item].Item1);
                Console.WriteLine($"{item}\t\t{articles[item].Item1}\t\t{articles[item].Item2}e\t\t" +
                                  $"{articles[item].Item3.ToString("dd.MM.yyyy")}\t\t{articles[item].Item1 * articles[item].Item2}e");
            }
            Console.WriteLine($"UKUPNA VRIJEDNOST: {valueUnsell}e");
            ReturnToMain();
            break;
        case 3:
            Console.Clear();
            valueUnsell = 0;
            var soldIndex = new List<string>();
            var soldArticles2 = articles.ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (var item in soldArticles2)
            {
                soldArticles2[item.Key] = (0, item.Value.Item2, item.Value.Item3);
            }
            foreach (var item in recipes)
            {
                foreach (var itemRecipes in item.Value.Item2)
                {
                    if (soldIndex.Contains(itemRecipes.Key) == false)
                    {
                        soldIndex.Add(itemRecipes.Key);
                    }
                    soldArticles2[itemRecipes.Key] = (soldArticles2[itemRecipes.Key].Item1 + itemRecipes.Value,
                        soldArticles2[itemRecipes.Key].Item2, soldArticles2[itemRecipes.Key].Item3);
                }
            }
            Console.WriteLine($"ARTIKLI KOJI SU PRODANI: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\tUkupno");
            foreach (var item in soldIndex)
            {
                valueUnsell += (soldArticles2[item].Item2*soldArticles2[item].Item1);
                Console.WriteLine($"{item}\t\t{soldArticles2[item].Item1}\t\t{soldArticles2[item].Item2}e\t\t" +
                                  $"{soldArticles2[item].Item3.ToString("dd.MM.yyyy")}\t\t{soldArticles2[item].Item1 * soldArticles2[item].Item2}e");
            }           
            Console.WriteLine($"UKUPNA VRIJEDNOST: {valueUnsell.ToString("00.00")}e");
            ReturnToMain();
            break;
        case 4:
            double income = 25000;//teoretske vrijednosti
            int paycheck = 1000 * workers.ToList().Count;
            int spend = 2000;
            Console.Clear();
            MonthDecide:
            Console.WriteLine("Unesite mjesec gdje želite vidjeti stanje: ");
            string s = Console.ReadLine();
            DateTime monthStats;
            while (DateTime.TryParseExact(s, "MM yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out monthStats) == false)
            {
                Console.Clear();
                Console.WriteLine("Unesite novi datum u obliku MM YYYY: ");
                s = Console.ReadLine();
            }
            if (monthStats.Year < 2022)
            {
                Console.Clear();
                Console.WriteLine("Mali dućan nije postojao u unesenoj godini");
                goto MonthDecide;
            }
            if (monthStats > DateTime.Now)
            {
                Console.Clear();
                Console.WriteLine("Unijeli ste mjesec koji nadolazi");
                goto MonthDecide; 
            }
            foreach (var item in recipes)
            {
                if (item.Value.Item1.ToString("MM,yyyy") == monthStats.ToString("MM,yyyy"))
                {
                    income += TotalValue(item.Key, articles, recipes);
                }
            }
            Console.Clear();
            Console.WriteLine($"PODACI O {monthStats.Month} mjesecu {monthStats.Year}");
            Console.WriteLine($"Prihodi: {income.ToString("00.00")}e\nPlaće: {paycheck}e\n" +
                              $"Troškovi: {spend}e\nUKUPNI PRIHOD: {(income/3 - paycheck - spend).ToString("00.00")}e");
            ReturnToMain();
            break;
        case 0:
            Console.Clear();
            return;
        default:
            Console.Clear();
            Console.WriteLine("Unesite ponuđene brojeve.");
            goto SChoice;
    }
}
}