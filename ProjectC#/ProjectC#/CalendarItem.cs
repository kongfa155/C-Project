namespace ProjectC_
{
    public class CalendarItem
    {
        public int Id { get; set; }
        public string Type { get; set; }    // "Schedule" hoặc "Expense"
        public string Content { get; set; }
        public string Description { get; set; }
        // Bạn có thể thêm các thuộc tính khác như Amount, Time...
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        // Cập nhật Logic Icon để hiển thị đẹp hơn
        public string Icon
        {
            get
            {
                if (Type == "Expense") return "💰";
                if (Type == "Birthday") return "🎂";
                return "📅";
            }
        }

        public override string ToString()
        {
            return $"{Icon} {Content}";
        }
    }
}
