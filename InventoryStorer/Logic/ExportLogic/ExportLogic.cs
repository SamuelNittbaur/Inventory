using System.Text;

namespace Logic.ExportLogic
{
    public static class ExportManager
    {
        public static string GenerateCsv<T>(IEnumerable<T> data)
        {
            var properties = typeof(T).GetProperties();
            var csvBuilder = new StringBuilder();

            // Add header row
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Add data rows
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item, null)?.ToString() ?? string.Empty);
                csvBuilder.AppendLine(string.Join(",", values));
            }

            return csvBuilder.ToString();
        }
    }
}
