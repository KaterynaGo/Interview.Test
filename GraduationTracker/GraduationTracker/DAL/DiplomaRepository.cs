using GraduationTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker.DAL
{
    public class DiplomaRepository : IDiplomaRepository
    {
        public Diploma GetDiploma(int id)
        {
            Diploma diploma = GetDiplomas().Where(x => x.Id == id).First();
            return diploma;
        }

        public List<Diploma> GetDiplomas()
        {
            return new List<Diploma>
           {
                new Diploma
                {
                    Id = 1,
                    Credits = 4,
                    Requirements = new int[]{100,102,103,104}
                }
            };
        }
    }
}
