using System;
using System.Collections.Generic;
using System.Linq;

namespace Prefix_and_Suffix_Search
{
  class Program
  {
    static void Main(string[] args)
    {
      var words = new string[] { "cabaabaaaa", "ccbcababac", "bacaabccba", "bcbbcbacaa", "abcaccbcaa", "accabaccaa", "cabcbbbcca", "ababccabcb", "caccbbcbab", "bccbacbcba" };
      WordFilter wordFilter = new WordFilter(words);
      var answer = wordFilter.F("ab", "abcaccbcaa");

      Console.WriteLine(string.Join(",", answer));
    }
  }

  public class WordFilter
  {
    // for the original words
    Trie prefix;
    // for the reversed words
    Trie suffix;
    public WordFilter(string[] words)
    {
      prefix = new Trie();
      suffix = new Trie();

      // for each word we would be creating the Trie for the word and also for the reversed word
      for (int i = 0; i < words.Length; i++)
      {
        prefix.Insert(words[i], i);
        string reversed = new string(words[i].Reverse().ToArray());
        suffix.Insert(reversed, i);
      }
    }

    public int F(string pref, string suff)
    {
      var prefixIndexes = prefix.StartsWith(pref);
      string reversed = new string(suff.Reverse().ToArray());
      var suffixIndexes = suffix.StartsWith(reversed);

      var commonIndexes = prefixIndexes.Intersect(suffixIndexes).ToList();

      // in case no search found for the pref and suff return -1
      return (commonIndexes == null || !commonIndexes.Any()) ? -1 : commonIndexes.Max();
    }
  }

  public class Trie
  {
    private Trie[] children;
    private List<int> indexes;

    public Trie()
    {
      children = new Trie[26];
      indexes = new List<int>();
    }

    public void Insert(string word, int index)
    {
      Trie root = this;
      foreach (char c in word)
      {
        if (root.children[c - 'a'] == null)
        {
          root.children[c - 'a'] = new Trie();
        }
        root = root.children[c - 'a'];
        root.indexes.Add(index);
      }
    }

    public List<int> StartsWith(string word)
    {
      Trie root = this;
      foreach (char c in word)
      {
        if (root.children[c - 'a'] == null)
        {
          return new List<int>();
        }
        root = root.children[c - 'a'];
      }

      return root.indexes;
    }
  }

  /**
   * Your WordFilter object will be instantiated and called as such:
   * WordFilter obj = new WordFilter(words);
   * int param_1 = obj.F(prefix,suffix);
   */
}
