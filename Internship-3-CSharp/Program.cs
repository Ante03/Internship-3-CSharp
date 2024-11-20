using Internship_3_CSharp;
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
                DateStartOfProject = DateTime.Now.AddDays(1),
                DateEndOfProject = DateTime.Now.AddDays(5),
                StatusOfProject = Project.ProjectStatus.OnWait
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
                StartDateOfTask = DateTime.Now
            };

            var task2 = new Task
            {
                NameOfTask = "Task 2",
                DescriptionOfTask = "Second task in project 1",
                DateEndOfTask = DateTime.Now.AddDays(5),
                StartDateOfTask = DateTime.Now
            };

            projectTasks[project1] = new Task[] { task1 };
            projectTasks[project2] = new Task[] { task2 };

            var choice = 0;
            do
            {
                Console.WriteLine("Odaberite: \n1 - Ispis svih projekata s pripadajućim zadacima\n2 - Dodavanje novog projekta\n3 - Brisanje projekta\n4 - Prikaz svih zadataka s rokom u sljedećih 7 dana\n5 - Prikaz  projekata filtriranih po status (samo aktivni, ili samo završeni, ili samo na čekanju)\n6 - Podizbornik");
                choice = CheckNumber(0, 6);
                switch (choice)
                {
                    case 0:
                        {
                            return;
                        }
                    case 1:
                        {
                            PrintProjectTasks(projectTasks);
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
                            PrintProjectThatWillEndInSevenDays(projectTasks);
                            break;
                        }
                    case 5:
                        {
                            PrintFilteredProject(projectTasks);
                            break;
                        }
                    case 6:
                        {
                            SubMenu(projectTasks);
                            break;
                        }
                    
                }
            } while (choice != 0);
            
            
            


            Console.ReadLine();
        }

        static void PrintProjectTasks(Dictionary<Project, Task[]> projectTasks)
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
            while (string.IsNullOrWhiteSpace(nameOfNewProject) || CheckNameOfProject(nameOfNewProject, projectTasks))
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
            while (dateEndOfNewProject <= dateStartOfNewProject)
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
            PrintProjectTasks(projectTasks);

            Console.Write("Unesite ime projekta kojeg zelite obrisati: ");
            var nameOfProjectToDelete = Console.ReadLine();

            if(!CheckNameOfProject(nameOfProjectToDelete, projectTasks))
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
            if (!CheckYesOrNo())
            {
                Console.WriteLine("Odustali od brisanja!");
                return;
            }
            projectTasks.Remove(foundProjectToDelete);
            Console.WriteLine("Korisnik uspjesno obrisan!");
        }

        static void PrintProjectThatWillEndInSevenDays(Dictionary<Project, Task[]> projectTasks)
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

        static void PrintFilteredProject(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Console.WriteLine("Koje projekte zelite vidjeti: ");
            Console.WriteLine("1 - aktivni");
            Console.WriteLine("2 - na cekanju");
            Console.WriteLine("3 - zavrseni");
            var choiceNumber = CheckNumber(1, 3);

            switch (choiceNumber)
            {
                case 1:
                    {
                        PrintProjectTasksWithFilter(projectTasks, Project.ProjectStatus.Active);
                        break;
                    }
                case 2:
                    {
                        PrintProjectTasksWithFilter(projectTasks, Project.ProjectStatus.OnWait);
                        break;
                    }
                case 3:
                    {
                        PrintProjectTasksWithFilter(projectTasks, Project.ProjectStatus.Finished);
                        break;
                    }
            }
        }

        static void MakeChangeOnProject(Dictionary<Project, Task[]> projectTasks)
        {
            PrintProjectTasks(projectTasks);
            Console.WriteLine("Unesite ime projekta kojeg zelite uredivat: ");
            var nameOfProjectToChange = Console.ReadLine();
            Project projectToChange = null;
            if (!CheckNameOfProject(nameOfProjectToChange, projectTasks))
            {
                Console.WriteLine("ne postoji");
                return;
            }
            foreach (var project in projectTasks)
            {
                if (project.Key.NameOfProject == nameOfProjectToChange)
                {
                    projectToChange = project.Key;
                    break;
                }
            }
            if (projectToChange.StatusOfProject == Project.ProjectStatus.Finished)
            {
                Console.WriteLine("Projekt zavrsen ne more se uredivat");
                return;
            }

            Console.Write("Zelite li mijenjati ime(y/n): ");
            var ChangeName = projectToChange.NameOfProject;
            if (CheckYesOrNo())
            {
                do
                {
                    Console.Write("Unesite novo ime: ");
                    ChangeName = Console.ReadLine();
                } while (CheckNameOfProject(ChangeName, projectTasks));
            }

            Console.Write("Zelite li mijenjati opis(y/n): ");
            var ChangeDescription = projectToChange.DescriptionOfProject;
            if (CheckYesOrNo())
            {
                do
                {
                    Console.Write("Unesite opis projekta: ");
                    ChangeDescription = Console.ReadLine();
                } while (string.IsNullOrWhiteSpace(ChangeDescription) || int.TryParse(ChangeDescription, out _));
            }

            var ChangeStartDate = projectToChange.DateStartOfProject;
            var ChangeEndDate = projectToChange.DateEndOfProject;
            if (projectToChange.StatusOfProject != Project.ProjectStatus.Active)
            {
                Console.Write("Zelite li mijenjati datum pocetka projekta(y/n): ");
                if (CheckYesOrNo())
                {
                    do
                    {
                        Console.Write("Unesite datum pocetka projekta: ");
                        ChangeStartDate = NewDate();
                    } while (ChangeStartDate < DateTime.Now);
                }
                
                if (ChangeEndDate < ChangeStartDate)
                {
                    do
                    {
                        Console.Write("Unesite datum zavrsetka projekta: ");
                        ChangeEndDate = NewDate();
                    } while (ChangeEndDate < ChangeStartDate);
                }
                else
                {
                    Console.Write("Zelite li mijenjati datum zavrsetka projekta(y/n): ");
                    if (CheckYesOrNo())
                    {
                        do
                        {
                            Console.Write("Unesite datum zavrsetka projekta: ");
                            ChangeEndDate = NewDate();
                        } while (ChangeEndDate < ChangeStartDate);
                    }
                }
            }

            

            Console.Write("Zelite li mijenjati status projekta(y/n): ");
            var ChangeStatus = projectToChange.StatusOfProject;
            if (CheckYesOrNo())
            {
                var choiceChange = 0;
                do
                {
                    Console.Write("Odaberite status projekta: ");
                    Console.WriteLine("1 - aktivan");
                    Console.WriteLine("2 - na cekanju");
                    Console.WriteLine("3 - zavrsen");
                    choiceChange = CheckNumber(1, 3);

                    switch (choiceChange)
                    {
                        case 1:
                            {
                                ChangeStatus = Project.ProjectStatus.Active;
                                if(ChangeStartDate > DateTime.Now)
                                {
                                    ChangeStartDate = DateTime.Now;
                                }
                                break;
                            }
                        case 2:
                            {
                                ChangeStatus = Project.ProjectStatus.OnWait;
                                break;
                            }
                        case 3:
                            {
                                ChangeStatus = Project.ProjectStatus.Finished;
                                ChangeEndDate = DateTime.Now;
                                if(ChangeStartDate > ChangeEndDate)
                                {
                                    ChangeStartDate = ChangeEndDate;
                                    Console.WriteLine("Projekt postavljen da je gotov pa se automatski kraj projekta postavlja na trenutno vrijeme\nAko je vrijeme pocetka bilo nakon sada ono ce isto biti postavljeno na sada");
                                }
                                break;
                            }
                    }

                } while (choiceChange != 1 && choiceChange != 2 && choiceChange != 3);

                projectToChange.NameOfProject = ChangeName;
                projectToChange.DescriptionOfProject = ChangeDescription;
                projectToChange.DateStartOfProject = ChangeStartDate;
                projectToChange.DateEndOfProject = ChangeEndDate;
                projectToChange.StatusOfProject = ChangeStatus;

                Console.WriteLine("Promjene su uspješno spremljene!");
            }
            else
            {
                Console.WriteLine("Projekt nije pronaden ili je zavrsen!");
            }
        }

        static void SubMenu(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            var subMenuChoice = 0;
            Console.Write("Unesite ime projekta kojem zelite ici u podizbornik: ");
            var nameOfProjectSubMenu = Console.ReadLine();
            if (!CheckNameOfProject(nameOfProjectSubMenu, projectTasks))
            {
                Console.WriteLine("Projekt s tim imenom ne postoji!");
                return;
            }
            do
            {
                Console.WriteLine("Odaberite: \n1 - Ispis svih zadataka unutar odabranog projekta\n2 - Prikaz detalja odabranog projekta\n3 - Uređivanje statusa projekta\n4 - Dodavanje zadatka unutar projekta\n5 - Brisanje zadatka iz projekta\n6 - Prikaz ukupno očekivanog vremena potrebnog za sve aktivne zadatke u projektu\n7 - Podizbornik");
                subMenuChoice = CheckNumber(0, 6);
                switch (subMenuChoice)
                {
                    case 0:
                        {
                            return;
                        }
                    case 1:
                        {
                            SubMenuPrintProjectTasks(nameOfProjectSubMenu, projectTasks);
                            break;
                        }
                    case 2:
                        {
                            PrintDetailsOfProject(nameOfProjectSubMenu, projectTasks);
                            break;
                        }
                    case 3:
                        {
                            MakeChangeOnStatusSubMenu(nameOfProjectSubMenu, projectTasks);
                            break;
                        }
                    case 4:
                        {
                            AddNewTaskSubMenu(nameOfProjectSubMenu, projectTasks);
                            break;
                        }
                    case 5:
                        {
                            PrintFilteredProject(projectTasks);
                            break;
                        }
                    case 6:
                        {
                            MakeChangeOnProject(projectTasks);
                            break;
                        }
                }
            } while (subMenuChoice != 0);

        }

        static void SubMenuPrintProjectTasks(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            foreach (var project in projectTasks)
            {
                if(project.Key.NameOfProject == nameOfProject)
                {
                    Console.WriteLine($"Project: {project.Key.NameOfProject}");
                    foreach (var task in project.Value)
                    {
                        Console.WriteLine($"  Task: {task.NameOfTask}");
                    }
                }   
            }
        }

        static void PrintDetailsOfProject(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            foreach (var project in projectTasks)
            {
                if (project.Key.NameOfProject == nameOfProject)
                {
                    Console.WriteLine("Ime    -   Opis     -   Pocetak    -   Kraj    -   Status");
                    Console.WriteLine($"{project.Key.NameOfProject}    -   {project.Key.DescriptionOfProject}    -   {project.Key.DateStartOfProject.ToString("dd/MM/yyyy")}    -   {project.Key.DateEndOfProject.ToString("dd/MM/yyyy")}    -   {project.Key.StatusOfProject}");
                }
            }

        }

        static void MakeChangeOnStatusSubMenu(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {

            Console.Clear();
            Project projectToChange = null;
            foreach (var project in projectTasks)
            {
                if (project.Key.NameOfProject == nameOfProject)
                {
                    projectToChange = project.Key;
                    break;
                }
            }

            if(projectToChange.StatusOfProject == Project.ProjectStatus.Finished)
            {
                Console.WriteLine("Projekt zavrsen ne moze se mijenjati status!");
                return;
            }

            var ChangeStartDate = projectToChange.DateStartOfProject;
            var ChangeEndDate = projectToChange.DateEndOfProject;
            Console.Write("Zelite li mijenjati status projekta(y/n): ");
            var ChangeStatus = projectToChange.StatusOfProject;

            var choiceChange = 0;
            do
            {
                Console.Write("Odaberite status projekta: ");
                Console.WriteLine("1 - aktivan");
                Console.WriteLine("2 - na cekanju");
                Console.WriteLine("3 - zavrsen");
                choiceChange = CheckNumber(1, 3);

                switch (choiceChange)
                {
                    case 1:
                        {
                            ChangeStatus = Project.ProjectStatus.Active;
                            if (ChangeStartDate > DateTime.Now)
                            {
                                ChangeStartDate = DateTime.Now;
                            }
                            break;
                        }
                    case 2:
                        {
                            ChangeStatus = Project.ProjectStatus.OnWait;
                            break;
                        }
                    case 3:
                        {
                            ChangeStatus = Project.ProjectStatus.Finished;
                            ChangeEndDate = DateTime.Now;
                            if (ChangeStartDate > ChangeEndDate)
                            {
                                ChangeStartDate = ChangeEndDate;
                                Console.WriteLine("Projekt postavljen da je gotov pa se automatski kraj projekta postavlja na trenutno vrijeme\nAko je vrijeme pocetka bilo nakon sada ono ce isto biti postavljeno na sada");
                            }
                            break;
                        }
                }

            } while (choiceChange != 1 && choiceChange != 2 && choiceChange != 3);

                projectToChange.DateStartOfProject = ChangeStartDate;
                projectToChange.DateEndOfProject = ChangeEndDate;
                projectToChange.StatusOfProject = ChangeStatus;

                Console.WriteLine("Promjene su uspješno spremljene!");
         
        }

        static void AddNewTaskSubMenu(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = null;
            foreach (var project in projectTasks)
            {
                if (project.Key.NameOfProject == nameOfProject)
                {
                    projectToChange = project.Key;
                    break;
                }
            }
            if(projectToChange.StatusOfProject == Project.ProjectStatus.Finished)
            {
                Console.WriteLine("Projekt zavrsen ne mogu se dodavati novi zadaci!");
                return;
            }

            Console.Write("Unesite ime zadatka: ");
            var nameOfNewTask = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(nameOfNewTask) || CheckNameOfProject(nameOfNewTask, projectTasks))
            {
                Console.Write("Neispravno ime zadatka, unesite novo ime: ");
                nameOfNewTask = Console.ReadLine();
            }

            Console.Write("Unesite opis zadatka: ");
            var descriptionOfNewTask = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(descriptionOfNewTask) || int.TryParse(descriptionOfNewTask, out _))
            {
                Console.Write("Unesite opis zadatka: ");
                descriptionOfNewTask = Console.ReadLine();
            }


            Console.Write("Unesite datum pocetka zadatka: ");
            DateTime dateStartOfNewTask = NewDate();
            while (dateStartOfNewTask < DateTime.Now || dateStartOfNewTask < projectToChange.DateStartOfProject || dateStartOfNewTask > projectToChange.DateEndOfProject)
            {
                Console.Write("Unesite datum pocetka zadatka: ");
                dateStartOfNewTask = NewDate();
            }

            Console.Write("Unesite datum kraja zadatka: ");
            DateTime dateEndOfNewTask = NewDate();
            while (dateEndOfNewTask <= dateStartOfNewTask || dateEndOfNewTask > projectToChange.DateEndOfProject)
            {
                Console.Write("Unesite datum kraja zadatka: ");
                dateEndOfNewTask = NewDate();
            }

            var stausOfNewtask = Task.TaskStatus.Postponed;
            if (dateStartOfNewTask == DateTime.Now)
            {
                stausOfNewtask = Task.TaskStatus.Active;
            }

            var choicePriority = 0;
            var priorityOfTask = Task.TaskPriority.Low;
            do
            {
                Console.WriteLine("Koji prioritet je zadatak: \n1 - niski\n2 - srednji\n 3 - visoki");
                choicePriority = CheckNumber(1, 3);
            } while (choicePriority != 1 && choicePriority != 2 && choicePriority != 3);
            switch (choicePriority)
            {
                case 1:
                    {
                        break;
                    }
                case 2:
                    {
                        priorityOfTask = Task.TaskPriority.Middle;
                        break;
                    }
                case 3:
                    {
                        priorityOfTask = Task.TaskPriority.High;
                        break;
                    }
            }

            var taskToAdd = new Task
            {
                NameOfTask = nameOfNewTask,
                DescriptionOfTask = descriptionOfNewTask,
                DateEndOfTask = dateEndOfNewTask,
                StartDateOfTask = dateStartOfNewTask,
                PriorityOfTask = priorityOfTask,
                StatusOfTask = stausOfNewtask
            };

            if (projectTasks.ContainsKey(projectToChange))
            {
                var existingTasks = projectTasks[projectToChange];
                var updatedTasks = existingTasks.Append(taskToAdd).ToArray();
                projectTasks[projectToChange] = updatedTasks;
            }
            else
            {
                projectTasks[projectToChange] = new Task[] { taskToAdd };
            }

            Console.WriteLine("Zadatak uspjesno dodan!");


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

        static bool CheckNameOfProject(string name, Dictionary<Project, Task[]> projectTasks)
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

        static int CheckNumber(int smallestNumber, int biggestNumber)
        {
            var EnteredNumber = -1;
            do
            {
                int.TryParse(Console.ReadLine(), out EnteredNumber);
            }while(EnteredNumber > biggestNumber || EnteredNumber < smallestNumber);
            
            return EnteredNumber;
        }

        static void PrintProjectTasksWithFilter(Dictionary<Project, Task[]> projectTasks, Project.ProjectStatus filterStatus)
        {
            foreach (var project in projectTasks)
            {
                if(project.Key.StatusOfProject == filterStatus)
                {
                    Console.WriteLine($"Project: {project.Key.NameOfProject}");
                }
            }
        }

        static bool CheckYesOrNo()
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


