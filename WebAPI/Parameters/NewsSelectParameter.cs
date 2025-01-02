using System.Text.RegularExpressions;

namespace WebAPI.Parameters
{
    public class NewsSelectParameter
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool? Enable { get; set; }
        public DateTime? StartDateTime { get; set; }
        public int? MinOrder { get; set; }
        public int? MaxOrder { get; set; }

        private string _order;
        public string Order
        {
            get { return _order; }
            set
            {
                Regex regex = new Regex(@"^\d*-\d$");
                if (regex.Match(value).Success)
                {
                    MinOrder = Int32.Parse(value.Split('-')[0]);
                    MaxOrder = Int32.Parse(value.Split('-')[1]);
                }
                _order = value;
            }
        } 
    }
}
