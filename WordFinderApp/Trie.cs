using System;
using System.Collections.Generic;
using System.Text;

namespace WordFinderApp
{
    public class Trie
    {
        private TrieNode _root;

        public Trie()
        {
            _root = new TrieNode('^');
        }

        public void Insert(string word)
        {
            var node = _root;

            foreach (var c in word)
            {
                if (!node.Children.ContainsKey(c))
                {
                    node.Children[c] = new TrieNode(c);
                }
                node = node.Children[c];
            }

            node.IsEndOfWord = true;
        }

        public WordInfo FindWord(string prefix)
        {
            var node = _root;

            foreach (var c in prefix)
            {
                if (!node.Children.ContainsKey(c))
                {
                    return new WordInfo { Found = false, IsEndOfWord = false };
                }
                node = node.Children[c];
            }

            return new WordInfo { Found = true, IsEndOfWord = node.IsEndOfWord };
        }

        public void Clear()
        {
            _root.Children.Clear();
        }

        private class TrieNode
        {
            public char Value { get; set; }
            public bool IsEndOfWord { get; set; }
            public Dictionary<char, TrieNode> Children { get; set; }

            public TrieNode(char value)
            {
                Value = value;
                IsEndOfWord = false;
                Children = new Dictionary<char, TrieNode>();
            }
        }

        public class WordInfo
        {
            public bool Found { get; set; }
            public bool IsEndOfWord { get; set; }
        }
    }
}
