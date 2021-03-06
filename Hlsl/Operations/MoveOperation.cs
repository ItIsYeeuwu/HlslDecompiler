﻿namespace HlslDecompiler.Hlsl
{
    public class MoveOperation : UnaryOperation
    {
        public MoveOperation(HlslTreeNode value)
        {
            AddInput(value);
        }

        public override string Mnemonic => "mov";

        public override HlslTreeNode Reduce()
        {
            return Value.Reduce();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
