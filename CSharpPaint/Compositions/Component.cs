using System;
using System.Windows;

namespace CSharpPaint.Compositions
{
    public abstract class Component
    {
        public abstract void MoveOperation(Point compositeMiddle);

        public abstract void SizeOperation();

        public virtual void Add(Component component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(Component component)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsComposite()
        {
            return true;
        }
    }
}
