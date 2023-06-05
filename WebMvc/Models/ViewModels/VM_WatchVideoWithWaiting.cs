using WebMvc.Models.Domain;

namespace WebMvc.Models.ViewModels
{
    public class VM_WatchVideoWithWaiting
    {
            public string studentNo { get; set; }
            public string name { get; set; }
        public double waitToStart { get; set; }
        public double startTime { get; set; }
        public List<Questions> Question { get; set; }


    }
}
