using CSharpPaint.Editors.Shapes;
using CSharpPaint.Visitors;

namespace CSharpPaint.Decorators
{
    public abstract class ShapeDecorator : IShape
    {
        protected IShape decoratedShape { get; set; }

        public ShapeDecorator(IShape decoratedShape)
        {
            this.decoratedShape = decoratedShape;
        }

        public virtual string GetDescription()
        {
            return decoratedShape.GetDescription();
        }

        public virtual void SetDescription(string description)
        {
            decoratedShape.SetDescription(description);
        }

        public void Accept(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
