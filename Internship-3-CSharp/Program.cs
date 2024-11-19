using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Internship_3_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<Project, Task[]> projectTasks = new Dictionary<Project, Task[]>();

            var project1 = new Project
            {
                NameOfProject = "Project 1",
                DescriptionOfProject = "First project",
                DateStartOfProject = DateTime.Now,
                DateEndOfProject = DateTime.Now,
                StatusOfProject = Project.ProjectStatus.Active
            };

            var project2 = new Project
            {
                NameOfProject = "Project 2",
                DescriptionOfProject = "First project",
                DateStartOfProject = DateTime.Now,
                DateEndOfProject = DateTime.Now.AddMonths(1),
                StatusOfProject = Project.ProjectStatus.Active
            };

            var task1 = new Task
            {
                NameOfTask = "Task 1",
                DescriptionOfTask = "First task in project 1",
                DateEndOfTask = DateTime.Now.AddDays(15),
                DurationOfTask = DateTime.Now
            };

            var task2 = new Task
            {
                NameOfTask = "Task 2",
                DescriptionOfTask = "Second task in project 1",
                DateEndOfTask = DateTime.Now.AddDays(5),
                DurationOfTask = DateTime.Now
            };

            projectTasks[project1] = new Task[] { task1 };
            projectTasks[project2] = new Task[] { task2 };

            var choice = 0;
            do
            {
                Console.WriteLine("Odaberite: \n1 - Ispis svih projekata s pripadajućim zadacima\n2 - Dodavanje novog projekta\n3 - Brisanje projekta\n4 - Prikaz svih zadataka s rokom u sljedećih 7 dana\n5  -Prikaz  projekata filtriranih po status (samo aktivni, ili samo završeni, ili samo na čekanju)\n6 - Upravljanje pojedinim projektom ");
                choice = checkNumber(1, 6);
                switch (choice)
                {
                    case 1:
                        {
                            printProjectTasks(projectTasks);
                            break;
                        }
                    case 2:
                        {
                            AddNewProject(projectTasks);
                            break;
                        }
                    case 3:
                        {
                            DeleteProject(projectTasks);
                            break;
                        }
                    case 4:
                        {
                            printProjectThatWillEndInSevenDays(projectTasks);
                            break;
                        }
                    case 5:
                        {
                            printFilteredProject(projectTasks);
                            break;
                        }
                }
            } while (choice != 0);
            
            
            


            Console.ReadLine();
        }

        static void printProjectTasks(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            foreach (var project in projectTasks)
            {
                Console.WriteLine($"Project: {project.Key.NameOfProject}");
                foreach (var task in project.Value)
                {
                    Console.WriteLine($"  Task: {task.NameOfTask}");
                }
            }
        }

        static void AddNewProject(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Console.Write("Unesite ime projekta: ");
            var nameOfNewProject = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(nameOfNewProject) || checkNameOfProject(nameOfNewProject, projectTasks))
            {
                Console.Write("Neispravno ime projekta, unesite novo ime: ");
                nameOfNewProject = Console.ReadLine();
            }

            Console.Write("Unesite opis projekta: ");
            var descriptionOfNewProject = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(descriptionOfNewProject) || int.TryParse(descriptionOfNewProject, out _))
            {
                Console.Write("Unesite opis projekta: ");
                descriptionOfNewProject = Console.ReadLine();
            }


            Console.Write("Unesite datum pocetka projekta: ");
            DateTime dateStartOfNewProject = NewDate();
            while (dateStartOfNewProject < DateTime.Now)
            {
                Console.Write("Unesite datum pocetka projekta: ");
                dateStartOfNewProject = NewDate();
            }

            Console.Write("Unesite datum kraja projekta: ");
            DateTime dateEndOfNewProject = NewDate();
            while (dateEndOfNewProject < dateStartOfNewProject)
            {
                Console.Write("Unesite datum kraja projekta: ");
                dateEndOfNewProject = NewDate();
            } 

            var stausOfNewProject = Project.ProjectStatus.OnWait;
            if (dateStartOfNewProject == DateTime.Now)
            {
                stausOfNewProject = Project.ProjectStatus.Active;
            }
            
            var newProject = new Project {
                NameOfProject = nameOfNewProject,
                DescriptionOfProject = descriptionOfNewProject,
                DateStartOfProject = dateStartOfNewProject,
                DateEndOfProject = dateEndOfNewProject,
                StatusOfProject = stausOfNewProject
            };
            projectTasks[newProject] = new Task[] { };
        }
        
        static void DeleteProject(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            printProjectTasks(projectTasks);

            Console.Write("Unesite ime projekta kojeg zelite obrisati: ");
            var nameOfProjectToDelete = Console.ReadLine();

            if(!checkNameOfProject(nameOfProjectToDelete, projectTasks))
            {
                Console.WriteLine("Korisnik s tim imenom ne postoji!");
                return;
            }

            Project foundProjectToDelete = null;
            foreach (var project in projectTasks)
            {
                if(project.Key.NameOfProject == nameOfProjectToDelete)
                {
                    foundProjectToDelete = project.Key;
                    break;
                }
            }
            Console.WriteLine("Jeste sigurni da zelite obrisati projekt(y/n)?");
            if (!checkYesOrNo())
            {
                Console.WriteLine("Odustali od brisanja!");
                return;
            }
            projectTasks.Remove(foundProjectToDelete);
            Console.WriteLine("Korisnik uspjesno obrisan!");
        }

        static void printProjectThatWillEndInSevenDays(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Console.WriteLine("Zadaci sa rokom manjim od 7 dana: ");
            var brojac = 0;
            foreach (var project in projectTasks)
            {
                {
                    foreach (var task in project.Value)
                    {
                        if(DateTime.Now.Day - task.DateEndOfTask.Day <= 7)
                        {                       
                            Console.WriteLine($"{task.NameOfTask}");
                            brojac++;
                        } 
                    }
                }
            }
            if(brojac == 0) { Console.WriteLine("Nema zadataka sa rokom manjim od 7 dana!"); }
        }

        static void printFilteredProject(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Console.WriteLine("Koje projekte zelite vidjeti: ");
            Console.WriteLine("1 - aktivni");
            Console.WriteLine("2 - na cekanju");
            Console.WriteLine("3 - zavrseni");
            var choiceNumber = checkNumber(1, 3);

            switch (choiceNumber)
            {
                case 1:
                    {
                        printProjectTasksWithFilter(projectTasks, Project.ProjectStatus.Active);
                        break;
                    }
                case 2:
                    {
                        printProjectTasksWithFilter(projectTasks, Project.ProjectStatus.OnWait);
                        break;
                    }
                case 3:
                    {
                        printProjectTasksWithFilter(projectTasks, Project.ProjectStatus.Finished);
                        break;
                    }
            }
        }



        static DateTime NewDate()
        {
            DateTime Date;
            bool correct = DateTime.TryParse(Console.ReadLine(), out Date);
            while (!correct)
            {
                Console.WriteLine("Neispavan unos datuma, pokušajte ponovo: (YYYY-MM-DD)");
                correct = DateTime.TryParse(Console.ReadLine(), out Date);
            }
            return Date;
        }

        static bool checkNameOfProject(string name, Dictionary<Project, Task[]> projectTasks)
        {
            foreach (var project in projectTasks)
            {
                if(project.Key.NameOfProject == name)
                {
                    return true;
                }
            }
            return false;
        }

        static int checkNumber(int smallestNumber, int biggestNumber)
        {
            var EnteredNumber = 0;
            do
            {
                int.TryParse(Console.ReadLine(), out EnteredNumber);
            }while(EnteredNumber > biggestNumber || EnteredNumber < smallestNumber);
            
            return EnteredNumber;
        }

        static void printProjectTasksWithFilter(Dictionary<Project, Task[]> projectTasks, Project.ProjectStatus filterStatus)
        {
            foreach (var project in projectTasks)
            {
                if(project.Key.StatusOfProject == filterStatus)
                {
                    Console.WriteLine($"Project: {project.Key.NameOfProject}");
                }
            }
        }

        static bool checkYesOrNo()
        {
            var checkYesOrNo = Console.ReadLine().ToLower();
            if (checkYesOrNo == "y")
            {
                return true;
            }
            else
                return false;
        }
    }
}
