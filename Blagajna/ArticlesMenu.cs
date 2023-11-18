namespace Blagajna;

public class ArticlesMenu
{
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
    static string writeDate(DateTime expire)
    {
        string s;
        TimeSpan timeUntilExpiration = expire - DateTime.Now;
        int days = (int)Math.Floor(timeUntilExpiration.TotalDays);
        if (days > 0)
        {
            s = $"{days} dana do isteka roka";
        }
        else if (days == 0)
        {
            s = "Proizvod istječe danas.";
        }
        else
        {
            s= $"{Math.Abs(days)} dana od isteka roka";
        }

        return s;
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
    public void Menu(Dictionary<string, (int, float, DateTime)> articles, Dictionary<int, (DateTime, Dictionary<string, int>)> recipes)
    {
    Console.Clear();
    Console.WriteLine("IZBORNIK ARTIKLI: \n 1 - Unos artikla \n 2 - Brisanje artikla" +
                      "\n 3 - Uređivanje artikla \n 4 - Ispis\n 0 - Povratak");
    AChoice:
    int articlesChoice = CheckAnswer(Console.ReadLine());
    switch (articlesChoice)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("Unesite artikal sa sljedećim podacima\nNaziv: ");
            string name = Console.ReadLine();
            Console.WriteLine("Cijena: ");
            float price;
            string s = Console.ReadLine();
            while (float.TryParse(s ,out price) == false)
            {
               Console.Clear();
               Console.WriteLine("Unesite cijenu u obliku broja.");
               s = Console.ReadLine();
            }
            Console.WriteLine("Datum trajanja (YYYY MM DD): ");
            s = Console.ReadLine();
            DateTime expire;
            string format = "yyyy MM dd";
            while (DateTime.TryParseExact(s, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out expire) == false)
            {
                Console.Clear();
                Console.WriteLine("Unesite datum u obliku YYYY MM DD: ");
                s = Console.ReadLine();
            }
            Console.WriteLine("Količina: ");
            s = Console.ReadLine();
            int quantity;
            while (int.TryParse(s ,out quantity) == false)
            {
                Console.Clear();
                Console.WriteLine("Unesite količinu u obliku broja.");
                s = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine($"Unijeli ste informacije o artiklu: {name} {price}e datum trajanja {expire.ToString("dd.MM.yyyy")} , {quantity} komada\n" +
                              $"Potvrdite unos (da/ne): ");
            string save = Console.ReadLine();
            if (ConfirmChange(save))
            {
                articles.Add(name, (quantity, price, expire));
                Console.Clear();
                Console.WriteLine("Uspiješno je pohranjen artikal.");
            }
            else
            {
                Console.Clear();
                goto case 1;
            }
           break;
        case 2:
            Console.Clear();
            Console.WriteLine("1 - Pretražite artikal koji želite izbrisati\n" +
                              "2 - Izbrišite artikla kojima je istjekao rok trajanja");
            Choice2:
            s = Console.ReadLine();
            if (CheckAnswer(s) == 1)
            {
                Console.Clear();
                Console.WriteLine("Unesite ime artikla koji želite izbrisati: ");
                Delete:
                string deleteName = Console.ReadLine();
                
                if (CheckArticle(deleteName, articles))
                {
                    Console.WriteLine($"Želite li izbrsati proizvod: {articles[deleteName]} (da/ne)");
                    save = Console.ReadLine();
                    if (ConfirmChange(save))
                    {
                        articles.Remove(deleteName);
                        Console.Clear();
                        Console.WriteLine("Uspiješno je uklonjen artikal.");
                    }
                    else
                    {
                        Console.Clear();
                        goto case 2;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Nepostoji proizvod koji želite izbrisati, unesite ponovo: ");
                    goto Delete;
                }
            }
            else if(CheckAnswer(s) == 2)
            {
                Console.Clear();
                int temp = 0;
                Console.WriteLine("Artikli kojima je prošao rok su: ");
                foreach (var item in articles)
                {
                    if (item.Value.Item3 < DateTime.Now)
                    {
                        temp++;
                        Console.WriteLine($"{item.Key}, količina: {articles[item.Key].Item1}, cijena: {articles[item.Key].Item2}" +
                                          $"rok trajanja: {articles[item.Key].Item3.ToString("dd.MM.yyyy")}");
                    }
                }
                if (temp == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Nema artikala kojima je prošao rok.");
                }
                else
                {
                    Console.WriteLine("Potvrdite brisanje (da/ne)");
                    string deleteAll = Console.ReadLine();
                    if (ConfirmChange(deleteAll))
                    {
                        foreach (var item in articles)
                        {
                            if (item.Value.Item3 < DateTime.Now)
                            {
                                articles.Remove(item.Key);
                            }
                        }
                        Console.Clear();
                        Console.WriteLine("Uspiješno su uklonjeni artikli.");
                    }
                    else
                    {
                        Console.Clear();
                        goto case 2;
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Unesi ponuđene brojeve: ");
                goto Choice2;
            }
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("1 - Uređivanje artikla\n" +
                              "2 - popust/poskupljenje");
            Choice3:
            s = Console.ReadLine();
            if (CheckAnswer(s) == 1)
            {
                Console.Clear();
                Console.WriteLine("Pretražite proizvod koji želite promijeniti");
                Change:
                string changeArticle = Console.ReadLine();
                if (CheckArticle(changeArticle, articles))
                {
                    Console.Clear();
                    Console.WriteLine("Što želite promijeniti: \n 1 - Ime\n 2 - Količina\n 3- Cijena\n" +
                                      " 4 - Vrijeme trajanja");
                    ChangeSelect:
                    int changeSelect = CheckAnswer(Console.ReadLine());
                    switch (changeSelect)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("Unesite nov naziv artikla: ");
                            string changedName = Console.ReadLine();
                            Console.Clear();
                            Console.WriteLine($"Potvrdite uređivanje imena atrikla iz {changeArticle} u {changedName} (da/ne)");
                            string confirm = Console.ReadLine();
                            if (ConfirmChange(confirm))
                            {
                                articles.Add(changedName, (articles[changeArticle].Item1, articles[changeArticle].Item2, articles[changeArticle].Item3));
                                articles.Remove(changeArticle);
                                Console.Clear();
                                Console.WriteLine("Promjena potvrđena.");
                            }
                            else
                            {
                                goto case 1;
                            }
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("Unesite novu količinu: : ");
                            s = Console.ReadLine();
                            int changedQuantity;
                            while (int.TryParse(s ,out changedQuantity) == false)
                            {
                                Console.Clear();
                                Console.WriteLine("Unesite novu količinu u obliku broja.");
                                s = Console.ReadLine();
                            }
                            Console.Clear();
                            Console.WriteLine($"Potvrdite uređivanje količina atrikla iz {articles[changeArticle].Item1} u {changedQuantity} (da/ne)");
                            confirm = Console.ReadLine();
                            if (ConfirmChange(confirm))
                            {
                                articles[changeArticle] = (changedQuantity, articles[changeArticle].Item2, articles[changeArticle].Item3);
                                Console.Clear();
                                Console.WriteLine("Promjena potvrđena.");
                            }
                            else
                            {
                                goto case 2;
                            }
                            break;
                        case 3:
                            Console.WriteLine("Unesite novu cijenu artikla: ");
                            s = Console.ReadLine();
                            float changedPrice;
                            while (float.TryParse(s ,out changedPrice) == false)
                            {
                                Console.Clear();
                                Console.WriteLine("Unesite novu cijenu u obliku broja.");
                                s = Console.ReadLine();
                            }                            
                            Console.WriteLine($"Potvrdite uređivanje cijene atrikla iz {articles[changeArticle].Item2} u {changedPrice} (da/ne)");
                            confirm = Console.ReadLine();
                            if (ConfirmChange(confirm))
                            {
                                articles[changeArticle] = (articles[changeArticle].Item1, changedPrice, articles[changeArticle].Item3);
                                Console.Clear();
                                Console.WriteLine("Promjena potvrđena.");
                            }
                            else
                            {
                                goto case 3;
                            }
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("Unesite novo vrijeme trajanja: ");
                            s = Console.ReadLine();
                            DateTime chagedExpire;
                            format = "yyyy MM dd";
                            while (DateTime.TryParseExact(s, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out chagedExpire) == false)
                            {
                                Console.Clear();
                                Console.WriteLine("Unesite novi datum u obliku YYYY MM DD: ");
                                s = Console.ReadLine();
                            }    
                            Console.Clear();
                            Console.WriteLine($"Potvrdite uređivanje vrijeme trajanja atrikla iz {articles[changeArticle].Item3} u {chagedExpire} (da/ne)");
                            confirm = Console.ReadLine();
                            if (ConfirmChange(confirm))
                            {
                                articles[changeArticle] = (articles[changeArticle].Item1, articles[changeArticle].Item1, chagedExpire);
                                Console.Clear();
                                Console.WriteLine("Promjena potvrđena.");
                            }
                            else
                            {
                                goto case 4;
                            }
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Unesi ponuđene brojeve");
                            goto ChangeSelect;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Nepostoji proizvod koji želite urediti, unesite ponovo: ");
                    goto Change;
                }
            }
            else if (CheckAnswer(s) == 2)
            {
                Console.Clear();
                Console.WriteLine("Unesite popust ili poskupljenje u postotcima:");
                float percent = CheckAnswer(Console.ReadLine());
                foreach (var item in articles)
                {
                    articles[item.Key] = (item.Value.Item1, item.Value.Item2 * percent / 100, item.Value.Item3);
                }
                Console.Clear();
                Console.WriteLine($"Uspiješno je promijenjena cijena {percent} posto od početne cijene");
            }
            else
            { 
                Console.Clear();
                Console.WriteLine("Unesi ponuđene brojeve: ");
                goto Choice3;
            }
            break;
        case 4:
            Console.Clear();
            Console.WriteLine("1 - Ispiši artikle\n2 - Ispiši artikle po imenu\n3 - Ispiši artikle po datumu uzlazno\n4 - Ispiši artikle po datumu silazno\n5 - Ispiši artikle po količini\n6 - Najprodaviji artikl\n7 - Najmanje prodavan artikl");
            choiceWrite:
            int choiceList = CheckAnswer(Console.ReadLine());
            switch (choiceList)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine($"SVI ARTIKLI: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\t\tUkupno");
                    foreach (var item in articles)
                    {
                        Console.WriteLine($"{item.Key}\t\t{item.Value.Item1}\t\t{item.Value.Item2}e\t\t" +
                                          $"{writeDate(item.Value.Item3)}\t\t" +
                                          $"{item.Value.Item1 * item.Value.Item2}e");
                    }
                    ReturnToMain();
                    return;
                    break;
                case 2:
                    Console.Clear();
                    var sortedArticles = articles.Keys.ToList();
                    sortedArticles.Sort();
                    Console.WriteLine($"SVI ARTIKLI PO IMENU: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\t\tUkupno");
                    foreach (var item in sortedArticles)
                    {

                        Console.WriteLine($"{item}\t\t{articles[item].Item1}\t\t{articles[item].Item2}e\t\t" +
                                          $"{writeDate(articles[item].Item3)}\t\t" +
                                          $"{articles[item].Item1 * articles[item].Item2}e");
                    }
                    ReturnToMain();
                    return;
                    break;
                case 3:                    
                    Console.Clear();
                    Console.WriteLine($"SVI ARTIKLI PO DATUMU: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\t\tUkupno");
                    var sortedArticles3 = articles.OrderBy(item => item.Value.Item3).ToList();
                    foreach (var item in sortedArticles3)
                    {
                        Console.WriteLine($"{item.Key}\t\t{articles[item.Key].Item1}\t\t{articles[item.Key].Item2}e\t\t" +
                                          $"{writeDate(articles[item.Key].Item3)}\t\t" +
                                          $"{articles[item.Key].Item1 * articles[item.Key].Item2}e");
                    }
                    ReturnToMain();
                    return;
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine($"SVI ARTIKLI PO DATUMU SILAZNO: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\t\tUkupno");
                    var sortedArticles4 = articles.OrderByDescending(item => item.Value.Item3).ToList();
                    foreach (var item in sortedArticles4)
                    {
                        Console.WriteLine($"{item.Key}\t\t{articles[item.Key].Item1}\t\t{articles[item.Key].Item2}e\t\t" +
                                          $"{writeDate(articles[item.Key].Item3)}\t\t" +
                                          $"{articles[item.Key].Item1 * articles[item.Key].Item2}e");
                    }
                    ReturnToMain();
                    return;
                    break;
                case 5:                    
                    Console.Clear();
                    Console.WriteLine($"SVI ARTIKLI PO KOLIČINI: \nNaziv\t\tKoličina\tCijena\t\tRok trajanja\t\t\tUkupno");
                    var sortedArticles2 = articles.OrderBy(item => item.Value.Item3).ToList();
                    foreach (var item in sortedArticles2)
                    { 
                        Console.WriteLine($"{item.Key}\t\t{articles[item.Key].Item1}\t\t{articles[item.Key].Item2}e\t\t" +
                                        $"{writeDate(articles[item.Key].Item3)}\t\t" +
                                        $"{articles[item.Key].Item1 * articles[item.Key].Item2}e");
                    }
                    ReturnToMain();
                    return;
                    break;
                case 6:
                    Console.Clear();
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
                        Console.WriteLine($"{item}\t\t{soldArticles2[item].Item1}\t\t{soldArticles2[item].Item2}e\t\t" +
                                          $"{soldArticles2[item].Item3.ToString("dd.MM.yyyy")}\t\t{soldArticles2[item].Item1 * soldArticles2[item].Item2}e");
                    }

                    Console.WriteLine($"NAJPRODAVANIJI ARTIKAL: {soldArticles2.OrderByDescending(x => x.Value.Item1).First().Key}");
                    ReturnToMain();
                    break;
                case 7:
                    Console.Clear();
                    soldIndex = new List<string>();
                    soldArticles2 = articles.ToDictionary(entry => entry.Key, entry => entry.Value);
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
                        Console.WriteLine($"{item}\t\t{soldArticles2[item].Item1}\t\t{soldArticles2[item].Item2}e\t\t" +
                                          $"{soldArticles2[item].Item3.ToString("dd.MM.yyyy")}\t\t{soldArticles2[item].Item1 * soldArticles2[item].Item2}e");
                    }

                    Console.WriteLine($"NAJMANJE PRODAVAN ARTIKAL: {soldArticles2.Where(x => x.Value.Item1 != 0).OrderBy(x => x.Value.Item1).First().Key}");
                    ReturnToMain();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Unesite ponuđene brojeve.");
                    goto choiceWrite;
                    break;
            }
            break;
        case 0:
            Console.Clear();
            return;
        default:
            Console.Clear();
            Console.WriteLine("Unesite ponuđene brojeve.");
            goto AChoice;
    }
}
}