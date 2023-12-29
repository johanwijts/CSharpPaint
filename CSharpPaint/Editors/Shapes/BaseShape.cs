using CSharpPaint.Visitors;
using System.Windows.Shapes;

namespace CSharpPaint.Editors.Shapes
{
    public class BaseShape : IShape
    {
        public BaseShape()
        {
        }

        public BaseShape(Shape shape)
        {
            this.shape = shape;
            this.width = shape.Width;
            this.height = shape.Height;
        }

        public Shape shape { get; set; } = new Rectangle();

        public double width;
        public double height;
        private string description;

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string GetDescription()
        {
            return this.description;
        }

        public void SetDescription(string description)
        {
            this.description = description;
        }
    }
}
