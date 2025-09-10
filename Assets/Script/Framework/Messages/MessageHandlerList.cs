using System;
using System.Runtime.CompilerServices;

namespace Frame
{
    public class MessageHandlerList<T> : IDisposable
    {
        MessageHandlerNode<T> root; // root 是链表头 root->prev 是链表尾部
        bool isDisposed;
        public MessageHandlerNode<T> Root => root;
        public bool IsDisposed => isDisposed;
        private ulong version = 0;
        object gate = new object(); // 锁

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(MessageHandlerNode<T> node)
        {
            lock (gate)
            {
                if (isDisposed) return;
                node.Parent = this;
                node.Version = version;
                if (root == null)
                {
                    root = node;
                    root.NextNode = null;
                    root.PreviousNode = null;
                }
                else
                {
                    var lastNode = root.PreviousNode ?? root;

                    lastNode.NextNode = node;
                    node.PreviousNode = lastNode;
                    root.PreviousNode = node;

                    node.NextNode = null;
                }
            }
        }
        public void Remove(MessageHandlerNode<T> node)
        {
            lock (gate)
            {
                if (isDisposed) return;
                if (node.Parent != this) return;
                // 怎么判断一个node是否在链表中呢
                if (root == node)
                {
                    // 当前链表只有一个节点
                    if (node.PreviousNode == null || node.NextNode == null)
                    {
                        root = null;
                    }
                    else
                    {
                        var nextRoot = node.NextNode;

                        if (nextRoot.NextNode == null)
                        {
                            nextRoot.PreviousNode = null;
                        }
                        else
                        {
                            nextRoot.PreviousNode = node.PreviousNode; // 设置尾指针
                        }

                        root = nextRoot;
                    }
                }
                else
                {
                    node.PreviousNode!.NextNode = node.NextNode;

                    if (node.NextNode != null)
                    {
                        node.NextNode.PreviousNode = node.PreviousNode;
                    }
                    else
                    {
                        root!.PreviousNode = node.PreviousNode;
                    }
                }
                
                node.PreviousNode = null;
                node.NextNode = null;
                node.Parent = null;
            }
        }

        public void Dispose()
        {
            lock (gate)
            {
                if (isDisposed) return;
                root = null;
                isDisposed = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong GetVersion()
        {
            lock (gate)
            {
                ulong curVersion = 0;
                // 版本溢出了，每次publish都需要检查一下版本
                if (version == ulong.MaxValue)
                {
                    MessageHandlerNode<T> node = root;
                    while (node != null)
                    {
                        node.Version = 0;
                        node = node.NextNode;
                    }
                    version = 1;
                    curVersion = 0;
                }
                else
                {
                    curVersion = version++;
                }
                return curVersion;
            }
        }
    }
}