using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CinammonRoll.Models
{
    public class TrieNode
    {
        private Char character;
        public TrieNode[] children;
        private bool finished;
        private int series;

        public TrieNode(Char c)
        {
            this.character = c;
            this.children = new TrieNode[101];
            this.finished = false;
        }

        public void ToggleFinished(int index)
        {
            this.finished = true;
            this.series = index;
        }

        public int GetIndex()
        {
            return this.series;
        }

        public bool IsFinished()
        {
            return this.finished;
        }

    }

    public class SearchManager
    {
        private TrieNode parent;

        public SearchManager()
        {
            this.parent = new TrieNode('*');
        }

        public void AddWord(string word, int index)
        {
            TrieNode node = this.parent;
            foreach(Char c in word)
            {
                if(node.children[(int)c - 32] != null)
                {
                    node = node.children[(int)c - 32];
                }
                else
                {
                    TrieNode newNode = new TrieNode(c);
                    node.children[(int)c - 32] = newNode;
                    node = newNode;
                }
            }
            node.ToggleFinished(index);

        }

        public List<int> SearchWord(string s)
        {
            TrieNode result = this.GetNode(s);
            List<int> resultIndices = new List<int>();
            if (result != null)
            {
                resultIndices = this.GetFiles(result, resultIndices);
            }
            return resultIndices;
        }

        private TrieNode GetNode(string check)
        {
            TrieNode node = this.parent;
            foreach(Char c in check)
            {
                if(node.children[(int)c -32] != null)
                {
                    node = node.children[(int)c - 32];
                } else
                {
                    // Search not found
                    return null;
                }
            }
            return node;
        }

        private List<int> GetFiles(TrieNode node, List<int> indices)
        {
            if(node.IsFinished())
            {
                indices.Add(node.GetIndex());
            }
            foreach (TrieNode n in node.children)
            {
                if (n != null)
                {
                    indices = GetFiles(n, indices);
                }
            }
            return indices;
        }
    }
}
