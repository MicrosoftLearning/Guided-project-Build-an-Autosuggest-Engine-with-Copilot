namespace TrieDictionaryTest;

[TestClass]
public class TrieTest
{
    // Test that a word is inserted in the trie
    [TestMethod]
    public void TestInsert()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        Assert.IsTrue(trie.Search("hello"));
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void TestDelete()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Delete("hello");
        Assert.IsFalse(trie.Search("hello"));
    }

    // Test that a word is not inserted twice in the trie
    [TestMethod]
    public void TestInsertDuplicate()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Insert("hello");
        Assert.IsTrue(trie.Search("hello"));
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void TestDeleteNonExistent()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Delete("world");
        Assert.IsTrue(trie.Search("hello"));
    }

    // Test that a word is not deleted from the trie if it is not present
    [TestMethod]
    public void TestDeletePrefix()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Insert("hell");
        trie.Delete("hell");
        Assert.IsTrue(trie.Search("hello"));
        Assert.IsFalse(trie.Search("hell"));
    }

    // Test that a word is deleted from the trie if it is a prefix of another word
    [TestMethod]
    public void TestDeleteWordWithPrefix()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Insert("hell");
        trie.Delete("hello");
        Assert.IsFalse(trie.Search("hello"));
        Assert.IsTrue(trie.Search("hell"));
    }

    // Test AutoSuggest for the prefix "cat" not present in the 
    // trie containing "catastrophe", "catatonic", and "caterpillar"
    [TestMethod]
    public void TestAutoSuggest_PrefixNotPresent()
    {
        Trie trie = new Trie();
        trie.Insert("catastrophe");
        trie.Insert("catatonic");
        trie.Insert("caterpillar");

        List<string> suggestions = trie.AutoSuggest("cat");
        CollectionAssert.AreEquivalent(
            new List<string> { "catastrophe", "catatonic", "caterpillar" },
            suggestions);
    }

    // Test GetSpellingSuggestions for a word not present in the trie
    [TestMethod]
    public void TestGetSpellingSuggestions_WordNotPresent()
    {
        Trie trie = new Trie();
        trie.Insert("catastrophe");
        trie.Insert("catatonic");
        trie.Insert("caterpillar");

        List<string> suggestions = trie.GetSpellingSuggestions("caterpilar");
        CollectionAssert.AreEquivalent(
            new List<string> { "caterpillar" },
            suggestions);
    }
    
    // Test GetSpellingSuggestions for a word present in the trie
    [TestMethod]
    public void TestGetSpellingSuggestions_WordPresent()
    {
        Trie trie = new Trie();
        trie.Insert("catastrophe");
        trie.Insert("catatonic");
        trie.Insert("caterpillar");

        List<string> suggestions = trie.GetSpellingSuggestions("catatonic");
        CollectionAssert.AreEquivalent(
            new List<string> { "catatonic" },
            suggestions);
    }
}