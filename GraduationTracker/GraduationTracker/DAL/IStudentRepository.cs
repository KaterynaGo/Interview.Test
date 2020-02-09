using GraduationTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker.DAL
{
   public interface IStudentRepository
    {
        Student GetStudent(int id);
        List<Student> GetStudents();
    }
}
