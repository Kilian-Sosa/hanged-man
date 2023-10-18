using System.Net;

List<string> WORD_LIST = ["OLA", "MANZANA", "PERA", "CASA", "COCHE", "MOTO", "BICICLETA", "ORDENADOR", "MOVIL", "MESA"];
string word = string.Empty;
const int MAX_TRIES = 6;
int tries = 0;

void Main()
{
    Console.WriteLine("Bienvenido al juego del ahorcado");

    SelectMode();

    word = GetWord();
    word = string.IsNullOrEmpty(word) ? WORD_LIST[new Random().Next(WORD_LIST.Count)].Trim() : word;
    word = word.ToUpper();

    PrintIntro();

    string answer = new('_', word.Length);
    List<string> history = new();

    while (tries < MAX_TRIES && answer != word)
    {
        Console.WriteLine(answer);
        Console.WriteLine("Ingrese una letra: ");

        string input = Console.ReadLine();
        if (!IsValidWord(letter: input)) continue;

        char letter = input[0];
        letter = char.ToUpper(letter);

        bool isAlreadyUsed = history.Contains(letter.ToString());
        if (isAlreadyUsed) { Console.WriteLine("La letra ya ha sido usada\n"); continue; }

        history.Add(letter.ToString());

        Console.Clear();
        if (word.Contains(letter))
        {
            ShowDraw(tries);
            Console.WriteLine($"La palabra contiene la letra '{letter}'");
            ModifyWord(letter, ref answer);
        }
        else
        {
            ShowDraw(++tries);
            PrintErrorMessage(letter);
        }
        Console.WriteLine();
        Console.WriteLine($"Letras usadas: {string.Join(", ", history)}\n");
    }
    if (tries == MAX_TRIES) Console.WriteLine($"Has perdido, la palabra era {word}. GL la próxima vez");
    else Console.WriteLine("Has ganado!!!");
}

string GetWord()
{
    try
    {
        //HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://palabras-aleatorias-public-api.herokuapp.com/random"); // API Spanish words
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://random-word-api.herokuapp.com/word");  // API English words
        using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
        using (Stream stream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(stream))
        {
            // Read the JSON response
            string jsonResponse = reader.ReadToEnd();

            // Find the word within the JSON response
            int startIndex = jsonResponse.IndexOf("\"") + 1; // Find the first double quote
            int endIndex = jsonResponse.LastIndexOf("\""); // Find the last double quote

            if (startIndex >= 0 && endIndex >= 0)
            {
                string word = jsonResponse.Substring(startIndex, endIndex - startIndex);
                return word;
            }
            else
            {
                Console.WriteLine("Hubo un problema al recoger la palabra");
                return string.Empty;
            }
            
        }
    }
    catch (WebException ex)
    {
        Console.WriteLine("Error. Se utilizará la base de datos interna");
        if (ex.Response is HttpWebResponse httpWebResponse)
        {
            if (httpWebResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                // Handle the 503 Service Unavailable error here
                // You can log the error or take appropriate action
                // For example, you can retry the request after a delay
                Console.WriteLine("503 Servidor inaccesible: " + ex.Message);
            }
        }
        else
        {
            // Handle other WebException cases
            Console.WriteLine("WebException: " + ex.Message);
        }
    }
    catch (Exception ex)
    {
        // Handle other exceptions
        Console.WriteLine("Exception: " + ex.Message);
    }
    return string.Empty;
}

void SelectMode()
{
    string mode = string.Empty;
    while (mode != "1" && mode != "2")
    {
        PrintAskMode();
        mode = Console.ReadLine();
        Console.WriteLine();
    }

    if (mode == "2")
    {
        WORD_LIST = new List<string>();
        while (WORD_LIST.Count == 0)
        {
            Console.WriteLine("Jugador 1, introduce una o varias palabra/s");
            Console.WriteLine("separadas por coma:");

            string input = Console.ReadLine();

            bool isValidList = string.IsNullOrEmpty(input) || input.Length == 0;
            if (isValidList) { Console.WriteLine("Debe ingresar al menos una palabra\n"); continue; }

            WORD_LIST = input.ToUpper().Split(',').ToList();
        }
    }
}

void PrintAskMode()
{
    Console.WriteLine("Seleccione el modo de juego");
    Console.WriteLine("1. Un jugador");
    Console.WriteLine("2. Dos jugadores");
}

void PrintIntro()
{
    Console.Clear();
    Console.WriteLine($"Tienes {MAX_TRIES} intentos para adivinar la palabra");
    Console.WriteLine($"La palabra tiene {word.Length} letras");
    Console.WriteLine("Buena suerte!");
}
void PrintErrorMessage(char letter)
{
    Console.WriteLine($"La palabra no contiene la letra '{letter}'");
    Console.WriteLine();
    Console.WriteLine($"Te quedan {MAX_TRIES - tries} intentos");
}

bool IsValidWord(string letter)
{
    if (string.IsNullOrEmpty(letter) || letter.Length != 1)
        Console.WriteLine("Debe ingresar UNA letra\n");
    else if (!char.IsLetter(letter[0]))
        Console.WriteLine("Debe ingresar una letra del abecedario");
    return letter.Length == 1 && char.IsLetter(letter[0]);
}

void ModifyWord(char letter, ref string answer)
{
    for (int i = 0; i < word.Length; i++)
        if (word[i] == letter) answer = answer.Remove(i, 1).Insert(i, letter.ToString());
}

void ShowDraw(int tries)
{
    Console.WriteLine("  _______");
    Console.WriteLine("  |     |");

    switch (tries)
    {
        case 1:
            Console.WriteLine("  O");
            break;
        case 2:
            Console.WriteLine("  O");
            Console.WriteLine("  |");
            break;
        case 3:
            Console.WriteLine("  O");
            Console.WriteLine(" /|");
            break;
        case 4:
            Console.WriteLine("  O");
            Console.WriteLine(" /|\\");
            break;
        case 5:
            Console.WriteLine("  O");
            Console.WriteLine(" /|\\");
            Console.WriteLine(" /");
            break;
        case 6:
            Console.WriteLine("  O");
            Console.WriteLine(" /|\\");
            Console.WriteLine(" / \\");
            break;
    }

}

Main();