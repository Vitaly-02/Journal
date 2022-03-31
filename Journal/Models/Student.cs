using System;
using System.Collections.Generic;

namespace Journal.Models
{
    class StudentState
    {
        private static List<string> __disciplines { get; set; }
        private static string __path { get; set; }

        public static void SetListOfDisciplines(List<String> __listOfDisc)
        {
            __disciplines = __listOfDisc;
        }

        public static List<String> GetListOfDisciplines()
        {
            return __disciplines;
        }

        public static void SetPathToFile(string path)
        {
            if (path != null)
            {
                __path = path;
            }
        }

        public static string GetPathToFile()
        {
            if (__path != null)
            {
                return __path;
            }
            else
            {
                return "";
            }
        }

        public StudentState(string name, List<short> grades)
        {
            Grades = grades;
            Name = name;
        }

        public string Name { get; set; }
        public List<short> Grades { get; set; }
    }
}