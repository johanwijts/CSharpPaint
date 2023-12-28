using CSharpPaint.Editors.Shapes;

namespace CSharpPaint.Visitors
{
    public interface IVisitor
    {
        void Visit(BaseShape baseShape);
    }
}
