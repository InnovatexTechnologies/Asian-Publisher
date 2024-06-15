namespace AsianPublisher.Models
{
    public class CourseSemester
    {
        public string courseId { get; set; }
        public string courseName { get; set; }
        public string courseType { get; set; }
        public string semesterId { get; set; }
        public string semesterName { get; set; }
        public string semesterType { get; set; }
        public string combine => courseName +" ("+ semesterName+")";


    }
}
