using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Suffixed;

namespace kOS.Utils
{
    public class CurveValue : Structure
    {
        private FloatCurve curve;

        public CurveValue()
        {
            curve = new FloatCurve();
            InitializeSuffixes();
        }

        private void InitializeSuffixes()
        {
            AddSuffix("ADD", new TwoArgsSuffix<ScalarValue, ScalarValue>(Add));
            AddSuffix("ADDTAN", new OneArgsSuffix<ListValue<ScalarValue>>(AddTan));
            AddSuffix("EVALUATE", new OneArgsSuffix<ScalarValue, ScalarValue>(Evaluate));
            AddSuffix("MINVALUE", new Suffix<ScalarValue>(MinValue));
            AddSuffix("MAXVALUE", new Suffix<ScalarValue>(MaxValue));
            AddSuffix("MINTIME", new Suffix<ScalarValue>(() => curve.minTime));
            AddSuffix("MAXTIME", new Suffix<ScalarValue>(() => curve.maxTime));
        }

        private void Add(ScalarValue time, ScalarValue value)
        {
            curve.Add(time, value);
        }

        private void AddTan(ListValue<ScalarValue> values)
        {
            curve.Add(values[0], values[1], values[2], values[3]);
        }

        private ScalarValue Evaluate(ScalarValue time)
        {
            return curve.Evaluate(time);
        }

        private ScalarValue MinValue()
        {
            curve.FindMinMaxValue(out float minVal, out _);
            return minVal;
        }
        
        private ScalarValue MaxValue()
        {
            curve.FindMinMaxValue(out _, out float maxVal);
            return maxVal;
        }

    }

    [kOSAddon("CURVE")]
    [Safe.Utilities.KOSNomenclature("CURVEAddon")]
    public class CurveAddon : Addon
    {
        public CurveAddon(SharedObjects shared) : base(shared)
        {
            
        }

        private void InitializeSuffixes()
        {
            AddSuffix("NEW", new Suffix<CurveValue>(() => new CurveValue()));
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }
}