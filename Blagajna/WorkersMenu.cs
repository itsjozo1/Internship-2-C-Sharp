namespace Blagajna;

public class WorkersMenu
{
    static void returnToMain(){
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
    static bool CheckWorker(string ar, Dictionary<string, DateTime> workers)
    {
        foreach (var item in workers)
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
    public void Menu(Dictionary<string, DateTime> workers)
    {
    Console.Clear();
    Console.WriteLine("1 - Unos radnika\n2 - Brisanje radnika\n3 - Uređivanje radnika\n4- Ispis\n0 - Povratak");
    WChoice:
    int workersChoice = CheckAnswer(Console.ReadLine());
    switch (workersChoice)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("Unesite radnika sa sljedećim podacima\nIme i Prezime: ");
            string name = Console.ReadLine();
            Console.WriteLine("Datum rođenja (YYYY MM DD): ");
            string s = Console.ReadLine();
            DateTime birth;
            string format = "yyyy MM dd";
            while (DateTime.TryParseExact(s, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out birth) == false)
            {
                Console.Clear();
                Console.WriteLine("Unesite datum u obliku YYYY MM DD: ");
                s = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine($"Unijeli ste informacije o radniku: {name} datum trajanja {birth.ToString("yyyy,MM,dd")}\n" +
                              $"Potvrdite unos (da/ne): ");
            string save = Console.ReadLine();
            if (ConfirmChange(save))
            {
                workers.Add(name, birth);
                Console.Clear();
                Console.WriteLine("Uspiješno je pohranjen radnik.");
                returnToMain();
            }
            else
            {
                Console.Clear();
                goto case 1;
            }
            break;
        case 2:
            Console.Clear();
            Console.WriteLine("1 - Pretražite radnika koji želite izbrisati\n" +
                              "2 - Izbrišite radnike koji imaju preko 65 godina");
            Choice2:
            s = Console.ReadLine();
            if (CheckAnswer(s) == 1)
            {
                Console.Clear();
                Console.WriteLine("Unesite ime radnika koji želite izbrisati: ");
                Delete:
                string deleteName = Console.ReadLine();
                
                if (CheckWorker(deleteName, workers))
                {
                    Console.WriteLine($"Želite li izbrsati radnika: {deleteName} {workers[deleteName].Date.ToString("yyyy,MM,dd")} (da/ne)");
                    save = Console.ReadLine();
                    if (ConfirmChange(save))
                    {
                        workers.Remove(deleteName);
                        Console.Clear();
                        Console.WriteLine("Uspiješno je uklonjen radnik.");
                        returnToMain();
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
                    Console.WriteLine("Nepostoji radnik koji želite izbrisati, unesite ponovo: ");
                    goto Delete;
                }
            }
            else if(CheckAnswer(s) == 2)
            {
                Console.Clear();
                int temp = 0;
                Console.WriteLine("Radnici preko 65 godina su: ");
                foreach (var item in workers)
                {
                    if (item.Value.Year < (DateTime.Now.Year-65))
                    {
                        temp++;
                        Console.WriteLine($"Ime: {item.Key} Datum rođenja: {item.Value.ToString("yyyy,MM,dd")}");
                    }
                }
                if (temp == 0)
                {
                    Console.Clear();
                    Console.WriteLine("Nema radnika starijih od 65 godina.");
                    returnToMain();
                }
                else
                {
                    Console.WriteLine("Potvrdite brisanje (da/ne)");
                    string deleteAll = Console.ReadLine();
                    if (ConfirmChange(deleteAll))
                    {
                        foreach (var item in workers)
                        {
                            if (item.Value.Year < (DateTime.Now.Year-65))
                            {
                                workers.Remove(item.Key);
                            }
                        }
                        Console.Clear();
                        Console.WriteLine("Uspiješno su uklonjeni radnici.");
                        returnToMain();
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
            Console.WriteLine("Unesite radnka koji želite promijeniti: ");
            Change:
            string changeWorker = Console.ReadLine();
            if (CheckWorker(changeWorker, workers))
            {
                Console.Clear();
                Console.WriteLine("Što želite promijeniti: \n1 - Ime i prezime\n2 - Datum rođenja");
                ChangeSelect:
                int changeSelect = CheckAnswer(Console.ReadLine());
                switch (changeSelect)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Unesite novo ime i prezime radnika: ");
                        string changedName = Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine($"Potvrdite uređivanje imena i prezimena radnika iz {changeWorker} u {changedName} (da/ne)");
                        string confirm = Console.ReadLine();
                        if (ConfirmChange(confirm))
                        {
                            workers.Add(changedName, workers[changeWorker].Date);
                            workers.Remove(changeWorker);
                            Console.Clear();
                            Console.WriteLine("Promjena potvrđena.");
                            returnToMain();
                        }
                        else
                        {
                            goto case 1;
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Unesite novi datum rođenja: ");
                        s = Console.ReadLine();
                        DateTime chagedBirth;
                        format = "yyyy MM dd";
                        while (DateTime.TryParseExact(s, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out chagedBirth) == false)
                        {
                            Console.Clear();
                            Console.WriteLine("Unesite novi datum u obliku YYYY MM DD: ");
                            s = Console.ReadLine();
                        }    
                        Console.Clear();
                        Console.WriteLine($"Potvrdite uređivanje datuma rođenja iz {workers[changeWorker].Date.ToString("yyyy,MM,dd")} u {chagedBirth.ToString("yyyy,MM,dd")} (da/ne)");
                        confirm = Console.ReadLine();
                        if (ConfirmChange(confirm))
                        {
                            workers[changeWorker] = chagedBirth;
                            Console.Clear();
                            Console.WriteLine("Promjena potvrđena.");
                            returnToMain();
                        }
                        else
                        {
                            goto case 2;
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Unesite ponuđene brojeve");
                        goto ChangeSelect;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Nepostoji radnik koji želite urediti, unesite ponovo: ");
                goto Change;
            }
            break;
        case 4:
            
            Console.Clear();
            Console.WriteLine("1 - Ispiši sve radnike\n2 - Radnike kojima je rođendan u tekućem mjesecu");
            ChoiceWrite:
            int write = CheckAnswer(Console.ReadLine());
            switch (write)
            {
                case 1:
                    Console.Clear();
                    foreach (var item in workers)
                    {
                        Console.WriteLine($"Radnik: {item.Key}, datum rođenja: {item.Value.ToString("yyyy,MM,dd")}");
                    }
                    returnToMain();
                    break;
                case 2:
                    int temp = 0;
                    Console.Clear();
                    foreach (var item in workers)
                    {
                        if (item.Value.Month == DateTime.Now.Month)
                        {
                            temp++;
                        }
                    }
                    if (temp == 0)
                    {
                        returnToMain();
                    }
                    else
                    {
                        Console.WriteLine("Radnici s rođendanom u tekućem mjesecu su: ");
                        foreach (var item in workers)
                        {
                            if (item.Value.Month == DateTime.Now.Month)
                            {
                                Console.WriteLine($"Radnik: {item.Key}, datum rođenja: {item.Value.ToString("yyyy,MM,dd")}");
                            }
                        }
                        returnToMain();
                    }
                    break;
                default:
                    Console.WriteLine("Unesite ponuđene brojeve: ");
                    goto ChoiceWrite;
            }
            break;
        case 0:
            Console.Clear();
            return;
        default:
            Console.Clear();
            Console.WriteLine("Unesite ponuđene brojeve: ");
            goto WChoice;
        }
    }

}