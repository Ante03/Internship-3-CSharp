using System;
using System.Collections.Generic;
using System.Linq;


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
                StartDateOfTask = DateTime.Now.AddDays(3),
                StatusOfTask = Task.TaskStatus.Postponed
            };

            var task2 = new Task
            {
                NameOfTask = "Task 2",
                DescriptionOfTask = "Second task in project 1",
                DateEndOfTask = DateTime.Now.AddDays(5),
                StartDateOfTask = DateTime.Now,
                StatusOfTask = Task.TaskStatus.Active
            };

            var task3 = new Task
            {
                NameOfTask = "Task 3",
                DescriptionOfTask = "Third task in project 1",
                DateEndOfTask = DateTime.Now.AddDays(15),
                StartDateOfTask = DateTime.Now.AddDays(4),
                StatusOfTask = Task.TaskStatus.Postponed
            };

            var task4 = new Task
            {
                NameOfTask = "Task 4",
                DescriptionOfTask = "Fourth task in project 1",
                DateEndOfTask = DateTime.Now.AddDays(6),
                StartDateOfTask = DateTime.Now.AddDays(1),
                StatusOfTask = Task.TaskStatus.Postponed
            };

            projectTasks[project1] = new Task[] { task1, task2 };
            projectTasks[project2] = new Task[] { task3, task4 };

            var choice = 0;
            do
            {
                Console.WriteLine("Odaberite: \n1 - Ispis svih projekata s pripadajućim zadacima\n2 - Dodavanje novog projekta\n3 - Brisanje projekta\n4 - Prikaz svih zadataka s rokom u sljedećih 7 dana\n5 - Prikaz  projekata filtriranih po status (samo aktivni, ili samo završeni, ili samo na čekanju)\n6 - Podizbornik\n0 - izlaz");
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
                Console.WriteLine("Projekt s tim imenom ne postoji!");
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
                        if(DateTime.Now.Day - task.DateEndOfTask.Day <= 7 && task.StatusOfTask != Task.TaskStatus.Done)
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

        static void SubMenu(Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            var subMenuChoice = 0;
            Console.Write("Unesite ime projekta kojem zelite ici u podizbornik: ");
            var nameOfProjectSubMenu = Console.ReadLine();
            Project projectToChange = checkProject(nameOfProjectSubMenu, projectTasks);
            do
            {
                Console.WriteLine("Odaberite: \n1 - Ispis svih zadataka unutar odabranog projekta\n2 - Prikaz detalja odabranog projekta\n3 - Uređivanje statusa projekta\n4 - Dodavanje zadatka unutar projekta\n5 - Brisanje zadatka iz projekta\n6 - Prikaz ukupno očekivanog vremena potrebnog za sve aktivne zadatke u projektu\n7 - zadaci od najkraceg do najduzeg\n8 - Sortiraj po prioritetu\n9 - Podizbornik\n0 - izlaz");
                subMenuChoice = CheckNumber(0, 9);
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
                            DeleteTask(nameOfProjectSubMenu, projectTasks);
                            break;
                        }
                    case 6:
                        {
                            TimeForActiveTasks(nameOfProjectSubMenu, projectTasks);
                            break;
                        }
                    case 7:
                        {
                            ShowTasksShortestToLongest(nameOfProjectSubMenu, projectTasks);
                            break;
                        }
                    case 8:
                        {
                            SortTasksByPriorities(nameOfProjectSubMenu, projectTasks);
                            break;                        }
                    case 9:
                        {
                            SubMenuTasks(nameOfProjectSubMenu, projectTasks);
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
            Project projectToChange = checkProject(nameOfProject, projectTasks);

            if (projectToChange.StatusOfProject == Project.ProjectStatus.Finished)
            {
                Console.WriteLine("Projekt zavrsen ne moze se mijenjati status!");
                return;
            }

            var ChangeStartDate = projectToChange.DateStartOfProject;
            var ChangeEndDate = projectToChange.DateEndOfProject;
            var ChangeStatus = projectToChange.StatusOfProject;

            Console.Write("Odaberite status projekta: ");
            Console.WriteLine("1 - aktivan");
            Console.WriteLine("2 - na cekanju");
            Console.WriteLine("3 - zavrsen");
            var choiceChange = CheckNumber(1, 3);
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


            Console.Write("Zelite li mijenjati status projekta(y/n): ");
            if (!CheckYesOrNo()) {
                Console.WriteLine("Odustali od promjene statusa!");
                return;
            }
            projectToChange.DateStartOfProject = ChangeStartDate;
            projectToChange.DateEndOfProject = ChangeEndDate;
            projectToChange.StatusOfProject = ChangeStatus;

            Console.WriteLine("Promjene su uspješno spremljene!"); 
        }

        static void AddNewTaskSubMenu(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = checkProject(nameOfProject, projectTasks);
            if (projectToChange.StatusOfProject == Project.ProjectStatus.Finished)
            {
                Console.WriteLine("Projekt zavrsen ne mogu se dodavati novi zadaci!");
                return;
            }

            Console.Write("Unesite ime zadatka: ");
            var nameOfNewTask = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(nameOfNewTask) || CheckNameOfTask(nameOfNewTask, projectTasks[projectToChange]))
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
                Console.Write($"Unesite datum pocetka zadatka izmedu {projectToChange.DateStartOfProject}{projectToChange.DateEndOfProject}: ");
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
            if (dateStartOfNewTask <= DateTime.Now)
            {
                stausOfNewtask = Task.TaskStatus.Active;
            }

            
            var priorityOfTask = Task.TaskPriority.Low;
            Console.WriteLine("Koji prioritet je zadatak: \n1 - niski\n2 - srednji\n3 - visoki");
            var choicePriority = CheckNumber(1, 3);
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

            var existingTasks = projectTasks[projectToChange];
            var updatedTasks = existingTasks.Append(taskToAdd).ToArray();
            projectTasks[projectToChange] = updatedTasks;

            Console.WriteLine("Zadatak uspjesno dodan!");
        }

        static void DeleteTask(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = checkProject(nameOfProject, projectTasks);
            if (projectToChange.StatusOfProject == Project.ProjectStatus.Finished)
            {
                Console.WriteLine("Projekt zavrsen ne mogu se dodavati novi zadaci!");
                return;
            }

            Console.Write("Unesite ime zadatka kojeg zelite obrisat: ");
            var nameOfTaskToDelete = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(nameOfTaskToDelete) || CheckNameOfProject(nameOfTaskToDelete, projectTasks))
            {
                Console.Write("Neispravno ime zadatka, unesite ponovo ime: ");
                nameOfTaskToDelete = Console.ReadLine();
            }

            Task taskToDelete = null;
            foreach (var task in projectTasks[projectToChange])
            {
                if (task.NameOfTask == nameOfTaskToDelete)
                {
                    taskToDelete = task;
                    break;
                }
            }
            Console.WriteLine("Jeste sigurni da zelite obrisati zadatak(y/n): ");
            if (!CheckYesOrNo()) {
                Console.WriteLine("Odustali od brisanja!");
                return;
            }
            var tasks = projectTasks[projectToChange];
            var updatedTasks = tasks.Where(task => task.NameOfTask != nameOfTaskToDelete).ToArray();
            projectTasks[projectToChange] = updatedTasks;
            Console.WriteLine("Zadatak uspjesno obrisan!");
            bool areAllTasksDone = projectTasks[projectToChange].All(task => task.StatusOfTask == Task.TaskStatus.Done);
            if (areAllTasksDone)
            {
                projectToChange.StatusOfProject = Project.ProjectStatus.Finished;
            }
        }

        static void TimeForActiveTasks(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = checkProject(nameOfProject, projectTasks);
            if (projectToChange.StatusOfProject == Project.ProjectStatus.Finished)
            {
                Console.WriteLine("Projekt zavrsen ne mogu se dodavati novi zadaci!");
                return;
            }

            TimeSpan difference = DateTime.Now - DateTime.Now;
            TimeSpan temp;
            foreach (var task in projectTasks[projectToChange])
            {
                if(task.StatusOfTask == Task.TaskStatus.Active)
                {
                    temp = task.DateEndOfTask - task.StartDateOfTask;
                    difference +=  temp;
                }
            }
            Console.WriteLine($"Ukupno minuta za aktivne zadatke u ovom projektu: {difference.TotalMinutes}");
        }

        static void SubMenuTasks(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            var subMenuChoiceTasks = 0;
            Console.Write("Unesite ime zadatka kojem zelite ici u podizbornik: ");
            var nameOfTaskSubMenu = Console.ReadLine();
            Project projectToChange = checkProject(nameOfProject, projectTasks);
            if (!projectTasks[projectToChange].Any(task => task.NameOfTask == nameOfTaskSubMenu))
            {
                Console.WriteLine("Zadatak s tim imenom ne postoji!");
                return;
            }
            
            do
            {
                Console.WriteLine("Odaberite: \n1 - Prikaz detalja odabranog zadatka\n2 - Uređivanje statusa zadatka\n0 - Izlaz");
                subMenuChoiceTasks = CheckNumber(0, 3);
                switch (subMenuChoiceTasks)
                {
                    case 0:
                        {
                            return;
                        }
                    case 1:
                        {
                            PrintDetailsOfTasks(nameOfTaskSubMenu, nameOfProject, projectTasks);
                            break;
                        }
                    case 2:
                        {
                            MakeChangeOnTaskStatus(nameOfTaskSubMenu, nameOfProject, projectTasks);
                            break;
                        }
                }
            } while (subMenuChoiceTasks != 0);
        }

        static void PrintDetailsOfTasks(string nameOfTasks, string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = checkProject(nameOfProject, projectTasks);
            Task taskToShow = checkTask(nameOfTasks, projectToChange, projectTasks);


            TimeSpan durationOfTask = taskToShow.DateEndOfTask - taskToShow.StartDateOfTask;
            Console.WriteLine("Ime  -   Opis    -   Trajanje    -   Status     -    Prioritet");
            Console.WriteLine($"{taskToShow.NameOfTask}{taskToShow.DescriptionOfTask}{durationOfTask.TotalMinutes}{taskToShow.StatusOfTask}{taskToShow.PriorityOfTask}");

        }

        static void MakeChangeOnTaskStatus(string nameOfTasks, string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = checkProject(nameOfProject, projectTasks);
            Task taskToShow = checkTask(nameOfTasks, projectToChange, projectTasks);

            var choiceMakeChangeTaskStatus = 0;
            if(taskToShow.StatusOfTask == Task.TaskStatus.Done)
            {
                Console.WriteLine("Zadatak je vec zavrsen!");
                return;
            }
            Console.WriteLine("Unesite koji status zelite postaviti: \n1 - aktivan\n2 - odgoden\n3 - zavrsen");
            choiceMakeChangeTaskStatus = CheckNumber(1, 3);
            switch (choiceMakeChangeTaskStatus)
            {
                case 1:
                    {
                        taskToShow.StatusOfTask = Task.TaskStatus.Active;
                        Console.WriteLine("Zadatak postavljen na aktivno!");
                        if (taskToShow.StartDateOfTask > DateTime.Now) { taskToShow.StartDateOfTask = DateTime.Now; }
                        break;
                    }
                case 2:
                    {
                        taskToShow.StatusOfTask = Task.TaskStatus.Postponed;
                        Console.WriteLine("Zadatak postavljen na odgodeno!");

                        break;
                    }
                case 3:
                    {
                        taskToShow.StatusOfTask = Task.TaskStatus.Done;
                        Console.WriteLine("Zadatak postavljen na zavrseno!");

                        if (taskToShow.StartDateOfTask > DateTime.Now)
                        {
                            taskToShow.StartDateOfTask = DateTime.Now;
                            taskToShow.DateEndOfTask = DateTime.Now;
                        }
                        if (taskToShow.DateEndOfTask > DateTime.Now) { taskToShow.DateEndOfTask = DateTime.Now; }
                        bool areAllTasksDone = projectTasks[projectToChange].All(task => task.StatusOfTask == Task.TaskStatus.Done);
                        if (areAllTasksDone)
                        {
                            projectToChange.StatusOfProject = Project.ProjectStatus.Finished;
                        }
                        break;
                    }
            }

        }

        static void ShowTasksShortestToLongest(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = checkProject(nameOfProject, projectTasks);

            var sortedTasks = projectTasks[projectToChange]
            .OrderBy(task => (task.DateEndOfTask - task.StartDateOfTask).TotalMinutes)
            .ToList();

            Console.WriteLine("Zadaci u projektu sortirani od najkraćeg do najdužeg trajanja:");
            foreach (var task in sortedTasks)
            {
                var duration = task.DateEndOfTask - task.StartDateOfTask;
                Console.WriteLine($"{task.NameOfTask}: {duration.TotalMinutes} minuta");
            }

        }

        static void SortTasksByPriorities(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Console.Clear();
            Project projectToChange = checkProject(nameOfProject, projectTasks);

            var sortedTasks = projectTasks[projectToChange]
            .OrderBy(task => task.PriorityOfTask)
            .ToList();

            Console.WriteLine("Zadaci u projektu sortirani po prioritetu:");
            foreach (var task in sortedTasks)
            {
                Console.WriteLine($"{task.NameOfTask}: {task.PriorityOfTask}");
            }
        }

        static Task checkTask(string nameOfTasks, Project projectToChange, Dictionary<Project, Task[]> projectTasks)
        {
            Task taskToShow = null;
            foreach (var task in projectTasks[projectToChange])
            {
                if (task.NameOfTask == nameOfTasks)
                {
                    taskToShow = task;
                    break;
                }
            }
            if (taskToShow == null)
            {
                Console.WriteLine("Zadatak s unesenim imenom nije pronađen.");
                return null;
            }
            if (taskToShow.StatusOfTask == Task.TaskStatus.Done)
            {
                Console.WriteLine("Zadatak s unesenim imenom je zavrsen.");
                return null;
            }
            return taskToShow;
        }

        static Project checkProject(string nameOfProject, Dictionary<Project, Task[]> projectTasks)
        {
            Project projectToChange = null;
            foreach (var project in projectTasks)
            {
                if (project.Key.NameOfProject == nameOfProject)
                {
                    projectToChange = project.Key;
                    return projectToChange;
                }
            }
            return null;
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

        static bool CheckNameOfTask(string taskName, Task[] tasks)
        {
            foreach (var task in tasks)
            {
                if (task.NameOfTask == taskName)
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


