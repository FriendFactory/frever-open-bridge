using System.Text;
using Bridge.Models.Common;

namespace Bridge.AssetManagerServer
{
    internal class ExpandDeepQueryBuilder<T> where T : IEntity
    {
        internal string Build(ExpandQuerySetup<T> setup,bool addExpandKeyWord) 
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat(addExpandKeyWord
                    ? "$Expand={0}" : ",{0}"
                , setup.RootProperty);

            if (setup.ThenInclude == null)
                return stringBuilder.ToString();

            foreach (var expandedSubFields in setup.ThenInclude)
            {
                stringBuilder.AppendFormat("($Expand={0}", expandedSubFields);
            }

            //close brackets
            var totalCount = setup.ThenInclude.Count;
            for (int i = 0; i < totalCount; i++)
            {
                stringBuilder.AppendFormat(")");
            }

            return stringBuilder.ToString();
        }
    }
}