using GraduationTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker.DAL
{
    public class RequirementRepository : IRequirementRepository
    {
        public Requirement GetRequirement(int id)
        {
            Requirement requirement = GetRequirements().Where(x => x.Id == id).First();
            return requirement;
        }

        public List<Requirement> GetRequirements()
        {
            return new List<Requirement>()
            {
                    new Requirement{Id = 100, Name = "Math", MinimumMark=50, Courses = new int[]{1}, Credits=1 },
                    new Requirement{Id = 102, Name = "Science", MinimumMark=50, Courses = new int[]{2}, Credits=1 },
                    new Requirement{Id = 103, Name = "Literature", MinimumMark=50, Courses = new int[]{3}, Credits=1},
                    new Requirement{Id = 104, Name = "Physichal Education", MinimumMark=50, Courses = new int[]{4}, Credits=1 }
            };
        }
    }
}
