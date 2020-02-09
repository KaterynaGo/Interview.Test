using GraduationTracker.DAL;
using GraduationTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker.BLL
{
    public class GraduationTrackerBLL
    {
        private readonly RequirementRepository _requirementRepository;

        public GraduationTrackerBLL() { }
        public GraduationTrackerBLL(RequirementRepository requirementRepository) 
        {
            _requirementRepository = requirementRepository;
        }

        public Tuple<bool, STANDING> GetGraduationStatus(Diploma diploma, Student student)
        {
            Tuple<bool, STANDING> graduationStatus = null;
            int averageMark = GetAverageMark(diploma, student);
            if (averageMark > 0)
            {
                STANDING standing = GetStanding(averageMark);
                bool hasGraduated = HasGraduated(standing);
                graduationStatus = new Tuple<bool, STANDING>(hasGraduated, standing);
            }
            return graduationStatus;
        }
        public int GetAverageMark(Diploma diploma, Student student)
        {
            int credits = 0;
            int marksTotal = 0;
            int average = 0;

            if (diploma.Requirements != null && student.Courses != null)
            {
                for (int i = 0; i < diploma.Requirements.Length; i++)
                {
                    for (int j = 0; j < student.Courses.Length; j++)
                    {
                        Requirement requirement = _requirementRepository.GetRequirement(diploma.Requirements[i]);
                        if (requirement == null)
                        {
                            throw new ArgumentNullException($"Requirement with id: {diploma.Requirements[i]} was not found");
                        }

                        for (int k = 0; k < requirement.Courses.Length; k++)
                        {
                            if (requirement.Courses[k] == student.Courses[j].Id)
                            {
                                marksTotal += student.Courses[j].Mark < 0 ? 0 : student.Courses[j].Mark;
                                if (student.Courses[j].Mark > requirement.MinimumMark)
                                {
                                    credits += requirement.Credits < 0 ? 0 : requirement.Credits;
                                }
                            }
                        }
                    }
                }

                average = marksTotal / student.Courses.Length;
            }

            return average;
        }

        public STANDING GetStanding(int average)
        {
            STANDING standing = STANDING.None;

            if (average < 50)
                standing = STANDING.Remedial;
            else if (average < 80)
                standing = STANDING.Average;
            else if (average < 95)
                standing = STANDING.MagnaCumLaude;
            else
                standing = STANDING.SumaCumLaude;

            return standing;
        }

        public bool HasGraduated(STANDING standing)
        {
            switch (standing)
            {
                case STANDING.Remedial:
                    return false;

                case STANDING.Average:
                case STANDING.SumaCumLaude:
                case STANDING.MagnaCumLaude:
                    return true;

                default:
                    return false;
            }
        }
    }
}
