using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordFinderApp
{
    public class WordFinder
    {
        private readonly char[][] _matrix;
        private readonly int _numMatrixRows;
        private readonly int _numMatrixColumns;
        private readonly Trie _trie;

        public WordFinder(IEnumerable<string> matrix)
        {
            _matrix = matrix.Select(row => row.ToCharArray()).ToArray();
            _numMatrixRows = _matrix.Length;
            _numMatrixColumns = _matrix[0].Length;

            _trie = new Trie();
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            try
            {
                var uniqueWords = new HashSet<string>(wordstream);
                int lengthWord = uniqueWords.Count > 0 ? uniqueWords.First().Length : 0;

                _trie.Clear();
                foreach (var word in uniqueWords)
                {
                    _trie.Insert(word);
                }

                var wordCounts = new Dictionary<string, int>();

                SearchInMatrix(_numMatrixRows, _numMatrixColumns, lengthWord, ref wordCounts, SearchDirection.Horizontal);


                SearchInMatrix(_numMatrixColumns, _numMatrixRows, lengthWord, ref wordCounts, SearchDirection.Vertical);


                return wordCounts.OrderByDescending(kv => kv.Value)
                                 .ThenBy(kv => kv.Key)
                                 .Take(10)
                                 .Select(kv => kv.Key)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while processing the wordstream.", ex);
            }
        }

        private void SearchInMatrix(int jumpTo, int walkTo, int lengthWord, ref Dictionary<string, int> wordCounts, SearchDirection direction)
        {
            for (int i = 0; i < jumpTo; i++)
            {
                var accumulation = new StringBuilder();
                List<int> history = new List<int>();

                for (int j = 0; j < walkTo; j++)
                {                    
                    char currentChar = GetMatrixChar(i, j, direction);
                    accumulation.Append(currentChar);
                    history.Add(j);

                    var wordInfo = _trie.FindWord(accumulation.ToString());
                    if (wordInfo.Found && wordInfo.IsEndOfWord)
                    {
                        if (!wordCounts.ContainsKey(accumulation.ToString()))
                        {
                            wordCounts[accumulation.ToString()] = 1;
                        }
                        else
                        {
                            wordCounts[accumulation.ToString()]++;
                        }

                        accumulation.Clear();
                        history.Clear();
                    }
                    else if (!wordInfo.Found)
                    {
                        if (lengthWord >= (walkTo - j))
                        {
                            break;
                        }

                        accumulation.Clear();
                        j = history[0];
                        history.Clear();
                    }
                }
            }
        }

        private char GetMatrixChar(int row, int col, SearchDirection direction)
        {
            switch (direction)
            {
                case SearchDirection.Horizontal:
                    return _matrix[row][col];
                case SearchDirection.Vertical:
                    return _matrix[col][row];
                default:
                    throw new ArgumentException($"Invalid search direction: {direction}", nameof(direction));
            }
        }
    }
}
