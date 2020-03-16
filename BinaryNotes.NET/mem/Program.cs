using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using mmwl.AMQP.ASN1Classes;

namespace mem
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0] == "1") test1(args);

            // takes
            if (args[0] == "2") test2<org.bn.attributes.ASN1Sequence>(args, typeof(mmwl.AMQP.ASN1Classes.Just.Counter));

            // does not
            if (args[0] == "3") test2<org.bn.attributes.ASN1Enum>(args, typeof(mmwl.AMQP.ASN1Classes.Just.Counter.TypeEnumType));

            // takes
            if (args[0] == "4")
            {
                var t = typeof(mmwl.AMQP.ASN1Classes.Just.Counter.TypeEnumType.EnumType);
                var f = t.GetFields()[4];
                test2<org.bn.attributes.ASN1EnumItem>(args, f);
            }

            // takes
            if (args[0] == "5") test2<org.bn.attributes.ASN1Sequence>(args, typeof(mmwl.AMQP.ASN1Classes.Just.ThroughputResponse));

        }

        private static void test2<A>(string[] args, MemberInfo t) where A:Attribute
        {
            var list = new List<A>();
            var s = DateTime.Now;
            int clear = 50000;
            while (true)
            {
                var a = org.bn.coders.CoderUtils.getAttribute<A>(t);
                list.Add(a);
                if (args.Length > 1) Console.ReadLine();
                if (list.Count >= clear)
                {
                    list.Clear();
                    Console.WriteLine("Clear done " + (DateTime.Now - s).TotalMilliseconds / clear);
                    if (args.Length > 0) GC.Collect();
                    s = DateTime.Now;
                }
            }
        }

        public static T getAttribute<T>(ICustomAttributeProvider field)
        {
            object[] attrs = field.GetCustomAttributes(typeof(T), false);
            if (attrs != null && attrs.Length > 0)
            {
                T attribute = (T)attrs[0];
                return attribute;
            }
            else
            {
                return default(T);
            }
        }
        public static T getAttribute2<T>(MemberInfo field) where T:Attribute
        {
            var attr = field.GetCustomAttribute<T>();
            if (attr != null)
            {
                T attribute = (T)attr;
                return attribute;
            }
            else
            {
                return default;
            }
        }

        static CustomAttributeData _d = null;
        public static T getAttribute3<T>(MemberInfo field) where T : Attribute
        {
            var attr = _d ?? field.CustomAttributes.Where(a => a.AttributeType == typeof(T)).FirstOrDefault();
            _d = attr;
            if (attr == null) return default;

            var res = (T)attr.Constructor.Invoke(new object[0]);
            foreach (var a in attr.NamedArguments)
            {
                ((PropertyInfo)a.MemberInfo).SetValue(res, a.TypedValue.Value);
            }
            return res;
        }

        private static void test1(string[] args)
        {
            var bytes = PrepareData();

            var list = new List<mmwl.AMQP.ThroughputResponse>();
            var s = DateTime.Now;
            int clear = 100;
            while (true)
            {
                var r = new mmwl.AMQP.ThroughputResponse();
                var decoded = r.DecodeFromASN1(bytes);
                list.Add(decoded);
                if (args.Length > 1) Console.ReadLine();
                if (list.Count >= clear)
                {
                    list.Clear();
                    Console.WriteLine("Clear done " + (DateTime.Now - s).TotalMilliseconds / clear);
                    if (args.Length > 0) GC.Collect();
                    s = DateTime.Now;
                }
            }
        }

        private static byte[] PrepareData()
        {
            int slices = 601;
            int counters = 10;
            var now = DateTime.Now;

            var r = new mmwl.AMQP.ASN1Classes.Just.ThroughputResponse();
            r.Slices = new List<mmwl.AMQP.ASN1Classes.Just.Slice>();
            for (int i = 0; i < slices; i++)
            {
                var sl = new mmwl.AMQP.ASN1Classes.Just.Slice()
                {
                    PeriodStart = now.AddSeconds(i).ToString("yyMMddHHmmss"),
                    Counters = new List<mmwl.AMQP.ASN1Classes.Just.Counter>()
                };

                for (int j = 0; j < counters; j++)
                {
                    sl.Counters.Add(new mmwl.AMQP.ASN1Classes.Just.Counter()
                    {
                        ConnectionName = "connectionnam asdf,mh  alsdjkfhlkjdhflasjh l kjhe" + Guid.NewGuid().ToString(),
                        CustomerId = Guid.NewGuid().ToString(),
                        GatewayId = Guid.NewGuid().ToString(),
                        Type = new mmwl.AMQP.ASN1Classes.Just.Counter.TypeEnumType() { Value = mmwl.AMQP.ASN1Classes.Just.Counter.TypeEnumType.EnumType.smsIn },
                        Count = j
                    });
                }

                r.Slices.Add(sl);
            }

            return mmwl.AMQP.ASN1Coder.Encode(r);

        }
    }
}
