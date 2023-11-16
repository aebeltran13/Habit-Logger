using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger
{
    public class LogEntry
    {
        public string Habit { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        // You can add additional properties if needed

        public override string ToString()
        {
            return $"Habit: {Habit}, Quantity: {Quantity}, Date: {Date.ToString("yyyy-MM-dd")}";
        }
    }
}
