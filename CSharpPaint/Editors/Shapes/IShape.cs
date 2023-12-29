using CSharpPaint.Visitors;

namespace CSharpPaint.Editors.Shapes
{
    public interface IShape
    {
        void Accept(IVisitor visitor);

        string GetDescription();

        void SetDescription(string description);
    }
}
