using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageQuantization
{
    class MinHeap
    {
        List<HeapNode> NodeList = new List<HeapNode>();
        Dictionary<Tuple<RGBPixel, RGBPixel>,bool> NodesMap = new Dictionary<Tuple<RGBPixel, RGBPixel>, bool>();
        private int getParentIndex(int index)
        {
            return (index - 1) / 2;
        }
        private int getRightChildIndex(int index)
        {
            return (index * 2) + 2;
        }
        private int getLeftChildIndex(int index)
        {
            return (index * 2) + 1;
        }
        private bool hasparent(int index)
        {
            return (getParentIndex(index) >= 0);
        }
        private bool hasRightChild(int index)
        {
            return (getRightChildIndex(index) < NodeList.Count);
        }
        private bool hasLeftChild(int index)
        {
            return (getLeftChildIndex(index) < NodeList.Count);
        }
        private HeapNode getParent(int index)
        {
            return NodeList[getParentIndex(index)];
        }
        private HeapNode getRightChild(int index)
        {
            return NodeList[getRightChildIndex(index)];
        }
        private HeapNode getLeftChild(int index)
        {
            return NodeList[getLeftChildIndex(index)];
        }
        public HeapNode ExtractMinNode()
        {
            HeapNode min = NodeList[0];
            NodeList[0] = NodeList[NodeList.Count - 1];
            NodeList.RemoveAt(NodeList.Count - 1);
            
            if (NodeList.Count > 0)
            {
                HeapfiyDown(0);
            }
            return min;
        }
        private void HeapfiyDown(int index)
        {
            bool Test;

            do
            {
                Test = false;
                int Smallest = index;
                if (hasLeftChild(index) && getLeftChild(index).weight < NodeList[Smallest].weight)
                {
                    Smallest = getLeftChildIndex(index);
                }
                if (hasRightChild(index) && getRightChild(index).weight < NodeList[Smallest].weight)
                {
                    Smallest = getRightChildIndex(index);
                }
                if (Smallest != index)
                {
                    Swape(Smallest, index);
                    index = Smallest;
                    Test = true;
                }

            } while (Test);
        }
        private void Swape(int index1, int index2)
        {
            HeapNode node = NodeList[index1];
            NodeList[index1] = NodeList[index2];
            NodeList[index2] = node;
        }
        public void AddNode(HeapNode node)
        {
            NodeList.Add(node);
            HeapfiyUp(NodeList.Count - 1);
        }
        private void HeapfiyUp(int index)
        {
            while (hasparent(index) && getParent(index).weight > NodeList[index].weight)
            {
                
                Swape(getParentIndex(index), index);
                index = getParentIndex(index);
            }
        }
        public int Size()
        {
            return NodeList.Count;
        }
      
        public void UpdateHeap()
        {
            for (int i = 1; i < NodeList.Count; i++)
            {
                HeapfiyUp(i);
            }
        }
        public bool NotEmpty()
        {
            return (NodeList.Count > 0);
        }
    }
}
