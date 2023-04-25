##  Introduction
As developers we know that the first step to solving a problem is to understand it.
This exercise is interesting because the process is more important than the result itself.
In this case, we want to develop an application that receives a matrix of characters and a list of words as input parameters. The application should search for each word in the list within the matrix and return a list of the top 10 most frequently found words as a result.

Other aspects to consider are:

- Words can appear horizontally from left to right and vertically from top to bottom.
- All words to search have the same number of characters.
- If there is any repeated word in the list, it should only be searched once.
- This class must be implemented:
```
public class WordFinder
{
		public WordFinder(IEnumerable<string> matrix)
		{
		
		}

		public IEnumerable<string> Find(IEnumerable<string> wordstream)
		{
		
		}
}
```
## Analysis and development
I proceeded to analyze the problem and think about possible solutions, since it's about achieving the greatest efficiency in search, I was sure that I should use data structures like trees to achieve an optimal solution. So, I decided to use Trie (also known as a prefix tree), which is a tree-shaped data structure used to efficiently store and retrieve a large number of strings. In a trie, each node represents a prefix of the stored strings, and each path from the root to a terminal node represents a complete string. It's mainly used in applications involving text search. Therefore, I implemented the Trie class and the TrieNode class in the application.
In this image we can see how the words are organized in the Trie structure
[![Trie](https://theoryofprogramming.files.wordpress.com/2015/01/trie12.jpg "Trie")](https://theoryofprogramming.files.wordpress.com/2015/01/trie12.jpg "Trie")


After that I started to develop the main WordFinder class taking into account the requirements of the application. The WordFinder class has 4 properties that are initialized in the constructor:
```
 	private readonly char[][] _matrix;
     private readonly int _numMatrixRows;
     private readonly int _numMatrixColumns;
     private readonly Trie _trie;
```
- _matrix: Two-dimensional array of characters that will contain the matrix itself, represented as characters so that we can traverse it character by character.
- _numMatrixRows and _numMatrixColumns: These are properties that we will use to traverse the matrix later.
- _trie: Of course our trie to find matches.

After implementing the Find method,
In this line, we eliminate any possible repeated word by using a HashSet.
```
var uniqueWords = new HashSet<string>(wordstream);
```
Given that all words in the list are of the same size
in this line, we create the variable lengthWord, which will be used later in the horizontal and vertical search.
```
int lengthWord = uniqueWords.Count > 0 ? uniqueWords.First().Length : 0;
```

We load our trie:
```
    _trie.Clear();
    foreach (var word in uniqueWords)
    {
        _trie.Insert(word);
    }
```

I created the wordsCount variable to return the top 10 most found words:
```
var wordCounts = new Dictionary<string, int>();
```
To perform the search, we traverse the matrix using a pair of nested for loops to walk through each row and column horizontally and vertically. We have the variable "accumulation" of type StringBuilder that we use to query the trie object if the substring exists. We use the variable "history" of type List so that if the substring is not found, the search continues from the next position, thus not skipping any characters in the matrix. If the substring is found within the trie object, we ask if it is the "end of word," and if it is, we update the "wordcounts" variable and reset both the "accumulation" and "history."
```
private void SearchInMatrix(int jumpTo, int walkTo, int lengthWord, ref Dictionary<string, int> wordCounts, SearchDirection direction) 
        {            
            for (int i = 0; i < jumpTo; i++)
            {
                var accumulation = new StringBuilder();
                List<int> history = new List<int>();

                for (int j = 0; j < walkTo; j++)
                {
                    //char currentChar = isHorizontalSearch ? _matrix[i][j]:_matrix[j][i];
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
```
Something interesting is that if the substring is not found and the remaining cells are less than the length of the words, the search stops and moves on to the next row or column because it is obvious that the remaining characters are not enough to form a word from the list of words.This is where the "lengthWord" variable is useful.
The method SearchInMatrix was implemented to perform vertical and horizontal search and reuse code, remember: don't repeat yourself.

After finishing the horizontal and vertical search, we sort the found words to return the top 10. For this, we use the LinQ library.
```
 return wordCounts.OrderByDescending(kv => kv.Value)
                                 .ThenBy(kv => kv.Key)
                                 .Take(10)
                                 .Select(kv => kv.Key)
                                 .ToList();
```
First, the elements of the wordCounts dictionary are sorted in descending order by the values (kv.Value) of each key-value pair (kv).
Then, the elements are sorted in ascending order by the keys (kv.Key) in case there are elements with equal values.
Next, the first 10 elements of the sorted dictionary are taken.
Then, only the keys (kv.Key) of each key-value pair (kv) in the dictionary are selected.
Finally, the result is converted into a list.

## Conclusion
Definitely, the larger the size of the matrix and the wordstream, the longer the program's execution time will be. It would be interesting to perform a Big O notation analysis to understand how this algorithm would scale according to the input.

These types of exercises are very good for practicing data structures and challenging ourselves on how we can optimize processes. Thank you for your time.