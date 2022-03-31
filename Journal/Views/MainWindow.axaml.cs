using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Journal.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Journal.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static List<StudentState> __states = new List<StudentState>();

        private void Button_File_Click(object sender, RoutedEventArgs e)
        {
            var _FileButton = sender as MenuItem;
            if (_FileButton != null)
            {
                _FileButton.ContextMenu.Open();
                if (_FileButton.ContextMenu.IsOpen)
                {
                    _FileButton.ContextMenu.FindControl<MenuItem>("Button_Save").Click += delegate
                    {
                        this.Close();
                    };

                    _FileButton.ContextMenu.FindControl<MenuItem>("Button_Exit").Click += delegate
                    {
                        this.Close();
                    };

                    _FileButton.ContextMenu.FindControl<MenuItem>("Button_Load").Click += delegate
                    {
                        var TaskGetPathToFile = new OpenFileDialog()
                        {
                            Title = "Open DataBase",
                            Filters = null
                        }.ShowAsync((Window)this.VisualRoot);

                        string[]? PathToFile = TaskGetPathToFile.Result;
                        if (PathToFile != null)
                        {
                            StudentState.SetPathToFile(string.Join(@"\", PathToFile));
                        }
                        UpdateTable();
                    };
                }
            }
        }

        private void UpdateTable()
        {
            GetStates();
            if (Info_About_Students == null) return;
            int size = StudentState.GetListOfDisciplines().Count;
            for (int i = 0; i < StudentState.GetListOfDisciplines().Count; ++i)
            {
                DataGridColumn? dgl = new DataGridTextColumn();
                dgl.Header = StudentState.GetListOfDisciplines()[i];
                Info_About_Students.Columns.Add(dgl);
            }
            FillTable();
        }

        private void FillTable()
        {
            return;
        }

        // Discipline#Discipline#Discipline...#}
        // Name=1:0:2:2:1:2...:1;
        private void GetStates()
        {
            try
            {
                string Path = StudentState.GetPathToFile();
                StreamReader fReader = new StreamReader(Path);
                string Data = fReader.ReadToEnd();
                List<string> Disciplines = new List<string>();
                int cur_index = 0;
                string Temp = "";
                while (Data[cur_index] != '}')
                {
                    if (Data[cur_index] == '#')
                    {
                        Disciplines.Add(Temp);
                        Temp = "";
                        ++cur_index;
                        continue;
                    }
                    Temp += Data[cur_index];
                    ++cur_index;
                }
                StudentState.SetListOfDisciplines(Disciplines);
                Temp = "";
                List<short> Grades = new List<short>();
                string bName = "";
                StudentState bStudent;
                while (cur_index < Data.Length)
                {
                    if (Data[cur_index] == '=')
                    {
                        bName = Temp;
                        Temp = "";
                        ++cur_index;
                        continue;
                    }
                    if (Data[cur_index] == ':')
                    {
                        short t = Convert.ToInt16(Temp);
                        Grades.Add(t);
                        Temp = "";
                        ++cur_index;
                        continue;
                    }
                    if (Data[cur_index] == ';')
                    {
                        bStudent = new StudentState(bName, Grades);
                        bName = "";
                        Grades.Clear();
                        __states.Add(bStudent);
                    }
                    Temp += Data[cur_index];
                    ++cur_index;
                }
                fReader.Close();
            }
            catch
            {
                return;
            }
        }
    }
}