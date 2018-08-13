﻿namespace HlslDecompiler.Hlsl
{
    public class MultiplyOperation : Operation
    {
        public MultiplyOperation(HlslTreeNode factor1, HlslTreeNode factor2)
        {
            AddInput(factor1);
            AddInput(factor2);
        }

        public HlslTreeNode Factor1 => Inputs[0];
        public HlslTreeNode Factor2 => Inputs[1];

        public override string Mnemonic => "mul";

        public override HlslTreeNode Reduce()
        {
            var factor1 = Factor1.Reduce();
            var factor2 = Factor2.Reduce();

            var constant1 = factor1 as ConstantNode;
            var constant2 = factor2 as ConstantNode;
            if (constant1 != null)
            {
                float value1 = constant1.Value;
                if (value1 == 0)
                {
                    Replace(factor1);
                    return factor1;
                }
                if (value1 == 1)
                {
                    Replace(factor2);
                    return factor2;
                }
                if (value1 == -1)
                {
                    var negation = new NegateOperation(factor2);
                    Replace(negation);
                    return negation;
                }
                if (constant2 != null)
                {
                    return new ConstantNode(value1 * constant2.Value);
                }
            }

            if (constant2 != null)
            {
                float value2 = constant2.Value;
                if (value2 == 0)
                {
                    Replace(factor2);
                    return factor2;
                }
                if (value2 == 1)
                {
                    Replace(factor1);
                    return factor1;
                }
                if (value2 == -1)
                {
                    var negation = new NegateOperation(factor1);
                    Replace(negation);
                    return negation;
                }
            }

            if (factor1 is DivisionOperation reciprocalDivision)
            {
                if (reciprocalDivision.Dividend is ConstantNode one && one.Value == 1)
                {
                    var division = new DivisionOperation(factor2, reciprocalDivision.Divisor);
                    Replace(division);
                    return division;
                }
            }

            Inputs[0] = factor1;
            Inputs[1] = factor2;
            return this;
        }
    }
}
