using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using GraduationTracker.Model;
using GraduationTracker.BLL;
using GraduationTracker.DAL;

namespace GraduationTracker.Tests.Unit
{
    [TestClass]
    public class GraduationTrackerTests
    {
        private DiplomaRepository _diplomaRepository;
        private RequirementRepository _requirementRepository;
        private StudentRepository _studentRepository;
        private GraduationTrackerBLL _tracker;

        [TestInitialize]
        public void Init()
        {
            _diplomaRepository = new DiplomaRepository();
            _requirementRepository = new RequirementRepository();
            _studentRepository = new StudentRepository();
            _tracker = new GraduationTrackerBLL( _requirementRepository);
        }

        [TestMethod]
        public void Student_TestHasCredits()
        {
            Diploma diploma = _diplomaRepository.GetDiploma(1);

            List<Student> students = _studentRepository.GetStudents();

            List<Tuple<bool, STANDING>> graduationStatus = new List<Tuple<bool, STANDING>>();

            foreach (Student student in students)
            {
                graduationStatus.Add(_tracker.GetGraduationStatus(diploma, student));
            }

            Assert.IsTrue(graduationStatus.Any());

        }

        [TestMethod]
        public void Students_TestAllGraduated()
        {
            Diploma diploma = _diplomaRepository.GetDiploma(1);
            List<List<int>> marks = new List<List<int>>();
            List<int> marks1 = new List<int>() { 80, 60, 70, 80 };
            List<int> marks2 = new List<int>() { 100, 90, 90, 85 };
            marks.Add(marks1);
            marks.Add(marks2);

            List<Student> students = GetStudents_Fake(marks);
            int countHasGraduated = 0;
            foreach (Student student in students)
            {
                bool hasGraduated = _tracker.GetGraduationStatus(diploma, student).Item1;
                if (hasGraduated)
                    countHasGraduated++;
            }

            Assert.AreEqual(marks.Count(), countHasGraduated);
        }

        [TestMethod]
        public void Students_TestAllFailed()
        {
           
            Diploma diploma = _diplomaRepository.GetDiploma(1);
            List<List<int>> marks = new List<List<int>>();
            List<int> marks1 = new List<int>() { 40, 30, 40, 60 };
            List<int> marks2 = new List<int>() { 60, 45, 30, 30 };
            marks.Add(marks1);
            marks.Add(marks2);

            List<Student> students = GetStudents_Fake(marks);
            int countHasGraduated = 0;
            foreach (Student student in students)
            {
                bool hasGraduated = _tracker.GetGraduationStatus(diploma, student).Item1;
                if (!hasGraduated)
                    countHasGraduated++;
            }

            Assert.AreEqual(marks.Count(), countHasGraduated);
        }


        [TestMethod]
        public void TestStatus_Remedial()
        {
            Diploma diploma = _diplomaRepository.GetDiploma(1);
            List<List<int>> marks = new List<List<int>>();
            List<int> marks1 = new List<int>() { 40, 30, 40, 60 };
            List<int> marks2 = new List<int>() { 60, 45, 30, 30 };
            marks.Add(marks1);
            marks.Add(marks2);
            List<STANDING> standings = new List<STANDING>();

            List<Student> students = GetStudents_Fake(marks);
            foreach (Student student in students)
            {
                STANDING studentStanding = _tracker.GetGraduationStatus(diploma, student).Item2;
                standings.Add(studentStanding);
            }

            Assert.IsTrue(standings.FindAll(x => x == STANDING.Remedial).Count() == 2);
        }

        [TestMethod]
        public void TestStatus_Average()
        {
            Diploma diploma = _diplomaRepository.GetDiploma(1);
            List<List<int>> marks = new List<List<int>>();
            List<int> marks1 = new List<int>() { 50, 60, 70, 80 };
            List<int> marks2 = new List<int>() { 60, 50, 65, 70 };
            marks.Add(marks1);
            marks.Add(marks2);
            List<STANDING> standings = new List<STANDING>();

            List<Student> students = GetStudents_Fake(marks);
            foreach (Student student in students)
            {
                STANDING studentStanding = _tracker.GetGraduationStatus(diploma, student).Item2;
                standings.Add(studentStanding);
            }

            Assert.IsTrue(standings.FindAll(x => x == STANDING.Average).Count() == 2);
        }

        [TestMethod]
        public void TestStatus_MagnaCumLaude()
        {
           
            Diploma diploma = _diplomaRepository.GetDiploma(1);
            List<List<int>> marks = new List<List<int>>();
            List<int> marks1 = new List<int>() { 80, 90, 85, 90 };
            List<int> marks2 = new List<int>() { 70, 90, 95, 80 };
            marks.Add(marks1);
            marks.Add(marks2);
            List<STANDING> standings = new List<STANDING>();

            List<Student> students = GetStudents_Fake(marks);
            foreach (Student student in students)
            {
                STANDING studentStanding = _tracker.GetGraduationStatus(diploma, student).Item2;
                standings.Add(studentStanding);
            }

            Assert.IsTrue(standings.FindAll(x => x == STANDING.MagnaCumLaude).Count() == 2);
        }

