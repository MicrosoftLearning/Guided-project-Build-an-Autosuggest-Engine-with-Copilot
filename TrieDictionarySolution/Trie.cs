using System;
using System.Collections.Generic;

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public bool IsEndOfWord { get; set; }

    public char _value;

    public TrieNode(char value = ' ')
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        _value = value;
    }

    public bool HasChild(char c)
    {
        return Children.ContainsKey(c);
    }
}

public class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    public bool Search(string word)
    {
        TrieNode current = root;

        foreach (char c in word)
        {
            if (!current.HasChild(c))
            {
                return false;
            }
            current = current.Children[c];
        }

        return current.IsEndOfWord;
    }
    
    public bool Insert(string word)
    {
        // Start at the root node
        TrieNode current = root;

        // For each character in the word
        foreach (char c in word)
        {
            // If the current node doesn't have a child with the current character
            if (!current.HasChild(c))
            {
                // Add a new child with the current character
                current.Children[c] = new TrieNode(c);
            }
            // Move to the child node with the current character
            current = current.Children[c];
        }

        if (current.IsEndOfWord)
        {
            // Word already exists in the trie
            return false;
        }
        
        // Mark the end of the word
        current.IsEndOfWord = true;

        // Word successfully inserted into the trie
        return true;
    }

    public List<string> AutoSuggest(string prefix)
    {
        TrieNode currentNode = root;

        foreach (char c in prefix)
        {
            if (!currentNode.HasChild(c))
            {
                return new List<string>();
            }
            currentNode = currentNode.Children[c];
        }

        return GetAllWordsWithPrefix(currentNode, prefix);
    }

    /// <summary>
    /// Recursively gets all words in the trie that start with the given prefix.
    /// </summary>
    /// <param name="root">The root node of the trie.</param>
    /// <param name="prefix">The prefix to search for.</param>
    /// <returns>A list of all words in the trie that start with the given prefix.</returns>
    private List<string> GetAllWordsWithPrefix(TrieNode root, string prefix)
    {
        List<string> words = new();

        if (root.IsEndOfWord)
        {
            words.Add(prefix);
        }

        foreach (char c in root.Children.Keys)
        {
            words.AddRange(GetAllWordsWithPrefix(root.Children[c], prefix + c));
        }

        return words;
    }

    public List<string> GetAllWords()
    {
        return GetAllWordsWithPrefix(root, "");
    }

    private bool DeleteHelper(TrieNode root, string word, int index)
    {
        if (index == word.Length)
        {
            if (!root.IsEndOfWord)
            {
                return false;
            }
            root.IsEndOfWord = false;
            return root.Children.Count == 0;
        }

        char c = word[index];
        if (!root.HasChild(c))
        {
            return false;
        }

        bool shouldDeleteCurrentNode = DeleteHelper(root.Children[c], word, index + 1);

        if (shouldDeleteCurrentNode)
        {
            root.Children.Remove(c);
            return root.Children.Count == 0;
        }

        return false;
    }

    public bool Delete(string word)
    {
        return DeleteHelper(root, word, 0);
    }

    public List<string> GetSpellingSuggestions(string word)
    {
        char firstLetter = word[0];
        List<string> suggestions = new();
        List<string> words = GetAllWordsWithPrefix(root.Children[firstLetter], firstLetter.ToString());
        
        foreach (string w in words)
        {
            int distance = LevenshteinDistance(word, w);
            if (distance <= 2)
            {
                suggestions.Add(w);
            }
        }

        return suggestions;
    }

    private int LevenshteinDistance(string s, string t)
    {
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m + 1, n + 1];

        if (m == 0)
        {
            return n;
        }

        if (n == 0)
        {
            return m;
        }

        for (int i = 0; i <= m; i++)
        {
            d[i, 0] = i;
        }

        for (int j = 0; j <= n; j++)
        {
            d[0, j] = j;
        }

        for (int j = 1; j <= n; j++)
        {
            for (int i = 1; i <= m; i++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }

        return d[m, n];
    }

    public void PrintTrieStructure()
    {
        Console.WriteLine("\nroot");
        _printTrieNodes(root);
    }

    private void _printTrieNodes(TrieNode root, string format = " ", bool isLastChild = true) 
    {
        if (root == null)
            return;

        Console.Write($"{format}");

        if (isLastChild)
        {
            Console.Write("└─");
            format += "  ";
        }
        else
        {
            Console.Write("├─");
            format += "│ ";
        }

        Console.WriteLine($"{root._value}");

        int childCount = root.Children.Count;
        int i = 0;
        var children = root.Children.OrderBy(x => x.Key);

        foreach(var child in children)
        {
            i++;
            bool isLast = i == childCount;
            _printTrieNodes(child.Value, format, isLast);
        }
    }
}
