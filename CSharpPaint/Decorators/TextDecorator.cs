using CSharpPaint.Editors.Shapes;

namespace CSharpPaint.Decorators
{
    public class TextDecorator : ShapeDecorator
    {
        public TextDecorator(IShape shape) : base(shape)
        {
        }

        public override string GetDescription()
        {
            return base.GetDescription() + "This Shape has text";
        }

        public override void SetDescription(string description)
        {
            base.SetDescription(description);
        }
    }
}