        [TestMethod]
        public void TestStatus_SumaCumLaude()
        {
           
            Diploma diploma = _diplomaRepository.GetDiploma(1);
            List<List<int>> marks = new List<List<int>>();
            List<int> marks1 = new List<int>() { 95, 90, 100, 100 };
            List<int> marks2 = new List<int>() { 100, 100, 95, 85 };
            marks.Add(marks1);
            marks.Add(marks2);
            List<STANDING> standings = new List<STANDING>();

            List<Student> students = GetStudents_Fake(marks);
            foreach (Student student in students)
            {
                STANDING studentStanding = _tracker.GetGraduationStatus(diploma, student).Item2;
                standings.Add(studentStanding);
            }

            Assert.IsTrue(standings.FindAll(x => x == STANDING.SumaCumLaude).Count() == 2);
        }

        [TestMethod]
        public void TestHasRequiredCourses_WithExtraCourse_IsTrue()
        {
            Student student = new Student()
            {
                Id = 1,
                Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=85 },
                        new Course{Id = 2, Name = "Science", Mark=70 },
                        new Course{Id = 3, Name = "Literature", Mark=60 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=65 },
                        new Course{Id = 5, Name = "Chemistry", Mark=40 }
                   }
            };

            bool hasCourse = true;
            List<Requirement> requirements = _requirementRepository.GetRequirements();
            foreach (Requirement requirement in requirements)
            {
                if (!student.Courses.Where(x => x.Name == requirement.Name).Any())
                {
                    hasCourse = false;
                }
            }
            Assert.IsTrue(hasCourse);
        }

        [TestMethod]
        public void TestHasRequiredCourses_IsFalse()
        {
            Student student = new Student()
            {
                Id = 1,
                Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=85 },
                        new Course{Id = 2, Name = "Chemistry", Mark=70 },
                        new Course{Id = 3, Name = "Literature", Mark=60 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=65 }
                   }
            };

            bool hasCourse = true;
            List<Requirement> requirements = _requirementRepository.GetRequirements();
            foreach (Requirement requirement in requirements)
            {
                if (!student.Courses.Where(x => x.Name == requirement.Name).Any())
                {
                    hasCourse = false;
                }
            }
            Assert.IsFalse(hasCourse);
        }

        [TestMethod]
        public void TestPassedAllRequiredCourses_IsTrue()
        {
            Student student = new Student()
            {
                Id = 1,
                Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=85 },
                        new Course{Id = 2, Name = "Science", Mark=70 },
                        new Course{Id = 3, Name = "Literature", Mark=60 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=65 }
                   }
            };

            bool passed = true;
            List<Requirement> requirements = _requirementRepository.GetRequirements();
            foreach (Requirement requirement in requirements)
            {
                Course course = student.Courses.Where(x => x.Name == requirement.Name).First();
                if (course.Mark < requirement.MinimumMark)
                {
                    passed = false;
                }
            }
            Assert.IsTrue(passed);
        }

        [TestMethod]
        public void TestPassedRequiredCourses_IsFalse()
        {
            Student student = new Student()
            {
                Id = 1,
                Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=45 },
                        new Course{Id = 2, Name = "Science", Mark=40 },
                        new Course{Id = 3, Name = "Literature", Mark=80 },
                        new Course{Id = 4, Name = "Physichal Education", Mark=90 }
                   }
            };

            bool passed = true;
            List<Requirement> requirements = _requirementRepository.GetRequirements();
            foreach (Requirement requirement in requirements)
            {
                Course course = student.Courses.Where(x => x.Name == requirement.Name).First();
                if (course.Mark < requirement.MinimumMark)
                {
                    passed = false;
                }
            }
            Assert.IsFalse(passed);
        }


        #region Helper methods
        public List<Student> GetStudents_Fake(List<List<int>> marks)
        {
            List<Student> students = new List<Student>();
            int idCount = 1;

            foreach (List<int> marksPerStudent in marks)
            {
                Student student = new Student()
                {
                    Id = idCount,
                    Courses = new Course[]
                   {
                        new Course{Id = 1, Name = "Math", Mark=marksPerStudent.ElementAt(0) },
                        new Course{Id = 2, Name = "Science", Mark=marksPerStudent.ElementAt(1) },
                        new Course{Id = 3, Name = "Literature", Mark=marksPerStudent.ElementAt(2) },
                        new Course{Id = 4, Name = "Physichal Education", Mark=marksPerStudent.ElementAt(3) }
                   }
                };
                students.Add(student);
            }

            return students;
        }

        #endregion

    }
}
