using System.Text;

string[] words = {
        "as", "astronaut", "asteroid", "are", "around",
        "cat", "cars", "cares", "careful", "carefully",
        "for", "follows", "forgot", "from", "front",
        "mellow", "mean", "money", "monday", "monster",
        "place", "plan", "planet", "planets", "plans",
        "the", "their", "they", "there", "towards"};

Trie dictionary = InitializeTrie(words);
// SearchWord();
// PrefixAutocomplete();
// DeleteWord();
// GetSpellingSuggestions();

Trie InitializeTrie(string[] words)
{
    Trie trie = new Trie();

    foreach (string word in words)
    {
        trie.Insert(word);
    }

    return trie;
}

void SearchWord()
{
    while (true)
    {
        Console.WriteLine("Enter a word to search for, or press Enter to exit.");
        string? input = Console.ReadLine();
        if (input == "")
        {
            break;
        }
        /*
        if (input != null && dictionary.Search(input))
        {
            Console.WriteLine($"Found \"{input}\" in dictionary");
        }
        */
        else
        {
            Console.WriteLine($"Did not find \"{input}\" in dictionary");
        }
    }
}

void PrefixAutocomplete()
{
    PrintTrie(dictionary);
    GetPrefixInput();
}

void DeleteWord() 
{
    PrintTrie(dictionary);
    while(true)
    {
        Console.WriteLine("\nEnter a word to delete, or press Enter to exit.");
        string? input = Console.ReadLine();
        if (input == "")
        {
            break;
        }
        /*
        if (input != null && dictionary.Search(input))
        {
            dictionary.Delete(input);
            Console.WriteLine($"Deleted \"{input}\" from dictionary\n");
            PrintTrie(dictionary);
        }
        */
        else
        {
            Console.WriteLine($"Did not find \"{input}\" in dictionary");
        }
    }
}

void GetSpellingSuggestions() 
{
    PrintTrie(dictionary);
    Console.WriteLine("\nEnter a word to get spelling suggestions for, or press Enter to exit.");
    string? input = Console.ReadLine();
    if (input != null)
    {
        var similarWords = dictionary.GetSpellingSuggestions(input);
        Console.WriteLine($"Spelling suggestions for \"{input}\":");
        if (similarWords.Count == 0)
        {
            Console.WriteLine("No suggestions found.");
        }
        else 
        {
            foreach (var word in similarWords)
            {
                Console.WriteLine(word);
            }
        }
    }
}

#pragma warning disable CS8321
void RunAllExercises()
{
    SearchWord();
    PrefixAutocomplete();
    DeleteWord();
    GetSpellingSuggestions();
}

void GetPrefixInput()
{
    Console.WriteLine("\nEnter a prefix to search for, then press Tab to " + 
                      "cycle through search results. Press Enter to exit.");

    bool running = true;
    string prefix = "";
    StringBuilder sb = new StringBuilder();
    List<string>? words = null;
    int wordsIndex = 0;

    while(running)
    {
        var input = Console.ReadKey(true);

        if (input.Key == ConsoleKey.Spacebar)
        {
            Console.Write(' ');
            prefix = "";
            sb.Append(' ');
            continue;
        } 
        else if (input.Key == ConsoleKey.Backspace && Console.CursorLeft > 0)
        {
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write(' ');
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);

            sb.Remove(sb.Length - 1, 1);
            prefix = sb.ToString().Split(' ').Last();
        }
        else if (input.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            running = false;
            continue;
        }
        else if (input.Key == ConsoleKey.Tab && prefix.Length > 1)
        {
            string previousWord = sb.ToString().Split(' ').Last();

            if (words != null) {
                if (!previousWord.Equals(words[wordsIndex - 1]))
                {
                    words = dictionary.AutoSuggest(prefix);
                    wordsIndex = 0;
                }
            } 
            else {
                words = dictionary.AutoSuggest(prefix);
                wordsIndex = 0;
            }

            for (int i = prefix.Length; i < previousWord.Length; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                Console.Write(' ');
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                sb.Remove(sb.Length - 1, 1);
            }
        
            
            if (words.Count > 0 && wordsIndex < words.Count)
            {
                string output = words[wordsIndex++];
                Console.Write(output.Substring(prefix.Length));
                sb.Append(output.Substring(prefix.Length));
            }
            continue;
        }
        else if (input.Key != ConsoleKey.Tab)
        {
            Console.Write(input.KeyChar);
            prefix += input.KeyChar;
            sb.Append(input.KeyChar);
            words = null;
            wordsIndex = 0;
        }
    }
}

void PrintTrie(Trie trie)
{
    Console.WriteLine("The dictionary contains the following words:");
    List<string> words = trie.GetAllWords();
    foreach (string word in words)
    {
        Console.Write($"{word}, ");
    }
    Console.WriteLine();
}