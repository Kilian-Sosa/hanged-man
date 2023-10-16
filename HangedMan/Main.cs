

List<string> WORD_LIST = ["OLA", "MANZANA", "PERA", "CASA", "COCHE", "MOTO", "BICICLETA", "ORDENADOR", "MOVIL"];
string word = WORD_LIST[new Random().Next(WORD_LIST.Count)];
const int MAX_TRIES = 6;
int tries = 0;

void Main()
{
    PrintIntro();

    string answer = new('_', word.Length);

    while (tries < MAX_TRIES && answer != word)
    {
        Console.WriteLine(answer);
        Console.WriteLine("Ingrese una letra: ");

        string input = Console.ReadLine();
        if (!IsValid(letter: input, answer)) continue;

        char letter = input[0];
        letter = char.ToUpper(letter);

        if (word.Contains(letter))
        {
            Console.WriteLine($"La palabra contiene la letra '{letter}'");
            ModifyWord(letter, ref answer);
        }
        else
        {
            AddBodyPart(++tries);
            PrintErrorMessage(letter);
        }
        Console.WriteLine();
    }
    if (tries == MAX_TRIES) Console.WriteLine("Has perdido. GL la próxima vez");
    else Console.WriteLine("Has ganado!!!");
}

void PrintIntro()
{
    Console.WriteLine("Bienvenido al juego del ahorcado");
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

bool IsValid(string letter, string answer)
{
    bool isAlreadyUsed = false;
    if (string.IsNullOrEmpty(letter) || letter.Length != 1)
        Console.WriteLine("Debe ingresar UNA letra");
    else if (!char.IsLetter(letter[0]))
        Console.WriteLine("Debe ingresar una letra del abecedario");
    else
    {
        isAlreadyUsed = answer.Contains(letter.ToUpper());
        if (isAlreadyUsed) Console.WriteLine("La letra ya ha sido usada");
    }

    return letter.Length == 1 && char.IsLetter(letter[0]) && !isAlreadyUsed;
}

void ModifyWord(char letter, ref string answer)
{
    for (int i = 0; i < word.Length; i++)
        if (word[i] == letter) answer = answer.Remove(i, 1).Insert(i, letter.ToString());
}

void AddBodyPart(int tries)
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