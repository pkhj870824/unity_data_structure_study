using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class AVLTreeTest<TKey, TValue> : AVLTree<TKey, TValue> where TKey : IComparable<TKey>
{
    public TreeNode<TKey, TValue> GetRoot()
    {
        return root;
    }
}
public class AVLTree<TKey, TValue> : BinarySearchTree<TKey, TValue>
where TKey : IComparable<TKey>
{

    protected override TreeNode<TKey, TValue> Add(
        TreeNode<TKey, TValue> node,
        TKey key,
        TValue value
    )
    {
        node = base.Add(node, key, value);
        return Balance(node);
    }
    protected override TreeNode<TKey, TValue> AddOrUpdate(
        TreeNode<TKey, TValue> node,
        TKey key,
        TValue value
    )
    {
        node = base.AddOrUpdate(node, key, value);
        return Balance(node);
    }
    protected override TreeNode<TKey, TValue> Remove(
TreeNode<TKey, TValue> node,
TKey key
)
    {
        node = base.Remove(node, key);
        return Balance(node);
    }

    protected int BalanceFactor(TreeNode<TKey, TValue> node)
    {
        return node == null ? 0 : Height(node.Left) - Height(node.Right);
    }
    protected TreeNode<TKey, TValue> Balance(TreeNode<TKey, TValue> node)
    {
        int balanceFactor = BalanceFactor(node);
        //LL구현
        if (BalanceFactor(node) > 1 && BalanceFactor(node.Left) >= 0)
        {
            return RotateRight(node);
        }
        //RR구현
        else if (BalanceFactor(node) < -1 && BalanceFactor(node.Right) <= 0)
        {
            return RotateLeft(node);
        }
        //LR구현
        else if (BalanceFactor(node) > 1 && BalanceFactor(node.Left) < 0)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }
        //RL구현
        else if (BalanceFactor(node) < -1 && BalanceFactor(node.Right) > 0)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }
    public bool IsBalanced()
    {
        return IsBalanced(root);
    }

    private bool IsBalanced(TreeNode<TKey, TValue> node)
    {
        if (node == null)
        {
            return true;
        }
        if (Mathf.Abs(BalanceFactor(node)) > 1)
        {
            return false;
        }
        return IsBalanced(node.Left) && IsBalanced(node.Right);
    }
    protected TreeNode<TKey, TValue> RotateLeft(TreeNode<TKey, TValue> node)
    {
        TreeNode<TKey, TValue> rotateNum = node.Right;
        node.Right = rotateNum.Left;
        rotateNum.Left = node;
        UpdateHeight(node);
        UpdateHeight(rotateNum);
        return rotateNum;
    }
    protected TreeNode<TKey, TValue> RotateRight(TreeNode<TKey, TValue> node)
    {
        TreeNode<TKey, TValue> rotateNum = node.Left;
        node.Left = rotateNum.Right;
        rotateNum.Right = node;
        UpdateHeight(node);
        UpdateHeight(rotateNum);
        return rotateNum;
    }
}
