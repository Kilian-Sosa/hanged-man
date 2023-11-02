using System.Net;

List<string> WORD_LIST = ["OLA", "MANZANA", "PERA", "CASA", "COCHE", "MOTO", "BICICLETA", "ORDENADOR", "MOVIL", "MESA"];
string word = string.Empty;
const int MAX_TRIES = 6;
int tries = 0;

void Main() {
    Console.WriteLine("Welcome to the HangMan game");

    SelectMode();

    word = string.IsNullOrEmpty(word) ? WORD_LIST[new Random().Next(WORD_LIST.Count)].Trim() : word;
    word = word.ToUpper();

    PrintIntro();
    ShowDraw(tries);
    string answer = new('_', word.Length);
    List<string> history = new();

    while (tries < MAX_TRIES && answer != word) {
        Console.WriteLine(answer);
        Console.WriteLine("Write a letter: ");

        string input = Console.ReadLine() ?? "";
        if (!IsValidWord(letter: input)) continue;

        char letter = input[0];
        letter = char.ToUpper(letter);

        bool isAlreadyUsed = history.Contains(letter.ToString());
        if (isAlreadyUsed) { Console.WriteLine("That letter has already been used\n"); continue; }

        history.Add(letter.ToString());

        Console.Clear();
        if (word.Contains(letter)) {
            ShowDraw(tries);
            Console.WriteLine($"The word contains the letter '{letter}'");
            ModifyWord(letter, ref answer);
        }else {
            ShowDraw(++tries);
            PrintErrorMessage(letter);
        }
        Console.WriteLine();
        Console.WriteLine($"Letters used: {string.Join(", ", history)}\n");
    }
    if (tries == MAX_TRIES) Console.WriteLine($"You lost. GL next time");
    else Console.WriteLine("You won!!!");
    Console.WriteLine($"The word was '{word}'");
}

string GetWord() {
    try {
        HttpClient httpClient = new HttpClient();
        //string apiUrl = "https://palabras-aleatorias-public-api.herokuapp.com/random"; // API Spanish words
        string apiUrl = "https://random-word-api.herokuapp.com/word";  // API English words
        HttpResponseMessage response = httpClient.GetAsync(apiUrl).Result;
        Stream stream = response.Content.ReadAsStreamAsync().Result;

        using (StreamReader reader = new StreamReader(stream)) {
            // Read the JSON response
            string jsonResponse = reader.ReadToEnd();

            // Find the word within the JSON response
            int startIndex = jsonResponse.IndexOf("\"") + 1; // Find the first double quote
            int endIndex = jsonResponse.LastIndexOf("\""); // Find the last double quote

            if (startIndex >= 0 && endIndex >= 0) {
                string word = jsonResponse.Substring(startIndex, endIndex - startIndex);
                return word;
            } else {
                Console.WriteLine("There was a problem getting the word");
                return string.Empty;
            }

        }
    }
    catch (WebException ex) {
        Console.WriteLine("Error. The local DB will be used instead");
        if (ex.Response is HttpWebResponse httpWebResponse) {
            if (httpWebResponse.StatusCode == HttpStatusCode.ServiceUnavailable) {
                // Handle the 503 Service Unavailable error here
                // You can log the error or take appropriate action
                // For example, you can retry the request after a delay
                Console.WriteLine("503 Service Unavailabe: " + ex.Message);
            }
        } else {
            // Handle other WebException cases
            Console.WriteLine("WebException: " + ex.Message);
        }
    } catch (Exception ex) {
        // Handle other exceptions
        Console.WriteLine("Exception: " + ex.Message);
    }
    return string.Empty;
}

void SelectMode() {
    string mode = string.Empty;
    while (mode != "1" && mode != "2") {
        PrintAskMode();
        mode = Console.ReadLine() ?? "";
        Console.WriteLine();
    }

    if (mode == "2") {
        WORD_LIST = new List<string>();
        while (WORD_LIST.Count == 0) {
            Console.WriteLine("Player 1, write one or multiple words");
            Console.WriteLine("separated by comma:");

            string input = Console.ReadLine() ?? "";

            bool isValidList = string.IsNullOrEmpty(input) || input.Length == 0;
            if (isValidList) { Console.WriteLine("You need to write at least one word\n"); continue; }

            WORD_LIST = input.ToUpper().Split(',').ToList();
        }
    }else word = GetWord();
}

void PrintAskMode() {
    Console.WriteLine("Select game mode");
    Console.WriteLine("1. One player");
    Console.WriteLine("2. Two players");
}

void PrintIntro() {
    Console.Clear();
    Console.WriteLine($"You have {MAX_TRIES} tries to guess the word");
    Console.WriteLine($"The word has {word.Length} letters");
    Console.WriteLine("Good Luck!");
}
void PrintErrorMessage(char letter) {
    Console.WriteLine($"The word does not contain the letter '{letter}'");
    Console.WriteLine();
    Console.WriteLine($"You have {MAX_TRIES - tries} tries left");
}

bool IsValidWord(string letter) {
    if (string.IsNullOrEmpty(letter) || letter.Length != 1)
        Console.WriteLine("You need to write ONE letter\n");
    else if (!char.IsLetter(letter[0]))
        Console.WriteLine("You need to write a letter of the alphabet");
    return letter.Length == 1 && char.IsLetter(letter[0]);
}

void ModifyWord(char letter, ref string answer) {
    for (int i = 0; i < word.Length; i++)
        if (word[i] == letter) answer = answer.Remove(i, 1).Insert(i, letter.ToString());
}

void ShowDraw(int tries) {
    Console.WriteLine("  _______");
    Console.WriteLine("  |     |");

    switch (tries){
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