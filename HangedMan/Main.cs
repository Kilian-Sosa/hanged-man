using System;

List<string> WORD_LIST = ["OLA", "MANZANA", "PERA", "CASA", "COCHE", "MOTO", "BICICLETA", "ORDENADOR", "MOVIL"];
string word = WORD_LIST[new Random().Next(WORD_LIST.Count)];
int MAX_TRIES = 6;
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
        if (!IsValid(letter: input)) continue;

        char letter = input[0];
        letter = char.ToUpper(letter);

        if (word.Contains(letter))
        {
            Console.WriteLine($"La palabra contiene la letra '{letter}'");
            ModifyWord(letter, ref answer);
        }
        else
        {
            Console.WriteLine($"La palabra no contiene la letra '{letter}'");
            AddBodyPart(++tries);
            Console.WriteLine();
            Console.WriteLine($"Te quedan {MAX_TRIES - tries} intentos");
            Console.WriteLine();
        }
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

bool IsValid(string letter)
{
    if (letter.Length != 1)
    {
        Console.WriteLine("Debe ingresar una sola letra");
        return false;
    }
    if (!char.IsLetter(letter[0]))
    {
        Console.WriteLine("Debe ingresar una letra del abecedario");
        return false;
    }
    return true;
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